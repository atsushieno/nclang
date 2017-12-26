using System;
using NClang;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PInvokeGenerator
{
	class Driver
	{
		public static void Main (string [] args)
		{
			new Driver ().Run (args);
		}

		public static string LibraryName;
		public static string Namespace;
		public static List<string> Args = new List<string> ();

		bool InsideUsingDeclaration;
		List<string> sources = new List<string> ();
		List<TypeDef> usings = new List<TypeDef> ();

		void Run (string [] args)
		{
			var idx = ClangService.CreateIndex ();
			var tus = new List<ClangTranslationUnit> ();
			TextWriter output = Console.Out;
			List<Regex> fileMatches = new List<Regex> ();
			bool onlyExplicit = false;

			Args.Add ("-x");
			Args.Add ("c++");
			Args.Add ("--std=c++1y");
			foreach (var arg in args) {
				if (arg == "--help" || arg == "-?") {
					Console.Error.WriteLine ($"[USAGE] {GetType ().Assembly.GetName ().CodeBase} [options] [inputs]");
					Console.Error.WriteLine (@"options:
	--out:[filename]	output source file name.
	--lib:[library]		library name specified on [DllImport].
	--ns:[namespace]	namespace name that wraps the entire code.
	--match:[regex]		when specified, process only matching files.
	--arg:[namespace]	compiler arguments to parse the sources.");
					return;
				} else if (arg.StartsWith ("--out:", StringComparison.Ordinal))
					output = File.CreateText (arg.Substring (6));
				else if (arg.StartsWith ("--lib:", StringComparison.Ordinal))
					LibraryName = arg.Substring (6);
				else if (arg.StartsWith ("--ns:", StringComparison.Ordinal))
					Namespace = arg.Substring (5);
				else if (arg.StartsWith ("--arg:", StringComparison.Ordinal))
					Args.Add (arg.Substring (6));
				else if (arg.StartsWith ("--match:", StringComparison.Ordinal))
					fileMatches.Add (new Regex (arg.Substring (8)));
				else if (arg == "--only-explicit")
					onlyExplicit = true;
				else
					sources.Add (arg);
			}
			foreach (var source in sources) {
				ClangTranslationUnit tu;
				var err = idx.ParseTranslationUnit (source, Args.ToArray (), null, TranslationUnitFlags.SkipFunctionBodies, out tu);
				if (err == ErrorCode.Success)
					tus.Add (tu);
			}

			var members = new List<Named> ();
			Struct current = null;
			string current_typedef_name = null;
			int anonymous_type_count = 0;

			Action<ClangCursor> removeDuplicates = c => {
				var dups = members.Where (m => m.Name == c.Spelling || m.Line == c.Location.FileLocation.Line && m.Column == c.Location.FileLocation.Column && m.SourceFile == c.Location.FileLocation.File.FileName).ToArray ();
				foreach (var d in dups)
					members.Remove (d);
			};

			foreach (var tu in tus) {
				for (int i = 0; i < tu.DiagnosticCount; i++)
					Console.Error.WriteLine ($"[diag] {tu.GetDiagnostic (i).Location}: {tu.GetDiagnostic (i).Spelling}");

				Func<ClangCursor, ClangCursor, IntPtr, ChildVisitResult> func = null;
				func = (cursor, parent, clientData) => {
					// skip ignored file.
					if (onlyExplicit && !sources.Contains (cursor.Location.FileLocation.File.FileName))
						return ChildVisitResult.Continue;
					if (fileMatches.Any () && !fileMatches.Any (fm => fm.IsMatch (cursor.Location.FileLocation.File.FileName)))
						return ChildVisitResult.Continue;

					// FIXME: this doesn't work.
					if (cursor.Kind == CursorKind.InclusionDirective) {
						Console.Error.WriteLine ("Include File " + cursor.IncludedFile);
						idx.ParseTranslationUnit (cursor.IncludedFile.FileName, null, null, TranslationUnitFlags.None).GetCursor ().VisitChildren (func, IntPtr.Zero);
					}
					if (cursor.Kind == CursorKind.TypedefDeclaration) {
						if (cursor.Location.FileLocation.File != null) {
							var alias = ToTypeName (cursor.CursorType);
							InsideUsingDeclaration = true;
							if (usings.All (u => u.Alias != alias)) {
								var actual = ToTypeName (cursor.TypeDefDeclUnderlyingType);
								var td = new TypeDef {
									Alias = alias,
									Actual = cursor.TypeDefDeclUnderlyingType.ArraySize < 0 ? actual : $"System.IntPtr/*{actual}[]*/",
									Location = cursor.Location,
								};
								usings.Add (td);
							}
							InsideUsingDeclaration = false;

							current_typedef_name = alias;
							foreach (var child in cursor.GetChildren ())
								func (child, cursor, clientData);
							current_typedef_name = null;

							return ChildVisitResult.Continue;
						}
					}
					if (cursor.Kind == CursorKind.EnumConstantDeclaration) {
						removeDuplicates (cursor);
						current.Fields.Add (new Variable () {
							Type = ToTypeName (cursor.CursorType),
							Name = cursor.Spelling,
							// FIXME: this is HACK.
							Value = (cursor.EnumConstantDeclValue != 0 ? (decimal)cursor.EnumConstantDeclValue : (decimal)cursor.EnumConstantDeclUnsignedValue).ToString (),
						});
						return ChildVisitResult.Continue;
					}
					if (cursor.Kind == CursorKind.FieldDeclaration) {
						removeDuplicates (cursor);
						var typeCursor = cursor.CursorType.TypeDeclaration;
						var type = !string.IsNullOrEmpty (typeCursor.DisplayName) ?
								  ToTypeName (cursor.CursorType) :
								  members.FirstOrDefault (m => m.Line == typeCursor.Location.FileLocation.Line && m.Column == typeCursor.Location.FileLocation.Column && m.SourceFile == typeCursor.Location.FileLocation.File.FileName)?.Name ??
								  ToTypeName (cursor.CursorType);
						current.Fields.Add (new Variable () {
							Type = type,
							TypeDetails = GetTypeDetails (cursor.CursorType),
							ArraySize = cursor.CursorType.ArraySize,
							SizeOf = cursor.CursorType.SizeOf,
							Name = cursor.Spelling
						});
						return ChildVisitResult.Continue;
					}
					if (cursor.Kind == CursorKind.StructDeclaration || cursor.Kind == CursorKind.UnionDeclaration || cursor.Kind == CursorKind.EnumDeclaration) {
						removeDuplicates (cursor);
						var parentType = current;
						current = new Struct () {
							Name = current_typedef_name != null && parentType == null ? current_typedef_name :
								string.IsNullOrEmpty (cursor.DisplayName) ? "anonymous_type_" + (anonymous_type_count++) : cursor.DisplayName,
							Location = cursor.Location,
							IsUnion = cursor.Kind == CursorKind.UnionDeclaration,
							IsEnum = cursor.Kind == CursorKind.EnumDeclaration,
						};
						members.Add (current);
						foreach (var child in cursor.GetChildren ())
							func (child, cursor, clientData);
						current = parentType;
						return ChildVisitResult.Continue;
					}
					if (cursor.Kind == CursorKind.FunctionDeclaration) {
						removeDuplicates (cursor);
						var fargs = Enumerable.Range (0, cursor.ArgumentCount)
								     .Select (i => cursor.GetArgument (i));

						// function with va_list isn't supported in P/Invoke.
						if (fargs.Any (a => a.CursorType.Spelling == "va_list")) {
							Console.Error.WriteLine ($"Cannot bind {cursor.DisplayName} because it contains va_list.");
							return ChildVisitResult.Continue;
						}
						
						members.Add (new Function () {
							Name = cursor.Spelling,
							Return = ToTypeName (cursor.ResultType),
							Location = cursor.Location,
							Args = fargs.Select (a => new Variable () {
								Name = a.Spelling,
								Type = ToTypeName (a.CursorType),
								TypeDetails = GetTypeDetails (a.CursorType)
								})
							            .ToArray ()
						});
						return ChildVisitResult.Continue;
					}
					return ChildVisitResult.Recurse;
				};
				tu.GetCursor ().VisitChildren (func, IntPtr.Zero);
			}

			output.WriteLine ("// This source file is generated by nclang PInvokeGenerator.");
			output.WriteLine ("using System;");
			output.WriteLine ("using System.Runtime.InteropServices;");
			foreach (var u in usings.Distinct (new KeyComparer ())) {
				// FIXME: hacky matching
				var mbr = members.FirstOrDefault (m => u.Actual.EndsWith (m.Name, StringComparison.Ordinal));
				if (mbr != null)
					mbr.Name = u.Alias;
				else {
					// FIXME: this does not seem to work
					var del = delegates.FirstOrDefault (d => u.Actual.EndsWith (d.TypeNameForDeclaration, StringComparison.Ordinal));
					if (del != null) {
						del.TypeNameForDeclaration = u.Alias;
						del.TypeNameForReference = "Delegates." + u.Alias;
						output.WriteLine ("using {0} = {1}; // {2} ({3},{4})", u.Actual.Substring (Namespace.Length + 1), WithNamespace (del.TypeNameForReference), u.SourceFileName, u.Line, u.Column);
					}
				}
			}
			// some code references function pointer types without "Delegates.", so workaround that with hacky "Delegates." addition.
			foreach (var del in delegates)
				output.WriteLine ("using {0} = {1};", del.TypeNameForDeclaration, WithNamespace (del.TypeNameForReference));
			output.WriteLine ();

			if (Namespace != null)
				output.WriteLine ("namespace {0} {{", Namespace);
			
			foreach (var o in members.OfType<Struct> ()) {
				o.Write (output);
				output.WriteLine ();
			}

			output.WriteLine ("partial class Natives");
			output.WriteLine ("{");
			output.WriteLine ("\tconst string LibraryName = \"{0}\";", LibraryName);

			foreach (var o in members.OfType<Function> ()) {
				o.Write (output);
				output.WriteLine ();
			}
			output.WriteLine ("}");
			output.WriteLine ();

			if (delegates.Any ()) {
				output.WriteLine ("class Delegates");
				output.WriteLine ("{");
				foreach (var s in delegates)
					output.WriteLine ("public " + s.DelegateDefinition + ";");
				output.WriteLine ("}");
			}

			output.WriteLine (@"
public struct Pointer<T>
{
	public IntPtr Handle;
	public static implicit operator IntPtr (Pointer<T> value) { return value.Handle; }
	public static implicit operator Pointer<T> (IntPtr value) { return new Pointer<T> (value); }

	public Pointer (IntPtr handle)
	{
		Handle = handle;
	}

	public override bool Equals (object obj)
	{
		return obj is Pointer<T> && this == (Pointer<T>) obj;
	}

	public override int GetHashCode ()
	{
		return (int) Handle;
	}

	public static bool operator == (Pointer<T> p1, Pointer<T> p2)
	{
		return p1.Handle == p2.Handle;
	}

	public static bool operator != (Pointer<T> p1, Pointer<T> p2)
	{
		return p1.Handle != p2.Handle;
	}
}
public struct ArrayOf<T> {}
public struct ConstArrayOf<T> {}
public class CTypeDetailsAttribute : Attribute
{
	public CTypeDetailsAttribute (string value)
	{
		Value = value;
	}

	public string Value { get; set; }
}
");

			if (Namespace != null)
				output.WriteLine ("}");

			output.Close ();
		}

		class KeyComparer : IEqualityComparer<TypeDef>
		{
			public bool Equals (TypeDef x, TypeDef y)
			{
				return x.Alias == y.Alias;
			}

			public int GetHashCode (TypeDef obj)
			{
				return obj.Alias.GetHashCode ();
			}
		}

		class TypeDef : Named
		{
			public string Alias;
			public string Actual;

			public override void Write (TextWriter w)
			{
				throw new NotSupportedException ();
			}
		}

		abstract class Named
		{
			public string SourceFile { get; private set; }
			public int Line { get; private set; }
			public int Column { get; private set; }
			public string Name;

			public string SourceFileName {
				get { return SourceFile == null ? null : Path.GetFileName (SourceFile); }
			}

			public ClangSourceLocation Location {
				set {
					SourceFile = value.FileLocation.File.FileName;
					Line = value.FileLocation.Line;
					Column = value.FileLocation.Column;
				}
			}

			public abstract void Write (TextWriter w);
		}

		class Variable
		{
			public string Name;
			public string Type;
			public string TypeDetails;
			public int ArraySize; // array element count
			public int SizeOf; // sizeof entire struct
			public string Value;
		}

		class Struct : Named
		{
			public bool IsUnion;
			public bool IsEnum;
			public List<Variable> Fields = new List<Variable> ();

			public override void Write (TextWriter w)
			{
				if (IsEnum) {
					w.WriteLine("enum {0} // {1} ({2}, {3})", string.IsNullOrEmpty (Name) ? "_" + new string (Path.GetFileNameWithoutExtension (SourceFileName).TakeWhile (c => char.IsLetterOrDigit (c)).ToArray ()) + "_" + Line + "_" + Column : Name, SourceFileName, Line, Column);
					w.WriteLine("{");
					foreach (var m in Fields)
						w.WriteLine("\t{0} {1}{2},", m.Name, m.Value != null ? " = " : null, m.Value);
					w.WriteLine("}");
				} else {
					if (IsUnion)
						w.WriteLine ("[StructLayout (LayoutKind.Explicit)]");
					else
						w.WriteLine ("[StructLayout (LayoutKind.Sequential)]");
					w.WriteLine ("struct {0} // {1} ({2}, {3})", Name, SourceFileName, Line, Column);
					w.WriteLine ("{");
					foreach (var m in Fields) {
						if (m.ArraySize > 0)
							w.WriteLine ("\t[MarshalAs (UnmanagedType.ByValArray, SizeConst=" + m.ArraySize + ")]");
						if (IsUnion)
							w.WriteLine ("\t[FieldOffset (0)]");
						w.WriteLine ("\t{2}public {0} @{1};", m.Type, string.IsNullOrEmpty (m.Name) ? "_" + Fields.IndexOf (m) : m.Name, m.TypeDetails);
					}

					w.WriteLine ("}");
				}
			}
		}

		class Function : Named
		{
			public string Return;
			public Variable [] Args;

			public override void Write (TextWriter w)
			{
				w.WriteLine ("\t// function {0} - {1} ({2}, {3})", Name, SourceFileName, Line, Column);
				w.WriteLine ("\t[DllImport (LibraryName)]");
				w.WriteLine ("\tinternal static extern {0} {1} ({2});", Return, Name, string.Join (", ", Args.Select (a => a.TypeDetails + a.Type + " " + (string.IsNullOrEmpty (a.Name) ? "_" + Array.IndexOf (Args, a) : '@' + a.Name))));
			}
		}

		string ToNonKeywordTypeName (string s)
		{
			switch (s) {
			case "void":
				return "void";
			case "byte":
				return "System.Byte";
			case "sbyte":
				return "System.SByte";
			case "short":
				return "System.Int16";
			case "ushort":
				return "System.UInt16";
			case "int":
				return "System.Int32";
			case "uint":
				return "System.UInt32";
			case "long":
				return "System.Int32";
			case "long long":
				return "System.Int64"; // FIXME: this conversion is wrong
			case "unsigned long long":
				return "System.UInt64"; // FIXME: this conversion is wrong
			case "ulong":
				return "System.UInt32";
			case "float":
				return "System.Single";
			case "double":
				return "System.Double";
			}
			return WithNamespace (s);
		}

		string WithNamespace (string s)
		{
			return s.StartsWith ("System.", StringComparison.Ordinal) ? s : (Namespace != null ? Namespace + '.' : null) + s;
		}

		string ToTypeName (ClangType type, bool strip = true)
		{
			var ret = ToTypeName_ (type, strip);
			if (!InsideUsingDeclaration)
				return ret;
			var alias = usings.FirstOrDefault (u => u.Alias == ret)?.Actual;
			if (alias != null)
				return alias;
			return ToNonKeywordTypeName (ret);
		}

		string ToTypeName_ (ClangType type, bool strip = true)
		{
			if (type.IsPODType) {
				switch (type.Kind) {
				case TypeKind.Int:
					return "int"; // FIXME: this should be actually platform dependent
				case TypeKind.UInt:
					return "uint"; // FIXME: this should be actually platform dependent
				case TypeKind.Float:
					return "float";
				case TypeKind.Double:
				case TypeKind.LongDouble: // FIXME: this should be actually platform dependent
					return "double";
				case TypeKind.UChar:
				case TypeKind.CharU:
				case TypeKind.CharS:
					return "byte"; // The sign most unlikely matters.
				case TypeKind.SChar:
					return "sbyte";
				case TypeKind.Short:
					return "short";
				case TypeKind.UShort:
					return "ushort";
				case TypeKind.Long:
					return "long"; // FIXME: this should be actually platform dependent
				case TypeKind.ULong:
					return "ulong"; // FIXME: this should be actually platform dependent
				case TypeKind.LongLong:
					return "long"; // FIXME: this should be actually platform dependent
				case TypeKind.ULongLong:
					return "ulong"; // FIXME: this should be actually platform dependent
				}
				// for aliased types to POD they still have IsPODType = true, so we need to ignore them.
			}
			if (type.Kind == TypeKind.ConstantArray)
				return ToTypeName (type.ElementType) + "[]";
			if (type.Kind == TypeKind.IncompleteArray)
				return "ArrayOf<" + ToTypeName (type.ElementType) + ">";
			if (type.Kind == TypeKind.Pointer) {
				if (type.PointeeType != null && type.PointeeType.ArgumentTypeCount >= 0) {
					// function pointer
					return CreateFunctionPointerDelegateName (type);
				} else {
					return "System.IntPtr";
				}
			}
			if (strip && type.IsConstQualifiedType)
				return ToTypeName (type, false).Substring (6); // "const "
			else
				return type.Spelling.Replace ("struct ", "").Replace ("union ", "").Replace ("enum ", "");
		}

		class FunctionPointerDelegate : Named
		{
			public ClangType Type;
			public string ReturnType;
			public string Arguments;
			public string DelegateDefinition {
				get {
					return $"delegate {ReturnType} {TypeNameForDeclaration} ({Arguments})";
				}
			}
			public string TypeNameForDeclaration;
			public string TypeNameForReference;

			public override void Write (TextWriter w)
			{
				throw new NotSupportedException ();
			}
		}

		List<FunctionPointerDelegate> delegates = new List<FunctionPointerDelegate> ();

		string CreateFunctionPointerDelegateName (ClangType type)
		{
			var x = delegates.FirstOrDefault (e => e.Type == type);
			if (x != null)
				return x.TypeNameForReference;
			
			var pt = type.PointeeType;
			string ret = ToTypeName (pt.ResultType);
			bool hasArgs = pt.ArgumentTypeCount > 0;
			string args = "";
			string f = "delegate" + delegates.Count.ToString (System.Globalization.CultureInfo.InvariantCulture);
			for (int i = 0; i < pt.ArgumentTypeCount; i++) {
				var tn = ToTypeName (pt.GetArgumentType (i));
				if (i > 0)
					args += ", ";
				args += $"{tn} p{i}";
				if (tn == "va_list") {
					Console.Error.WriteLine ($"Cannot bind {type.Spelling} because its arguments contain va_list.");
					// give up and return void*
					return "System.IntPtr";
				}
			}
			// FIXME: we need to acquire location (impossible with current code structure)
			delegates.Add (new FunctionPointerDelegate { Type = type, ReturnType = ret, Arguments = args, TypeNameForDeclaration = f, TypeNameForReference = "Delegates." + f, });
			return f;
		}

		string GetTypeDetails (ClangType type)
		{
			if (type.Kind == TypeKind.Pointer)
				return "[CTypeDetails (\"Pointer<" + ToTypeName (type.PointeeType) + ">\")]";
			if (type.Kind == TypeKind.ConstantArray)
				return "[CTypeDetails (\"ConstArrayOf<" + ToTypeName (type.ElementType) + ">\")] ";
			return null;
		}
	}
}
