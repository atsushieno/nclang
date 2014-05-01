using System;
using System.Runtime.InteropServices;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint	clang_CXXMethod_isPureVirtual (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_CXXMethod_isStatic (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_CXXMethod_isVirtual (CXCursor C);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		internal static extern uint 	clang_CXXMethod_isConst (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CursorKind 	clang_getTemplateCursorKind (CXCursor C);

		[DllImport (LibraryName)]
		 internal static extern CXCursor 	clang_getSpecializedCursorTemplate (CXCursor C);

		[DllImport (LibraryName)]
		internal static extern CXSourceRange 	clang_getCursorReferenceNameRange (CXCursor C, [MarshalAs (UnmanagedType.SysUInt)] NameRefFlags NameFlags, [MarshalAs (UnmanagedType.SysUInt)] uint PieceIndex);
	}
}
