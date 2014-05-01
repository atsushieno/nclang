using System;
using System.Runtime.InteropServices;

using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXClientData = System.IntPtr; // void*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	delegate void CXInclusionVisitor (CXFile included_file, IntPtr inclusion_stack, [MarshalAs (UnmanagedType.SysUInt)] uint include_len, CXClientData client_data); // CXSourceLocation*

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_getClangVersion ();

		[DllImport (LibraryName)]
		 internal static extern void 	clang_toggleCrashRecovery ([MarshalAs (UnmanagedType.SysUInt)] uint isEnabled);

		[DllImport (LibraryName)]
		 internal static extern void 	clang_getInclusions (CXTranslationUnit tu, CXInclusionVisitor visitor, CXClientData client_data);
	}
}

