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

		static readonly int cxtoken_size = Marshal.SizeOf (typeof(CXToken));

		public IEnumerable<ClangToken> Tokens {
			get { return Enumerable.Range (0, count).Select (i => new ClangToken (tu, (CXToken)Marshal.PtrToStructure (tokens + cxtoken_size * i, typeof(CXToken)))); }
		}

		static readonly int cxcursor_size = Marshal.SizeOf (typeof(CXCursor));

		public IEnumerable<ClangCursor> Annotate ()
		{
			IntPtr cursors = IntPtr.Zero;
			LibClang.clang_annotateTokens (tu, tokens, (uint) count, ref cursors);
			return Enumerable.Range (0, count).Select (i => new ClangCursor ((CXCursor)Marshal.PtrToStructure (cursors + cxcursor_size * i, typeof(CXCursor))));
		}
	}

}
