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
		public static void Main (string[] args)
		{
			new Driver ().Run (args);
		}

		public static string LibraryName;
		public static string Namespace;
		public static List<string> Args = new List<string> ();

		bool InsideUsingDeclaration;
		List<string> sources = new List<string> ();
		List<TypeDef> usings = new List<TypeDef> ();
		public List<Regex> fileMatches = new List<Regex> ();

		void Run (string[] args)
		{
			var idx = ClangService.CreateIndex ();
			var tus = new List<ClangTranslationUnit> ();
			TextWriter output = Console.Out;
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
				else
					sources.Add (arg);
			}
			foreach (var source in sources)
				tus.Add (idx.ParseTranslationUnit (source, Args.ToArray (), null, TranslationUnitFlags.None));

			var members = new List<Locatable> ();
			Struct current = null;

			foreach (var tu in tus) {
				for (int i = 0; i < tu.DiagnosticCount; i++)
					Console.Error.WriteLine ("[diag] " + tu.GetDiagnostic (i).Spelling);
				
				Func<ClangCursor,ClangCursor,IntPtr,ChildVisitResult> func = null;
				func = (cursor, parent, clientData) => {
					// skip ignored file.
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
								usings.Add (new TypeDef () { Alias = alias, Actual = actual });
							}
							InsideUsingDeclaration = false;
						}
					}
					if (cursor.Kind == CursorKind.EnumConstantDeclaration)
						current.Fields.Add (new Variable ()
						{
							Type = ToTypeName (cursor.CursorType),
							Name = cursor.Spelling,
							// FIXME: this is HACK.
							Value = (cursor.EnumConstantDeclValue != 0 ? (decimal) cursor.EnumConstantDeclValue : (decimal) cursor.EnumConstantDeclUnsignedValue).ToString(),
						});
					if (cursor.Kind == CursorKind.FieldDeclaration)
						current.Fields.Add (new Variable () {
							Type = ToTypeName (cursor.CursorType),
							TypeDetails = GetTypeDetails (cursor.CursorType),
							ArraySize = cursor.CursorType.ArraySize,
							SizeOf = cursor.CursorType.SizeOf,
							Name = cursor.Spelling
						});
					if (cursor.Kind == CursorKind.StructDeclaration || cursor.Kind == CursorKind.UnionDeclaration || cursor.Kind == CursorKind.EnumDeclaration) {
						current = new Struct () {
							Name = cursor.DisplayName,
							SourceFile = cursor.Location.FileLocation.File.FileName,
							Line = cursor.Location.FileLocation.Line,
							Column = cursor.Location.FileLocation.Column,
							IsUnion = cursor.Kind == CursorKind.UnionDeclaration,
							IsEnum = cursor.Kind == CursorKind.EnumDeclaration,
						};
						if (members.All (m => m.Line != current.Line || m.Column != current.Column)) {
							var dup = members.OfType<Struct> ().Where (m => m.Name == current.Name).ToArray ();
							foreach (var d in dup)
								members.Remove (d);
							members.Add (current);
						}
					}
					if (cursor.Kind == CursorKind.FunctionDeclaration) {
						members.Add (new Function () {
							Name = cursor.Spelling,
							Return = ToTypeName (cursor.ResultType),
							SourceFile = cursor.Location.FileLocation.File.FileName,
							Line = cursor.Location.FileLocation.Line,
							Column = cursor.Location.FileLocation.Column,
							Args = Enumerable.Range (0, cursor.ArgumentCount)
							                 .Select (i => cursor.GetArgument (i))
							                 .Select (a => new Variable () {
										Name = a.Spelling,
										Type = ToTypeName (a.CursorType),
										TypeDetails = GetTypeDetails (a.CursorType) })
							                 .ToArray ()
							});
					}
					return ChildVisitResult.Recurse;
				};
				tu.GetCursor ().VisitChildren (func, IntPtr.Zero);
			}

			output.WriteLine ("// This source file is generated by nclang PInvokeGenerator.");
			output.WriteLine ("using System;");
			output.WriteLine ("using System.Runtime.InteropServices;");
			foreach (var u in usings.Distinct (new KeyComparer ()))
				if (u.Alias != u.Actual)
					output.WriteLine ("using {0} = {1};", u.Alias, u.Actual);
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

		struct TypeDef
		{
			public string Alias;
			public string Actual;
		}

		abstract class Locatable
		{
			public string SourceFile;
			public int Line;
			public int Column;

			public string SourceFileName {
				get { return SourceFile == null ? null : Path.GetFileName (SourceFile); }
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

		class Struct : Locatable
		{
			public bool IsUnion;
			public bool IsEnum;
			public string Name;
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
					w.WriteLine ("[StructLayout (LayoutKind.Sequential)]");
					w.WriteLine ("struct {0} // {1} ({2}, {3})", Name, SourceFileName, Line, Column);
					w.WriteLine ("{");
					foreach (var m in Fields) {
						if (m.ArraySize > 0)
							w.WriteLine ("\t[MarshalAs (UnmanagedType.ByValArray, SizeConst=" + m.ArraySize + ")]");
						w.WriteLine ("\t{2}public {0} {1};", m.Type, string.IsNullOrEmpty (m.Name) ? "_" + Fields.IndexOf (m) : m.Name, m.TypeDetails);
					}

					w.WriteLine ("}");
				}
			}
		}

		class Function : Locatable
		{
			public string Name;
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
			var alias = usings.FirstOrDefault (u => u.Alias == ret).Actual;
			if (alias != null)
				return alias;
			return ToNonKeywordTypeName (ret);
		}

		string ToTypeName_ (ClangType type, bool strip = true)
		{
			if (type.IsPODType) {
				switch (type.Spelling) {
				case "unsigned char":
					return "byte";
				case "char":
					return "byte"; // we most likely don't need sbyte
				case "signed char":
					return "sbyte"; // probably explicit signed specification means something
				case "short":
					return "short";
				case "unsigned short":
					return "ushort";
				case "long":
					return "long"; // FIXME: this should be actually platform dependent
				case "long long":
					return "/*FIXME: this was long long*/long"; // FIXME: this should be actually platform dependent
				case "unsigned long":
					return "ulong"; // FIXME: this should be actually platform dependent
				case "unsigned long long":
					return "/*FIXME: this was unsigned long long*/ulong"; // FIXME: this should be actually platform dependent
				case "int":
					return "int"; // FIXME: this should be actually platform dependent
				case "unsigned int":
					return "uint"; // FIXME: this should be actually platform dependent
				case "float":
					return "float";
				case "double":
					return "double";
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

		class FunctionPointerDelegate
		{
			public ClangType Type;
			public string DelegateDefinition;
			public string TypeName;
		}

		List<FunctionPointerDelegate> delegates = new List<FunctionPointerDelegate> ();

		string CreateFunctionPointerDelegateName (ClangType type)
		{
			var x = delegates.FirstOrDefault (e => e.Type == type);
			if (x != null)
				return x.TypeName;
			
			var pt = type.PointeeType;
			string ret = ToTypeName (pt.ResultType);
			var d = $"delegate {ret} delegate{delegates.Count} (";
			bool hasArgs = pt.ArgumentTypeCount > 0;
			string f = "Delegates.delegate" + delegates.Count;
			for (int i = 0; i < pt.ArgumentTypeCount; i++) {
				var tn = ToTypeName (pt.GetArgumentType (i));
				if (i > 0)
					d += ", ";
				d += $"{tn} p{i}";
			}
			d += ")";
			delegates.Add (new FunctionPointerDelegate { Type = type, DelegateDefinition = d, TypeName = f });
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
