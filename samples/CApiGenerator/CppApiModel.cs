using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NClang;

namespace CApiGenerator
{

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

	abstract class NamedConstruct : Locatable
	{
		public string Namespace = String.Empty;
		public string Name;
		public CursorKind Kind;

		string c_name;
		public virtual string CName ()
		{
			return (c_name = c_name ?? Namespace.ToLowerInvariant () + "_" + Name.ToLowerInvariant ());
		}
	}

	class Variable : NamedConstruct
	{
		public CXXAccessSpecifier Access;
		public string Type;
		public int ArraySize; // array element count
		public int SizeOf; // sizeof entire struct
		public bool IsStatic;
		public bool IsConst;

		public override void Write (TextWriter w)
		{
			w.WriteLine ($"\t// [{Access}][{Kind}] {SourceFileName} ({Line}, {Column})");
			w.WriteLine ("\tpublic {0} {1};", Type, Name);
		}

		public override string CName ()
		{
			return Name.ToLowerInvariant ();
		}

		public string CTypeName ()
		{
			// FIXME: needs namespace
			return Type.ToLowerInvariant ();
		}

		public string ToTypeAndName ()
		{
			var p = Type.IndexOf ('[') > 0 ? Type.Substring (0, Type.IndexOf ('[')) : Type;
			var s = Type.IndexOf ('[') > 0 ? Type.Substring (Type.IndexOf ('[')) : string.Empty;
			return p + " " + Name + s;
		}
	}

	class TypeParameter
	{
		public string Name;
	}

	class ClassDeclaration : NamedConstruct
	{
		public string BaseType;
		public List<TypeParameter> TypeParameters;
		public List<Variable> Fields = new List<Variable> ();
		public List<Function> Constructors = new List<Function> ();
		public List<Function> Functions = new List<Function> ();

		public override string CName ()
		{
			return base.CName ();
		}

		public string CTypeName ()
		{
			return CName () + "_t";
		}

		const string sep = ", ";
		internal string nssep => string.IsNullOrEmpty (Namespace) ? string.Empty : ".";

		public override void Write (TextWriter w)
		{
			w.WriteLine ($"class {CTypeName ()} : {Namespace}{nssep}{Name} // {SourceFileName} ({Line}, {Column})");
			w.WriteLine ("{");
			foreach (var m in Fields)
				w.WriteLine ($"\t// <{m.Access}>[{m.Kind}] {m.Type} {m.Name}");
			foreach (var m in Constructors)
				w.WriteLine ($"\t// <{m.Access}>[{m.Kind}] #ctor({string.Join(sep, m.Parameters.Select (p => p.Type + " " + p.Name))})");
			foreach (var m in Functions)
				w.WriteLine ($"\t// <{m.Access}>[{m.Kind}] {m.Name}({string.Join (sep, m.Parameters.Select (p => p.Type + " " + p.Name))})");

			w.WriteLine ("};");
		}
	}

	class Function : NamedConstruct
	{
		public CXXAccessSpecifier Access;
		public List<TypeParameter> TypeParameters;
		public string Return;
		public Variable [] Parameters;
		public bool IsVirtual;
		public bool IsPureVirtual;

		public string CTypeReturn ()
		{
			// FIXME: needs namespace
			return Return.ToLowerInvariant ();
		}

		public override void Write (TextWriter w)
		{
			w.WriteLine ($"\t// [{Access}][{Kind}] {SourceFileName} ({Line}, {Column})");
			w.WriteLine ($"\t{Return} {Name} ({string.Join (", ", Parameters.Select (a => a.ToTypeAndName ()))}) {{  }}");
		}
	}

	/*
	class FunctionPointerDelegate
	{
		public ClangType Type;
		public string DelegateDefinition;
		public string TypeName;
	}

	List<FunctionPointerDelegate> delegates = new List<FunctionPointerDelegate> ();
	*/
}
