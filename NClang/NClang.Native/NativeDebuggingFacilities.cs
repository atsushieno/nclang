using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// I doubt we should be binding them...
	static partial class LibClang
	{
		
		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getCursorKindSpelling (CursorKind Kind);
		// const char **startBuf, const char **endBuf
		[DllImport (LibraryName)]
		 internal static extern void 	clang_getDefinitionSpellingAndExtent (CXCursor _,
			out IntPtr startBuf, out IntPtr endBuf,
			[MarshalAs (UnmanagedType.SysUInt)] out uint startLine, [MarshalAs (UnmanagedType.SysUInt)] out uint startColumn,
			[MarshalAs (UnmanagedType.SysUInt)] out uint endLine, [MarshalAs (UnmanagedType.SysUInt)] out uint endColumn);

		[DllImport (LibraryName)]
		 internal static extern void 	clang_enableStackTraces ();

		[DllImport (LibraryName)]
		 internal static extern void 	clang_executeOnThread (Action fn, IntPtr user_data, [MarshalAs (UnmanagedType.SysUInt)] uint stack_size);
	}
}
		