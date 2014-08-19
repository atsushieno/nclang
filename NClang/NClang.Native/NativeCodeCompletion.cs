using System;
using System.Runtime.InteropServices;

using CXCompletionString = System.IntPtr; // void*
using CXCompletionResultPtr = System.IntPtr; // CXCompletionResultPtr*
using CXCodeCompleteResultsPtr = System.IntPtr; // CXCodeCompleteResults*

using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXDiagnostic = System.IntPtr; // void*
using CXString = NClang.ClangString;

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXCompletionResult
	{
		public readonly CursorKind CursorKind;
		public readonly CXCompletionString CompletionString;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct CXCodeCompletionResults
	{
		public readonly IntPtr Results; // CXCompletionResult[]
		public readonly uint NumResults;
	}

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CompletionChunkKind 	clang_getCompletionChunkKind (CXCompletionString completion_string, uint chunk_number);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCompletionChunkText (CXCompletionString completion_string, uint chunk_number);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCompletionString 	clang_getCompletionChunkCompletionString (CXCompletionString completion_string, uint chunk_number);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_getNumCompletionChunks (CXCompletionString completion_string);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_getCompletionPriority (CXCompletionString completion_string);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern AvailabilityKind 	clang_getCompletionAvailability (CXCompletionString completion_string);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_getCompletionNumAnnotations (CXCompletionString completion_string);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCompletionAnnotation (CXCompletionString completion_string, uint annotation_number);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXString 	clang_getCompletionParent (CXCompletionString completion_string, IntPtr kind); // kind is deprecated, must be always IntPtr.Zero

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getCompletionBriefComment (CXCompletionString completion_string);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCompletionString 	clang_getCursorCompletionString (CXCursor cursor);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CodeCompleteFlags 	clang_defaultCodeCompleteOptions ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXCodeCompleteResultsPtr 	clang_codeCompleteAt (CXTranslationUnit TU, string complete_filename, uint complete_line, uint complete_column,
			[MarshalAs (UnmanagedType.LPArray)] CXUnsavedFile[] unsaved_files, uint num_unsaved_files,
			CodeCompleteFlags options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_sortCodeCompletionResults (CXCompletionResultPtr Results, uint NumResults);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_disposeCodeCompleteResults (CXCodeCompleteResultsPtr Results);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_codeCompleteGetNumDiagnostics (CXCodeCompleteResultsPtr Results);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXDiagnostic 	clang_codeCompleteGetDiagnostic (CXCodeCompleteResultsPtr Results, uint Index);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern SystemLongLong 	clang_codeCompleteGetContexts (CXCodeCompleteResultsPtr Results);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CursorKind 	clang_codeCompleteGetContainerKind (CXCodeCompleteResultsPtr Results, out uint IsIncomplete);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_codeCompleteGetContainerUSR (CXCodeCompleteResultsPtr Results);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_codeCompleteGetObjCSelector (CXCodeCompleteResultsPtr Results);
				
	}
}
