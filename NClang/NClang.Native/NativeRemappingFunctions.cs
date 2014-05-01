using System;
using System.Runtime.InteropServices;

using CXRemapping = System.IntPtr; // void*

using CXFile = System.IntPtr;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXRemapping 	clang_getRemappings (string path);

		[DllImport (LibraryName)]
		 internal static extern CXRemapping 	clang_getRemappingsFromFileList ([MarshalAs (UnmanagedType.LPArray)] string[] filePaths, [MarshalAs (UnmanagedType.SysUInt)] uint numFiles);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_remap_getNumFiles (CXRemapping _);

		[DllImport (LibraryName)]
		 internal static extern void 	clang_remap_getFilenames (CXRemapping _, [MarshalAs (UnmanagedType.SysUInt)] uint index, out CXString original, out CXString transformed);

		[DllImport (LibraryName)]
		 internal static extern void 	clang_remap_dispose (CXRemapping _);
	}
}
