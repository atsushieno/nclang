using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCursorUSR (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_constructUSR_ObjCClass (string class_name);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_constructUSR_ObjCCategory (string class_name, string category_name);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_constructUSR_ObjCProtocol (string protocol_name);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_constructUSR_ObjCIvar (string name, CXString classUSR);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_constructUSR_ObjCMethod (string name, uint isInstanceMethod, CXString classUSR);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_constructUSR_ObjCProperty (string property, CXString classUSR);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCursorSpelling (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRange 	clang_Cursor_getSpellingNameRange (CXCursor _, uint pieceIndex, uint options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCursorDisplayName (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCursor 	clang_getCursorReferenced (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCursor 	clang_getCursorDefinition (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint	clang_isCursorDefinition (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCursor 	clang_getCanonicalCursor (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern int 	clang_Cursor_getObjCSelectorIndex (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern int clang_Cursor_isDynamicCall (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXType 	clang_Cursor_getReceiverType (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint	clang_Cursor_getObjCPropertyAttributes (CXCursor C, uint reserved);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint	clang_Cursor_getObjCDeclQualifiers (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint	clang_Cursor_isObjCOptional (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint	clang_Cursor_isVariadic (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRange 	clang_Cursor_getCommentRange (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_Cursor_getRawCommentText (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_Cursor_getBriefCommentText (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXComment 	clang_Cursor_getParsedComment (CXCursor C);
	}
}

