using System;
using System.Runtime.InteropServices;

using CXIndex = System.IntPtr; // void*
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXClientData = System.IntPtr; // void*

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXIndex clang_createIndex (int excludeDeclarationsFromPCH, int displayDiagnostics);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_disposeIndex (CXIndex index);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_CXIndex_setGlobalOptions (CXIndex _, GlobalOptionFlags options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern GlobalOptionFlags clang_CXIndex_getGlobalOptions (CXIndex _);
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXUnsavedFile
	{
		public CXUnsavedFile (string filename, string contents)
		{
			this.FileName = filename;
			this.Contents = contents;
			this.Length = (ulong) contents.Length;
		}

		public readonly string FileName;
		public readonly string Contents;
		public readonly ulong Length;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXVersion
	{
		public CXVersion (int major, int minor, int subminor)
		{
			Major = major;
			Minor = minor;
			SubMinor = subminor;
		}

		public readonly int Major;
		public readonly int Minor;
		public readonly int SubMinor;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXCursor
	{
		public CXCursor (CursorKind kind, int xData, IntPtr[] data)
		{
			this.Kind = kind;
			this.XData = xData;
			this.Data1 = data [0];
			this.Data2 = data [1];
			this.Data3 = data [2];
		}

		public readonly CursorKind Kind;
		public readonly int XData;
		// void* [3]
		public readonly IntPtr Data1;
		public readonly IntPtr Data2;
		public readonly IntPtr Data3;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXComment
	{
		public readonly IntPtr ASTNode;
		public readonly CXTranslationUnit TranslationUnit;
	}
}

