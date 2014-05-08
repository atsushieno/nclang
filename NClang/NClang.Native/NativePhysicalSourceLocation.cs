using System;
using System.Runtime.InteropServices;

using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXFile = System.IntPtr;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXSourceLocation
	{
		public readonly IntPtr PtrData1;
		public readonly IntPtr PtrData2;
		[MarshalAs (UnmanagedType.SysUInt)]
		public readonly uint IntData;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXSourceRange
	{
		public CXSourceRange (int begin, int end)
		{
			PtrData1 = PtrData2 = IntPtr.Zero;
			BeginIntData = begin;
			EndIntData = end;
		}

		public readonly IntPtr PtrData1;
		public readonly IntPtr PtrData2;
		[MarshalAs (UnmanagedType.SysUInt)]
		public readonly int BeginIntData;
		[MarshalAs (UnmanagedType.SysUInt)]
		public readonly int EndIntData;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXSourceRangeList
	{
		[MarshalAs (UnmanagedType.SysUInt)]
		public readonly uint Count;
		public IntPtr Ranges;
	}

	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation clang_getNullLocation ();

		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.SysUInt)]
		 internal static extern uint clang_equalLocations (CXSourceLocation loc1, CXSourceLocation loc2);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation clang_getLocation (CXTranslationUnit tu, CXFile file, [MarshalAs (UnmanagedType.SysUInt)] uint line, [MarshalAs (UnmanagedType.SysUInt)] uint column);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation clang_getLocationForOffset (CXTranslationUnit tu, CXFile file, [MarshalAs (UnmanagedType.SysUInt)] uint offset);

		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.SysInt)]
		 internal static extern int clang_Location_isInSystemHeader (CXSourceLocation location);

		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.SysInt)]
		 internal static extern int clang_Location_isFromMainFile (CXSourceLocation location);

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange clang_getNullRange ();

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange clang_getRange (CXSourceLocation begin, CXSourceLocation end);

		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.SysUInt)]
		 internal static extern uint clang_equalRanges (CXSourceRange range1, CXSourceRange range2);

		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.SysInt)]
		 internal static extern int clang_Range_isNull (CXSourceRange range);

		[DllImport (LibraryName)]
		 internal static extern void clang_getExpansionLocation (CXSourceLocation location, out CXFile file, [MarshalAs (UnmanagedType.SysUInt)] out uint line, [MarshalAs (UnmanagedType.SysUInt)]  out uint column, [MarshalAs (UnmanagedType.SysUInt)] out uint offset);

		[DllImport (LibraryName)]
		 internal static extern void clang_getPresumedLocation (CXSourceLocation location, out CXString filename, [MarshalAs (UnmanagedType.SysUInt)] out uint line, [MarshalAs (UnmanagedType.SysUInt)] out uint column);

		[DllImport (LibraryName)]
		 internal static extern void clang_getInstantiationLocation (CXSourceLocation location, out CXFile file, [MarshalAs (UnmanagedType.SysUInt)] out uint line, [MarshalAs (UnmanagedType.SysUInt)] out uint column, [MarshalAs (UnmanagedType.SysUInt)] out uint offset);

		[DllImport (LibraryName)]
		 internal static extern void clang_getSpellingLocation (CXSourceLocation location, out CXFile file, [MarshalAs (UnmanagedType.SysUInt)] out uint line, [MarshalAs (UnmanagedType.SysUInt)] out uint column, [MarshalAs (UnmanagedType.SysUInt)] out uint offset);

		[DllImport (LibraryName)]
		 internal static extern void clang_getFileLocation (CXSourceLocation location, out CXFile file, [MarshalAs (UnmanagedType.SysUInt)] out uint line, [MarshalAs (UnmanagedType.SysUInt)] out uint column, [MarshalAs (UnmanagedType.SysUInt)] out uint offset);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation clang_getRangeStart (CXSourceRange range);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation clang_getRangeEnd (CXSourceRange range);

		/* does not exist in clang 3.5
		[DllImport (LibraryName)]
		 internal static extern IntPtr clang_getSkippedRanges (CXTranslationUnit tu, CXFile file); // CXSourceRangeList*
		*/

		[DllImport (LibraryName)]
		internal static extern void clang_disposeSourceRangeList (IntPtr ranges); // CXSourceRangeList*
	}
}

