using System;
using System.Runtime.InteropServices;

using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXClientData = System.IntPtr; // void*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
    [UnmanagedFunctionPointer(LibClang.LibraryCallingConvention)]
	delegate void CXInclusionVisitor (CXFile included_file, IntPtr inclusion_stack, uint include_len, CXClientData client_data); // CXSourceLocation*

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getClangVersion ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_toggleCrashRecovery (uint isEnabled);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_getInclusions (CXTranslationUnit tu, CXInclusionVisitor visitor, CXClientData client_data);
	}
}

