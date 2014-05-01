using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NClang
{
	
	public class ClangTokenSet : IDisposable
	{
		internal ClangTokenSet (IntPtr translationUnit, IntPtr tokens, int count)
		{
			this.tu = translationUnit;
			this.tokens = tokens;
			this.count = count;
		}

		readonly IntPtr tu, tokens;
		readonly int count;

		public void Dispose ()
		{
			LibClang.clang_disposeTokens (tu, tokens, (uint) count);
		}

		static readonly int cxtoken_size = Marshal.SizeOf<CXToken> ();

		public IEnumerable<ClangToken> Tokens {
			get { return Enumerable.Range (0, count).Select (i => new ClangToken (Marshal.PtrToStructure<CXToken> (tokens + cxtoken_size * i))); }
		}

		static readonly int cxcursor_size = Marshal.SizeOf<CXCursor> ();

		public IEnumerable<ClangCursor> Annotate ()
		{
			IntPtr cursors = IntPtr.Zero;
			LibClang.clang_annotateTokens (tu, tokens, (uint) count, ref cursors);
			return Enumerable.Range (0, count).Select (i => new ClangCursor (Marshal.PtrToStructure<CXCursor> (cursors + cxcursor_size * i)));
		}
	}

}
