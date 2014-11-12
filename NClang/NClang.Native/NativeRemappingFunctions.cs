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
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXRemapping 	clang_getRemappings (string path);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXRemapping 	clang_getRemappingsFromFileList ([MarshalAs (UnmanagedType.LPArray)] string[] filePaths, uint numFiles);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_remap_getNumFiles (CXRemapping _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_remap_getFilenames (CXRemapping _, uint index, out CXString original, out CXString transformed);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_remap_dispose (CXRemapping _);
	}
}
