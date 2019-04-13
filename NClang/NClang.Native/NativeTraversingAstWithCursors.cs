#if MANUAL

using System;
using System.Runtime.InteropServices;

using CXClientData = System.IntPtr; // void*

namespace NClang.Natives
{
    [UnmanagedFunctionPointer(LibClang.LibraryCallingConvention)]
	delegate ChildVisitResult CXCursorVisitor (CXCursor cursor,CXCursor parent,CXClientData client_data); // CXChildVisitResult*

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern ChildVisitResult clang_visitChildren (CXCursor parent, CXCursorVisitor visitor, CXClientData client_data);
	}
}

#endif
