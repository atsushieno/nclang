using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NClang;

namespace CApiGenerator
{
	class HeaderParser
	{
		static List<string> processed = new List<string> ();

		public List<NamedConstruct> Run (CApiGeneratorOptions opts, IEnumerable<ClangTranslationUnit> tus)
		{
			var members = new List<NamedConstruct> ();
			ClassDeclaration current = null;
			CXXAccessSpecifier current_access_specifier = default (CXXAccessSpecifier);
			string current_namespace = "";
			List<TypeParameter> current_type_parameters = null;

			foreach (var tu in tus) {
				for (int i = 0; i < tu.DiagnosticCount; i++)
					Console.Error.WriteLine ("[diag] " + tu.GetDiagnostic (i).Spelling);

				Func<ClangCursor, ClangCursor, IntPtr, ChildVisitResult> func = null, doVisit = null;
				Exception error = null;
				func = (cursor, parent, clientData) => {
					try {
						return doVisit (cursor, parent, clientData);
					} catch (Exception ex) {
						error = ex;
						return ChildVisitResult.Break;
					}
				};
				doVisit = (cursor, parent, clientData) => {
					if (cursor.Location.FileLocation?.File?.FileName == null)
						return ChildVisitResult.Continue; // skip non-file input
					if (!opts.ShouldParse (cursor.Location.FileLocation?.File?.FileName))
						return ChildVisitResult.Continue; // skip unmatched files.
					string id = cursor.Kind + " " + cursor.Location;
					if (processed.Contains (id))
						return ChildVisitResult.Continue;
					processed.Add (id);

					if (cursor.Kind == CursorKind.Namespace) {
						current_namespace = cursor.Spelling;
						cursor.VisitChildren (func, IntPtr.Zero);
						current_namespace = "";
						return ChildVisitResult.Continue;
					} else if (cursor.Kind == CursorKind.TypeAliasDeclaration
					           || cursor.Kind == CursorKind.TypeAliasTemplateDecl
						   || cursor.Kind == CursorKind.ClassTemplatePartialSpecialization
						   || cursor.Kind == CursorKind.NonTypeTemplateParameter) {
						// no need to care.
						return ChildVisitResult.Continue;
					} else if (cursor.Kind == CursorKind.CXXFinalAttribute) {
						// "class XXX final"
						return ChildVisitResult.Continue;
					} else if (cursor.Kind == CursorKind.ClassDeclaration
						 || cursor.Kind == CursorKind.ClassTemplate
						 // FIXME: examine if they should apply here...
						 || cursor.Kind == CursorKind.StructDeclaration
						 || cursor.Kind == CursorKind.UnionDeclaration) {
						current_type_parameters = cursor.Kind == CursorKind.ClassTemplate ? new List<TypeParameter> () : null;
						current = new ClassDeclaration () {
							Namespace = current_namespace,
							Name = cursor.Spelling,
							TypeParameters = current_type_parameters,
							SourceFile = cursor.Location.FileLocation.File.FileName,
							Line = cursor.Location.FileLocation.Line,
							Column = cursor.Location.FileLocation.Column,
						};
						members.Add (current);
						cursor.VisitChildren (func, IntPtr.Zero);
						current = null;
						return ChildVisitResult.Continue;
					} else if (cursor.Kind == CursorKind.CXXAccessSpecifier) {
						current_access_specifier = cursor.CxxAccessSpecifier;
					} else if (cursor.Kind == CursorKind.CXXBaseSpecifier) {
						if (current != null)
							current.BaseType = cursor.Spelling;
						return ChildVisitResult.Continue;
					} else if (cursor.Kind == CursorKind.TemplateTypeParameter) {
						Console.Error.WriteLine ($"    --- <{cursor.Spelling}> {cursor.TemplateCursorKind} | [{cursor.SpecializedCursorTemplate.Spelling}] at {cursor.Location.SpellingLocation}");
						if (current_type_parameters != null)
							current_type_parameters.Add (new TypeParameter {
								Name = cursor.Spelling,
							});
					} else if (cursor.Kind == CursorKind.FieldDeclaration || cursor.Kind == CursorKind.VarDeclaration) {
						var f = new Variable () {
							Access = current_access_specifier,
							Kind = cursor.Kind,
							Type = cursor.CursorType.Spelling,
							ArraySize = cursor.CursorType.ArraySize,
							SizeOf = cursor.CursorType.SizeOf,
							IsStatic = cursor.IsCxxStatic, //FIXME: it doesn't work
							IsConst = cursor.IsCxxConst, //FIXME: it doesn't work
							Namespace = current_namespace,
							Name = cursor.Spelling,
							SourceFile = cursor.Location.FileLocation.File.FileName,
							Line = cursor.Location.FileLocation.Line,
							Column = cursor.Location.FileLocation.Column,
						};
						if (current != null) {
							current.Fields.Add (f);
							return ChildVisitResult.Continue;
						} else if (cursor.SemanticParent == null || (cursor.SemanticParent.Kind != CursorKind.ClassDeclaration && cursor.SemanticParent.Kind != CursorKind.ClassTemplate)) {
							members.Add (f);
							return ChildVisitResult.Continue;
						}
					} else if (cursor.Kind == CursorKind.CXXMethod || cursor.Kind == CursorKind.ConversionFunction || cursor.Kind == CursorKind.FunctionDeclaration || cursor.Kind == CursorKind.FunctionTemplate) {
						current_type_parameters = cursor.Kind == CursorKind.FunctionTemplate ? new List<TypeParameter> () : null;
						int ac = 0;
						var f = new Function () {
							Namespace = current_namespace,
							Name = cursor.Spelling,
							Kind = cursor.Kind,
							TypeParameters = Enumerable.Range (0, (ac = cursor.TemplateArgumentCount) < 0 ? 0 : ac)
										   .Select (i => cursor.GetTemplateArgumentType (i))
										   .Select (a => new TypeParameter { Name = a.Spelling })
										   .ToList (),
							Access = current_access_specifier,
							Return = cursor.ResultType.Spelling,
							IsVirtual = cursor.IsCxxVirtual,
							IsPureVirtual = cursor.IsCxxPureVirtual,
							SourceFile = cursor.Location.FileLocation.File.FileName,
							Line = cursor.Location.FileLocation.Line,
							Column = cursor.Location.FileLocation.Column,
							// wait, FunctionTemplate comes with no arguments??
							Parameters = Enumerable.Range (0, (ac = cursor.ArgumentCount) < 0 ? 0 : ac)
									 .Select (i => cursor.GetArgument (i))
									 .Select (a => new Variable () {
										 Name = a.Spelling,
										 Type = a.CursorType.Spelling,
									 })
									 .ToArray ()
						};
						if (current != null) {
							current.Functions.Add (f);
						} else if (cursor.SemanticParent == null || (cursor.SemanticParent.Kind != CursorKind.ClassDeclaration && cursor.SemanticParent.Kind != CursorKind.ClassTemplate)) {
							members.Add (f);
						} // otherwise it's a member definition
						return ChildVisitResult.Continue;
					} else if (cursor.Kind == CursorKind.Constructor) {
						var f = new Function () {
							Name = cursor.Spelling,
							Access = current_access_specifier,
							Kind = cursor.Kind,
							Return = null,
							SourceFile = cursor.Location.FileLocation.File.FileName,
							Line = cursor.Location.FileLocation.Line,
							Column = cursor.Location.FileLocation.Column,
							Parameters = Enumerable.Range (0, cursor.ArgumentCount)
										 .Select (i => cursor.GetArgument (i))
										 .Select (a => new Variable () {
											 Name = a.Spelling,
											 Type = a.CursorType.Spelling,
										 })
										 .ToArray ()
						};
						if (current != null) {
							current.Constructors.Add (f);
						} else if (cursor.SemanticParent == null || (cursor.SemanticParent.Kind != CursorKind.ClassDeclaration && cursor.SemanticParent.Kind != CursorKind.ClassTemplate)) {
							members.Add (f);
						} // otherwise it's a member definition
						return ChildVisitResult.Continue;
					} else {
						switch (cursor.Kind) {
						case CursorKind.TypedefDeclaration:
						case CursorKind.Destructor:
						case CursorKind.StructDeclaration:
						case CursorKind.FriendDecl:
						case CursorKind.EnumDeclaration:
							// they are safe to ignore
							return ChildVisitResult.Continue;
						default:
							Console.Error.WriteLine ($"{cursor.Location.SpellingLocation} Unhandled token: [{cursor.Kind}] {cursor.Spelling}");
							break;
						}
					}

					return ChildVisitResult.Continue;
				};
				tu.GetCursor ().VisitChildren (func, IntPtr.Zero);
				if (error != null)
					throw error;
			}

			return members;
		}
	}
}
