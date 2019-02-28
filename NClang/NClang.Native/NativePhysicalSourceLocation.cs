using System;
using System.Runtime.InteropServices;

using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXFile = System.IntPtr;
using CXSourceRangeListPtr = System.IntPtr;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXSourceLocation
	{
		public readonly IntPtr PtrData1;
		public readonly IntPtr PtrData2;
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
		public readonly int BeginIntData;
		public readonly int EndIntData;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXSourceRangeList
	{
		public readonly uint Count;
		public IntPtr Ranges;
	}

	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation clang_getNullLocation ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_equalLocations (CXSourceLocation loc1, CXSourceLocation loc2);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation clang_getLocation (CXTranslationUnit tu, CXFile file, uint line, uint column);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation clang_getLocationForOffset (CXTranslationUnit tu, CXFile file, uint offset);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern int clang_Location_isInSystemHeader (CXSourceLocation location);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern int clang_Location_isFromMainFile (CXSourceLocation location);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRange clang_getNullRange ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRange clang_getRange (CXSourceLocation begin, CXSourceLocation end);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_equalRanges (CXSourceRange range1, CXSourceRange range2);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern int clang_Range_isNull (CXSourceRange range);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_getExpansionLocation (CXSourceLocation location, out CXFile file, out uint line,  out uint column, out uint offset);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_getPresumedLocation (CXSourceLocation location, out CXString filename, out uint line, out uint column);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_getInstantiationLocation (CXSourceLocation location, out CXFile file, out uint line, out uint column, out uint offset);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_getSpellingLocation (CXSourceLocation location, out CXFile file, out uint line, out uint column, out uint offset);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_getFileLocation (CXSourceLocation location, out CXFile file, out uint line, out uint column, out uint offset);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation clang_getRangeStart (CXSourceRange range);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation clang_getRangeEnd (CXSourceRange range);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRangeListPtr clang_getSkippedRanges (CXTranslationUnit tu, CXFile file);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRangeListPtr clang_getAllSkippedRanges (CXTranslationUnit tu);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_disposeSourceRangeList (CXSourceRangeListPtr ranges);
	}
}

