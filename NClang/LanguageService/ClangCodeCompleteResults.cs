using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NClang
{
	
	public class ClangCodeCompleteResults : ClangObject, IDisposable
	{
		static readonly int result_size = Marshal.SizeOf<CXCompletionResult> ();
		CXCodeCompletionResults? source;

		public ClangCodeCompleteResults (IntPtr handle)
			: base (handle)
		{
		}

		CXCodeCompletionResults Source {
			get { return (CXCodeCompletionResults)(source ?? (source = Marshal.PtrToStructure<CXCodeCompletionResults> (Handle))); }
		}

		public int ResultCount {
			get { return (int) Source.NumResults; }
		}

		public IEnumerable<ClangCompletionResult> Results {
			get {
				for (int i = 0; i < Source.NumResults; i++)
					yield return new ClangCompletionResult (Source.Results + result_size * i);
			}
		}

		public void Dispose ()
		{
			LibClang.clang_disposeCodeCompleteResults (Handle);
		}

		public int DiagnosticsCount {
			get { return (int) LibClang.clang_codeCompleteGetNumDiagnostics (Handle); }
		}

		public ClangDiagnostic GetDiagnostic (int index)
		{
			return new ClangDiagnostic (LibClang.clang_codeCompleteGetDiagnostic (Handle, (uint) index));
		}

		public CompletionContext Contexts {
			get { return (CompletionContext) LibClang.clang_codeCompleteGetContexts (Handle); }
		}

		public CursorKind GetContainerKind (out bool isComplete)
		{
			uint ic;
			var ret = LibClang.clang_codeCompleteGetContainerKind (Handle, out ic);
			isComplete = ic != 0;
			return ret;
		}

		public string ContainerUSR {
			get { return ContainerUSRNative.Unwrap (); }
		}

		public ClangString ContainerUSRNative {
			get { return LibClang.clang_codeCompleteGetContainerUSR (Handle); }
		}

		public string GetObjCSelector ()
		{
			return LibClang.clang_codeCompleteGetObjCSelector (Handle).Unwrap ();
		}
	}
}
