using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// I doubt we should be binding them...
	static partial class LibClang
	{
		
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCursorKindSpelling (CursorKind Kind);
		// const char **startBuf, const char **endBuf
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_getDefinitionSpellingAndExtent (CXCursor _,
			out IntPtr startBuf, out IntPtr endBuf,
			out uint startLine, out uint startColumn,
			out uint endLine, out uint endColumn);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_enableStackTraces ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_executeOnThread (Action fn, IntPtr user_data, uint stack_size);
	}
}
		