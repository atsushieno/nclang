using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	enum  	CXObjCPropertyAttrKind
	{
		NoAttr = 0x00,
		ReadOnly = 0x01,
		Getter = 0x02,
		Assign = 0x04,
		ReadWrite = 0x08,
		Retain = 0x10,
		Copy = 0x20,
		NonAtomic = 0x40,
		Setter = 0x80,
		Atomic = 0x100,
		Weak = 0x200,
		Strong = 0x400,
		UnsafeU7nretained = 0x800
	}

	enum  	CXObjCDeclQualifierKind
	{
		None = 0x0,
		In = 0x1,
		Inout = 0x2,
		Out = 0x4,
		Bycopy = 0x8,
		Byref = 0x10,
		Oneway = 0x20
	}

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getCursorUSR (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_constructUSR_ObjCClass (string class_name);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_constructUSR_ObjCCategory (string class_name, string category_name);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_constructUSR_ObjCProtocol (string protocol_name);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_constructUSR_ObjCIvar (string name, CXString classUSR);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_constructUSR_ObjCMethod (string name, [MarshalAs (UnmanagedType.SysUInt)] uint isInstanceMethod, CXString classUSR);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_constructUSR_ObjCProperty (string property, CXString classUSR);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getCursorSpelling (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange 	clang_Cursor_getSpellingNameRange (CXCursor _, [MarshalAs (UnmanagedType.SysUInt)] uint pieceIndex, [MarshalAs (UnmanagedType.SysUInt)] uint options);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getCursorDisplayName (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getCursorReferenced (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getCursorDefinition (CXCursor _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint	clang_isCursorDefinition (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getCanonicalCursor (CXCursor _);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int 	clang_Cursor_getObjCSelectorIndex (CXCursor _);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int clang_Cursor_isDynamicCall (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXType 	clang_Cursor_getReceiverType (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint	clang_Cursor_getObjCPropertyAttributes (CXCursor C, [MarshalAs (UnmanagedType.SysUInt)] uint reserved);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint	clang_Cursor_getObjCDeclQualifiers (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint	clang_Cursor_isObjCOptional (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint	clang_Cursor_isVariadic (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange 	clang_Cursor_getCommentRange (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_Cursor_getRawCommentText (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_Cursor_getBriefCommentText (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXComment 	clang_Cursor_getParsedComment (CXCursor C);
	}
}

