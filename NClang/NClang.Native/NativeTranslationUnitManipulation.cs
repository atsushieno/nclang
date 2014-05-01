using System;
using System.Runtime.InteropServices;

using CXIndex = System.IntPtr; // void*
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXTUResourceUsageEntry
	{
		public readonly ResourceUsageKind Kind;
		public readonly ulong Amount;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXTUResourceUsage
	{
		public readonly IntPtr Data; // void*
		[MarshalAs (UnmanagedType.SysUInt)]
		public readonly uint NumEntries;
		public readonly IntPtr Entries; // CXTUResourceUsageEntry*
	}

	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName)]
		internal static extern CXString clang_getTranslationUnitSpelling (CXTranslationUnit ctUnit);

		[DllImport (LibraryName)]
		internal static extern CXTranslationUnit clang_createTranslationUnitFromSourceFile (CXIndex cIdx, string sourceFilename, int numClangCommandLineArgs, [MarshalAs (UnmanagedType.LPArray)] string[] clangCommandLineAgs, [MarshalAs (UnmanagedType.SysUInt)] uint numUnsavedFiles, [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsavedFiles);

		[DllImport (LibraryName)]
		internal static extern CXTranslationUnit clang_createTranslationUnit (CXIndex cIdx, string astFilename);

		[DllImport (LibraryName)]
		internal static extern ErrorCode clang_createTranslationUnit2 (CXIndex cIdx, string astFilename, out CXTranslationUnit outTU);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		internal static extern TranslationUnitFlags clang_defaultEditingTranslationUnitOptions ();

		[DllImport (LibraryName)]
		internal static extern CXTranslationUnit clang_parseTranslationUnit (CXIndex cIdx, string source_filename,
		                                                                     [MarshalAs (UnmanagedType.LPArray)] string[] command_line_args, int num_command_line_args,
		                                                                     [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsaved_files, [MarshalAs (UnmanagedType.SysUInt)] uint num_unsaved_files,
		                                                                     [MarshalAs (UnmanagedType.SysUInt)] TranslationUnitFlags options);

		/* this doesn't exist in LLVM 3.2 nor 3.4.
		[DllImport (LibraryName)]
		internal static extern ErrorCode clang_parseTranslationUnit2 (CXIndex cIdx, string source_filename, 
		                                                              [MarshalAs (UnmanagedType.LPArray)] string[] command_line_args, int num_command_line_args,
		                                                              [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsaved_files, [MarshalAs (UnmanagedType.SysUInt)] uint num_unsaved_files, 
		                                                              [MarshalAs (UnmanagedType.SysUInt)] TranslationUnitFlags options,
		                                                              out CXTranslationUnit out_TU);
		                                                              */

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		internal static extern SaveTranslationUnitFlags clang_defaultSaveOptions (CXTranslationUnit TU);

		[DllImport (LibraryName)]
		internal static extern SaveError clang_saveTranslationUnit (CXTranslationUnit TU, string FileName, [MarshalAs (UnmanagedType.SysUInt)] SaveTranslationUnitFlags options);

		[DllImport (LibraryName)]
		internal static extern void clang_disposeTranslationUnit (CXTranslationUnit _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		internal static extern ReparseTranslationUnitFlags clang_defaultReparseOptions (CXTranslationUnit TU);

		[DllImport (LibraryName)]
		internal static extern ErrorCode clang_reparseTranslationUnit (CXTranslationUnit TU, [MarshalAs (UnmanagedType.SysUInt)] uint num_unsaved_files, [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile [] unsaved_files, [MarshalAs (UnmanagedType.SysUInt)] ReparseTranslationUnitFlags options);

		[DllImport (LibraryName)]
		internal static extern string clang_getTUResourceUsageName (ResourceUsageKind kind);

		[DllImport (LibraryName)]
		internal static extern CXTUResourceUsage clang_getCXTUResourceUsage (CXTranslationUnit TU);

		[DllImport (LibraryName)]
		internal static extern void clang_disposeCXTUResourceUsage (CXTUResourceUsage usage);
	}
}

