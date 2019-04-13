#if MANUAL

using System;
using System.Runtime.InteropServices;

using CXIndex = System.IntPtr; // void*
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXTargetInfo = System.IntPtr; // CXTargetInfoImpl*

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
		public readonly uint NumEntries;
		public readonly IntPtr Entries; // CXTUResourceUsageEntry*
	}

	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXString clang_getTranslationUnitSpelling (CXTranslationUnit ctUnit);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXTranslationUnit clang_createTranslationUnitFromSourceFile (CXIndex cIdx, string sourceFilename, int numClangCommandLineArgs, [MarshalAs (UnmanagedType.LPArray)] string[] clangCommandLineAgs, uint numUnsavedFiles, [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsavedFiles);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXTranslationUnit clang_createTranslationUnit (CXIndex cIdx, string astFilename);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern ErrorCode clang_createTranslationUnit2 (CXIndex cIdx, string astFilename, out CXTranslationUnit outTU);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern TranslationUnitFlags clang_defaultEditingTranslationUnitOptions ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXTranslationUnit clang_parseTranslationUnit (CXIndex cIdx, string source_filename,
		                                                                     [MarshalAs (UnmanagedType.LPArray)] string[] command_line_args, int num_command_line_args,
		                                                                     [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsaved_files, uint num_unsaved_files,
		                                                                     TranslationUnitFlags options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern ErrorCode clang_parseTranslationUnit2 (CXIndex cIdx, string source_filename, 
		                                                              [MarshalAs (UnmanagedType.LPArray)] string[] command_line_args, int num_command_line_args,
		                                                              [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsaved_files, uint num_unsaved_files, 
		                                                              TranslationUnitFlags options,
		                                                              out CXTranslationUnit out_TU);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern ErrorCode clang_parseTranslationUnit2FullArgv (CXIndex cIdx, string source_filename,
									      [MarshalAs (UnmanagedType.LPArray)] string [] command_line_args, int num_command_line_args,
									      [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile [] unsaved_files, uint num_unsaved_files,
									      TranslationUnitFlags options,
									      out CXTranslationUnit out_TU);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern SaveTranslationUnitFlags clang_defaultSaveOptions (CXTranslationUnit TU);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern SaveError clang_saveTranslationUnit (CXTranslationUnit TU, string FileName, SaveTranslationUnitFlags options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern uint clang_suspendTranslationUnit (CXTranslationUnit _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_disposeTranslationUnit (CXTranslationUnit _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern ReparseTranslationUnitFlags clang_defaultReparseOptions (CXTranslationUnit TU);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern ErrorCode clang_reparseTranslationUnit (CXTranslationUnit TU, uint num_unsaved_files, [MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile [] unsaved_files, ReparseTranslationUnitFlags options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern string clang_getTUResourceUsageName (ResourceUsageKind kind);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXTUResourceUsage clang_getCXTUResourceUsage (CXTranslationUnit TU);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_disposeCXTUResourceUsage (CXTUResourceUsage usage);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXTargetInfo clang_getTranslationUnitTargetInfo (CXTranslationUnit CTUnit);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_TargetInfo_dispose (CXTargetInfo Info);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXString clang_TargetInfo_getTriple (CXTargetInfo Info);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern int clang_TargetInfo_getPointerWidth (CXTargetInfo Info);
	}
}

#endif
