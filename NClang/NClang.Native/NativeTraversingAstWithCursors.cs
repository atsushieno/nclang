using System;
using System.Runtime.InteropServices;

using CXClientData = System.IntPtr; // void*

namespace NClang.Natives
{
	delegate ChildVisitResult CXCursorVisitor (CXCursor cursor,CXCursor parent,CXClientData client_data); // CXChildVisitResult*

	// done
	static partial class LibClang
	{
		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		internal static extern ChildVisitResult clang_visitChildren (CXCursor parent, CXCursorVisitor visitor, CXClientData client_data);
	}
}
