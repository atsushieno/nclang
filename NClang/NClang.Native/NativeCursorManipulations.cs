using System;
using System.Runtime.InteropServices;

using CXCursorSet = System.IntPtr; // CXCursorSetImpl*

using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXFile = System.IntPtr;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXPlatformAvailability
	{
		public readonly CXString Platform;
		public readonly CXVersion Introduced;
		public readonly CXVersion Deprecated;
		public readonly CXVersion Obsoleted;
		public readonly int Unavailable;
		public readonly CXString Message;
	}

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getNullCursor ();

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getTranslationUnitCursor (CXTranslationUnit _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_equalCursors (CXCursor _, CXCursor __);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int clang_Cursor_isNull (CXCursor cursor);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_hashCursor (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CursorKind 	clang_getCursorKind (CXCursor _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isDeclaration (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isReference (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isExpression (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isStatement (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isAttribute (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isInvalid (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isTranslationUnit (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isPreprocessing (CursorKind _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_isUnexposed (CursorKind _);

		[DllImport (LibraryName)]
		 internal static extern LinkageKind 	clang_getCursorLinkage (CXCursor cursor);

		[DllImport (LibraryName)]
		 internal static extern AvailabilityKind 	clang_getCursorAvailability (CXCursor cursor);

		[return:MarshalAs (UnmanagedType.SysInt)]
		[DllImport (LibraryName)]
		 internal static extern int clang_getCursorPlatformAvailability (CXCursor cursor, out int always_deprecated,
		                                                       out CXString deprecated_message, out int always_unavailable, out CXString unavailable_message,
			ref IntPtr availability, int availability_size); // CXPlatformAvailability*

		[DllImport (LibraryName)]
		internal static extern void clang_disposeCXPlatformAvailability (IntPtr availability); // - or - ref CXPlatformAvailability

		[DllImport (LibraryName)]
		 internal static extern LanguageKind 	clang_getCursorLanguage (CXCursor cursor);

		[DllImport (LibraryName)]
		 internal static extern CXTranslationUnit 	clang_Cursor_getTranslationUnit (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXCursorSet 	clang_createCXCursorSet ();

		[DllImport (LibraryName)]
		 internal static extern void clang_disposeCXCursorSet (CXCursorSet cset);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_CXCursorSet_contains (CXCursorSet cset, CXCursor cursor);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_CXCursorSet_insert (CXCursorSet cset, CXCursor cursor);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getCursorSemanticParent (CXCursor cursor);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getCursorLexicalParent (CXCursor cursor);

		[DllImport (LibraryName)]
		internal static extern void clang_getOverriddenCursors (CXCursor cursor, out IntPtr overridden, [MarshalAs (UnmanagedType.SysUInt)] ref uint num_overridden);

		[DllImport (LibraryName)]
		 internal static extern void clang_disposeOverriddenCursors (ref CXCursor overridden);

		[DllImport (LibraryName)]
		 internal static extern CXFile 	clang_getIncludedFile (CXCursor cursor);
	}
}
