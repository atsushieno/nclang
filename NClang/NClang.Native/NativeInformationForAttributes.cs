using System;
using System.Runtime.InteropServices;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXType 	clang_getIBOutletCollectionType (CXCursor _);
	}
}

