using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangIndexActionTest
	{
		[Test]
		public void IndexTranslationUnit ()
		{
			string filename = "ClangIndexActionTest.IndexTranslationUnit.c";
			string code = @"#include <stdio.h>
int bar () { return 3; } int foo () { return 5; } int main () { return foo () * bar (); }";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var a = idx.CreateIndexAction ();
				var cb = new ClangIndexerCallbacks ();
				int abortRequested = 0;
				cb.AbortQuery += (arg) => { abortRequested++; return false; };

				int diagnosticCount = -1;
				cb.Diagnostic += (arg1, arg2) => diagnosticCount = arg2.Count;
				bool enteredMainFile = false;
				cb.EnteredMainFile += (arg1, arg2) => { enteredMainFile = true; return new ClangIndex.ClientFile (arg2.Handle); };

				// not practically tested here...
				cb.ImportedAstFile += (arg1, arg2) => new ClangIndex.ClientAstFile (arg2.Address);

				var refs = new List<string> ();
				cb.IndexEntityReference += (arg1, arg2) => refs.Add (string.Format ("({0}, {1})", arg2.Location.SourceLocation.ExpansionLocation.Line, arg2.Location.SourceLocation.ExpansionLocation.Column));

				bool startedTranslationUnit = false;
				cb.StartedTranslationUnit += (arg) => { startedTranslationUnit = true; return new ClangIndex.ContainerInfo (tu.Handle); };

				int includeCount = 0;
				cb.PreprocessIncludedFile += (arg1, arg2) => { includeCount++; return new ClangIndex.ClientFile (arg2.Handle); };

				cb.IndexDeclaration += (arg1, arg2) => {
					if (arg2.Location.FileLocation.File.FileName != filename)
						return;
					Assert.AreEqual (2, arg2.Location.SourceLocation.ExpansionLocation.Line, "Line: " + arg2.Location.SourceLocation);
					int col = arg2.Location.SourceLocation.ExpansionLocation.Column;
					var ent = arg2.EntityInfo;
					switch (col) {
					case 5:
						Assert.AreEqual ("bar", ent.Name, "EntityInfo.Name." + col);
						Assert.AreEqual ("c:@F@bar", ent.USR, "EntityInfo.USR." + col);
						goto case -1;
					case 30:
						Assert.AreEqual ("foo", ent.Name, "EntityInfo.Name." + col);
						Assert.AreEqual ("c:@F@foo", ent.USR, "EntityInfo.USR." + col);
						goto case -1;
					case 55:
						Assert.AreEqual ("main", ent.Name, "EntityInfo.Name." + col);
						Assert.AreEqual ("c:@F@main", ent.USR, "EntityInfo.USR." + col);
						goto case -1;
					case -1:
						Assert.AreEqual (0, arg2.Attributes.Count (), "Count." + col);
						Assert.IsTrue (arg2.IsContainer, "IsContainer." + col);
						Assert.IsTrue (arg2.IsDefinition, "IsDefinition." + col);
						Assert.IsFalse (arg2.IsImplicit, "IsImplicit." + col);
						Assert.IsFalse (arg2.IsRedeclaration, "IsRedeclaration." + col);
						Assert.AreEqual (IndexDeclInfoFlags.None, arg2.Flags, "Flags." + col);

						Assert.IsNotNull (ent, "EntityInfo." + col);
						Assert.AreEqual (0, ent.AttributeCount, "EntityInfo.AttributeCount." + col);
						Assert.AreEqual (IndexEntityCxxTemplateKind.NonTemplate, ent.CxxTemplateKind, "EntityInfo.CxxTemplateKind." + col);
						Assert.AreEqual (IndexEntityLanguage.C, ent.EntityLanguage, "EntityInfo.EntityLanguage." + col);
						Assert.AreEqual (IndexEntityKind.Function, ent.Kind, "EntityInfo.Kind." + col);

						Assert.IsNotNull (arg2.Cursor, "Cursor." + col);
						var dc = arg2.DeclarationAsContainer;
						Assert.IsNotNull (dc, "DeclarationAsContainer." + col);
						Assert.AreEqual (arg2.Cursor, dc.Cursor, "DeclarationAsContainer.Cursor." + col);

						// everything is at top level in this sample.
						var lc = arg2.LexicalContainer;
						Assert.IsNotNull (lc, "LexicalContainer." + col);
						Assert.AreEqual (tu.GetCursor (), lc.Cursor, "LexicalContainer.Cursor." + col);

						var sc = arg2.SemanticContainer;
						Assert.IsNotNull (sc, "SemanticContainer." + col);
						Assert.AreEqual (tu.GetCursor (), lc.Cursor, "SemanticContainer.Cursor." + col);

						Assert.IsNotNull (arg2.CxxClassDeclaration, "CxxClassDeclaration." + col);
						Assert.IsNotNull (arg2.ObjCCategoryDeclaration, "ObjCCategoryDeclaration." + col);
						Assert.IsNotNull (arg2.ObjCContainerDeclaration, "ObjCContainerDeclaration." + col);
						Assert.IsNotNull (arg2.ObjCInterfaceDeclaration, "ObjCInterfaceDeclaration." + col);
						Assert.IsNotNull (arg2.ObjCPropertyDeclaration, "ObjCPropertyDeclaration." + col);
						Assert.IsNotNull (arg2.ObjCProtocolReferenceListDeclaration, "ObjCProtocolReferenceListDeclaration." + col);

						break;
					default:
						Assert.Fail ("Unexpected index decl: " + arg2.Location.SourceLocation);
						break;
					}
				};

				a.IndexTranslationUnit (IntPtr.Zero, new ClangIndexerCallbacks[] {cb}, IndexOptionFlags.None, tu);

				Assert.IsTrue (startedTranslationUnit, "startedTranslationUnit");
				Assert.IsTrue (enteredMainFile, "enteredMainFile");
				Assert.IsTrue (abortRequested > 0, "abortRequested > 0");
				Assert.AreEqual (1, diagnosticCount, "diagnosticCount");
				Assert.IsTrue (refs.Contains ("(2, 72)"), "Entity Reference Indexed: (2, 72)");
				Assert.IsTrue (refs.Contains ("(2, 81)"), "Entity Reference Indexed: (2, 81)");
				Assert.IsTrue (includeCount > 0, "includeCount > 0");
			}, filename, code);
		}
	}
}

