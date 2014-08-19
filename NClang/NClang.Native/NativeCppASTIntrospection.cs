using System;
using System.Runtime.InteropServices;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint	clang_CXXMethod_isPureVirtual (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_CXXMethod_isStatic (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_CXXMethod_isVirtual (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern uint 	clang_CXXMethod_isConst (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CursorKind 	clang_getTemplateCursorKind (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCursor 	clang_getSpecializedCursorTemplate (CXCursor C);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXSourceRange 	clang_getCursorReferenceNameRange (CXCursor C, NameRefFlags NameFlags, uint PieceIndex);
	}
}
