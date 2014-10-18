using System;
using NClang.Natives;
using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

namespace NClang
{
	public class ClangType
	{
		// Static members

		public string GetTypeKindSpelling (TypeKind kind)
		{
			return LibClang.clang_getTypeKindSpelling (kind).Unwrap ();
		}

		// Instance members

		readonly CXType source;

		internal ClangType (CXType source)
		{
			this.source = source;
		}

		public TypeKind Kind {
			get { return source.Kind; }
		}

		// TypeInformationForCXCursor

		public string Spelling {
			get { return LibClang.clang_getTypeSpelling (source).Unwrap (); }
		}

		public bool Equals (ClangType other)
		{
			return other != null && LibClang.clang_equalTypes (source, other.source) != 0;
		}

		public static bool operator == (ClangType o1, ClangType o2)
		{
			return (object) o1 == null ? (object) o2 == null : o1.Equals (o2);
		}

		public static bool operator != (ClangType o1, ClangType o2)
		{
			return (object) o1 == null ? (object) o2 != null : !o1.Equals (o2);
		}

		public override bool Equals (object obj)
		{
			return obj is ClangType && Equals ((ClangType) obj);
		}

		public override int GetHashCode ()
		{
			return source.GetHashCode ();
		}

		public ClangType CanonicalType {
			get { return LibClang.clang_getCanonicalType (source).ToManaged (); }
		}

		public bool IsConstQualifiedType {
			get { return LibClang.clang_isConstQualifiedType (source) != 0; }
		}

		public bool IsVolatileQualifiedType {
			get { return LibClang.clang_isVolatileQualifiedType (source) != 0; }
		}

		public bool IsRestrictQualifiedType {
			get { return LibClang.clang_isRestrictQualifiedType (source) != 0; }
		}

		public ClangType PointeeType {
			get { return LibClang.clang_getPointeeType (source).ToManaged (); }
		}

		public ClangCursor TypeDeclaration {
			get { return new ClangCursor (LibClang.clang_getTypeDeclaration (source)); }
		}

		public CallingConvention FunctionTypeCallingConvention {
			get { return LibClang.clang_getFunctionTypeCallingConv (source); }
		}

		public ClangType ResultType {
			get { return LibClang.clang_getResultType (source).ToManaged (); }
		}

		public int ArgumentTypeCount {
			get { return (int) LibClang.clang_getNumArgTypes (source); }
		}

		public ClangType GetArgumentType (int index)
		{
			return LibClang.clang_getArgType (source, (uint) index).ToManaged ();
		}

		public bool IsFunctionTypeVariadic {
			get { return LibClang.clang_isFunctionTypeVariadic (source) != 0; }
		}

		public bool IsPODType {
			get { return LibClang.clang_isPODType (source) != 0; }
		}

		public ClangType ElementType {
			get { return LibClang.clang_getElementType (source).ToManaged (); }
		}

		public int ElementCount {
			get { return (int) ElementCountAsDecimal; }
		}

		public SystemLongLong ElementCountAsDecimal {
			get { return LibClang.clang_getNumElements (source); }
		}

		public ClangType ArrayElementType {
			get { return LibClang.clang_getArrayElementType (source).ToManaged (); }
		}

		public int ArraySize {
			get { return (int) ArraySizeAsDecimal; }
		}

		public SystemLongLong ArraySizeAsDecimal {
			get { return LibClang.clang_getArraySize (source); }
		}

		public int AlignOf {
			get { return (int) AlignOfAsDecimal; }
		}

		public SystemLongLong AlignOfAsDecimal {
			get { return LibClang.clang_Type_getAlignOf (source); }
		}

		public ClangType ClassType {
			get { return LibClang.clang_Type_getClassType (source).ToManaged (); }
		}

		public int SizeOf {
			get { return (int) SizeOfAsDecimal; }
		}

		public SystemLongLong SizeOfAsDecimal {
			get { return LibClang.clang_Type_getSizeOf (source); }
		}

		public int GetOffsetOf (string fieldName)
		{
			return (int) GetOffsetOfAsDecimal (fieldName);
		}

		public decimal GetOffsetOfAsDecimal (string fieldName)
		{
			return (decimal) LibClang.clang_Type_getOffsetOf (source, fieldName);
		}

		/* not in libclang 3.5
		public int TemplateArgumentCount {
			get { return LibClang.clang_Type_getNumTemplateArguments (source); }
		}
		*/

		public ClangType GetTemplateArgumentAsType (int index)
		{
			return LibClang.clang_Type_getTemplateArgumentAsType (source, (uint) index).ToManaged ();
		}

		public RefQualifierKind RefQualifier {
			get { return LibClang.clang_Type_getCXXRefQualifier (source); }
		}
	}
}

