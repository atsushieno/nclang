using System;
using System.Runtime.InteropServices;

using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXFileUniqueID
	{
		public CXFileUniqueID (SystemULongLong data1, SystemULongLong data2, SystemULongLong data3)
		{
			this.Data1 = data1;
			this.Data2 = data2;
			this.Data3 = data3;
		}

		// unsigned long long [3]
		public readonly SystemULongLong Data1;
		public readonly SystemULongLong Data2;
		public readonly SystemULongLong Data3;
	}

	
	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_getFileName (CXFile sFile);
		
		// not sure how to deal with time_t. So far we can skip it.
		//[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		// internal static extern TimeT clang_getFileTime (CXFile sFile);
		
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern int clang_getFileUniqueID (CXFile file, out CXFileUniqueID outID);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_isFileMultipleIncludeGuarded (CXTranslationUnit tu, CXFile file);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXFile clang_getFile (CXTranslationUnit tu, string fileName);
	}
}

