using System;
using System.Runtime.InteropServices;

using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

namespace NClang.Natives
{
	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getCursor (CXTranslationUnit _, CXSourceLocation __);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation 	clang_getCursorLocation (CXCursor _);

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange 	clang_getCursorExtent (CXCursor _);
	}
}

