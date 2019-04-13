using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using LibClang = NClang.Natives.Natives; 

namespace NClang
{
	
	public class ClangCodeCompleteResults : ClangObject, IDisposable
	{
		static readonly int result_size = Marshal.SizeOf (typeof(CXCompletionResult));
		CXCodeCompleteResults? source;

		public ClangCodeCompleteResults (IntPtr handle)
			: base (handle)
		{
		}

		CXCodeCompleteResults Source {
			get { return (CXCodeCompleteResults)(source ?? (source = (CXCodeCompleteResults)Marshal.PtrToStructure (Handle, typeof(CXCodeCompleteResults)))); }
		}

		public int ResultCount {
			get { return (int)Source.NumResults; }
		}

        public void Sort()
        {
		LibClang.clang_sortCodeCompletionResults(Source.Results, Source.NumResults);
        }

        public IEnumerable<ClangCompletionResult> Results {
			get {
				for (int i = 0; i < Source.NumResults; i++)
					yield return new ClangCompletionResult ((IntPtr) Source.Results + result_size * i);
			}
		}

		public void Dispose ()
		{
			LibClang.clang_disposeCodeCompleteResults (Handle);
		}

		public int DiagnosticsCount {
			get { return (int)LibClang.clang_codeCompleteGetNumDiagnostics (Handle); }
		}

		public ClangDiagnostic GetDiagnostic (int index)
		{
			return new ClangDiagnostic (LibClang.clang_codeCompleteGetDiagnostic (Handle, (uint)index));
		}

		public CompletionContext Contexts {
			get { return (CompletionContext)LibClang.clang_codeCompleteGetContexts (Handle); }
		}

		public CursorKind GetContainerKind (out bool isComplete)
		{
			Pointer<uint> ic = default (Pointer<uint>);
			var ret = LibClang.clang_codeCompleteGetContainerKind (Handle, ic);
			isComplete = Marshal.ReadInt32 (ic.Handle) != 0;
			return (CursorKind) ret;
		}

		public string ContainerUSR {
			get { return ContainerUSRNative.Unwrap (); }
		}

		CXString ContainerUSRNative {
			get { return LibClang.clang_codeCompleteGetContainerUSR (Handle); }
		}

		public string GetObjCSelector ()
		{
			return LibClang.clang_codeCompleteGetObjCSelector (Handle).Unwrap ();
		}
	}
}
