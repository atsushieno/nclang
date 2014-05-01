using System;
using System.Runtime.InteropServices;

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXType
	{
		public readonly TypeKind Kind;
		// void* [2]
		public readonly IntPtr Data1;
		public readonly IntPtr Data2;
	}

	enum  	CXTypeLayoutError
	{
		Invalid = -1,
		Incomplete = -2,
		Dependent = -3,
		NotConstantSize = -4,
		InvalidFieldName = -5
	}

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getCursorType (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getTypeSpelling (CXType CT);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getTypedefDeclUnderlyingType (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getEnumDeclIntegerType (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern SystemLongLong clang_getEnumConstantDeclValue (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern SystemULongLong clang_getEnumConstantDeclUnsignedValue (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int clang_getFieldDeclBitWidth (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int clang_Cursor_getNumArguments (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_Cursor_getArgument (CXCursor C, [MarshalAs (UnmanagedType.SysUInt)] uint i);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_equalTypes (CXType A, CXType B);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getCanonicalType (CXType T);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isConstQualifiedType (CXType T);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isVolatileQualifiedType (CXType T);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isRestrictQualifiedType (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType clang_getPointeeType (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getTypeDeclaration (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getDeclObjCTypeEncoding (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getTypeKindSpelling (TypeKind K);

		[DllImport (LibraryName)]
		 internal static extern CallingConvention 	clang_getFunctionTypeCallingConv (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getResultType (CXType T);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int clang_getNumArgTypes (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getArgType (CXType T, [MarshalAs (UnmanagedType.SysUInt)] uint i);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isFunctionTypeVariadic (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getCursorResultType (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isPODType (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType clang_getElementType (CXType T);

		[DllImport (LibraryName)]
		 internal static extern SystemLongLong clang_getNumElements (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_getArrayElementType (CXType T);

		[DllImport (LibraryName)]
		 internal static extern SystemLongLong clang_getArraySize (CXType T);

		[DllImport (LibraryName)]
		 internal static extern SystemLongLong clang_Type_getAlignOf (CXType T);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_Type_getClassType (CXType T);

		[DllImport (LibraryName)]
		 internal static extern SystemLongLong clang_Type_getSizeOf (CXType T);

		[DllImport (LibraryName)]
		 internal static extern SystemLongLong clang_Type_getOffsetOf (CXType T, string S);

		/* not in libclang 3.5
		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.SysInt)]
		internal static extern int clang_Type_getNumTemplateArguments (CXType T);
		*/

		[DllImport (LibraryName)]
		internal static extern CXType clang_Type_getTemplateArgumentAsType (CXType T, [MarshalAs (UnmanagedType.SysUInt) uint i);
		
		[DllImport (LibraryName)]
		 internal static extern RefQualifierKind 	clang_Type_getCXXRefQualifier (CXType T);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_Cursor_isBitField (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isVirtualBase (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXXAccessSpecifier 	clang_getCXXAccessSpecifier (CXCursor _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_getNumOverloadedDecls (CXCursor cursor);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getOverloadedDecl (CXCursor cursor, [MarshalAs (UnmanagedType.SysUInt)] uint index);
	}
}

