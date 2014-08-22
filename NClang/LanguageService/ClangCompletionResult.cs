using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NClang
{
	
	public class ClangCompletionResult
	{
		internal ClangCompletionResult (IntPtr handle)
		{
			this.handle = handle;
			// This handle does not have to be freed; CXCodeCompletionResults takes care of it.
            source = (CXCompletionResult)Marshal.PtrToStructure(handle, typeof(CXCompletionResult));
		}

		IntPtr handle;
		CXCompletionResult source;

		public CursorKind CursorKind {
			get { return source.CursorKind; }
		}

		public ClangCompletionString CompletionString {
			get { return new ClangCompletionString (source.CompletionString); }
		}

		public void Sort (int numResults)
		{
			LibClang.clang_sortCodeCompletionResults (handle, (uint) numResults);
		}
	}

}
