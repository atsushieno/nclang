#if MANUAL

using System;
using System.Runtime.InteropServices;

using CXModule = System.IntPtr; // void*
using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXModule 	clang_Cursor_getModule (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXModule clang_Cursor_getModuleForFile (CXTranslationUnit _, CXFile __);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXFile 	clang_Module_getASTFile (CXModule Module);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXModule 	clang_Module_getParent (CXModule Module);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_Module_getName (CXModule Module);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_Module_getFullName (CXModule Module);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_Module_getNumTopLevelHeaders (CXTranslationUnit _, CXModule Module);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXFile 	clang_Module_getTopLevelHeader (CXTranslationUnit _, CXModule Module, uint Index);
	}
}

#endif
