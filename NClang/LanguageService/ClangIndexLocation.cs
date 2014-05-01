using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexLocation 
	{
		CXIdxLoc source;

		internal ClangIndexLocation (CXIdxLoc source)
		{
			this.source = source;
		}

		internal CXIdxLoc ToNative ()
		{
			return source;
		}

		public ClangSourceLocation.IndexFileLocation FileLocation {
			get {
				IntPtr idx, f;
				uint l, c, o;
				LibClang.clang_indexLoc_getFileLocation (source, out idx, out f, out l, out c, out o);
				return new ClangSourceLocation.IndexFileLocation (idx, new ClangFile (f), (int) l, (int) c, (int) o);
			}
		}

		public ClangSourceLocation SourceLocation {
			get { return new ClangSourceLocation (LibClang.clang_indexLoc_getCXSourceLocation (source)); }
		}
	}
}
