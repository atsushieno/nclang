#if MANUAL

using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;
using CXStringSet = NClang.ClangStringSet;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXString 	clang_Cursor_getMangling (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXStringSet clang_Cursor_getCxxManglings (CXCursor _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXStringSet clang_Cursor_getObjCManglings (CXCursor _);
	}
}


#endif
