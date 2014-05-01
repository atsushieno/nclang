using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NClang
{
	
	public class ClangToken
	{
		CXToken source;

		internal ClangToken (CXToken source)
		{
			this.source = source;
		}

		internal CXToken Source {
			get { return source; }
		}

		public TokenKind Kind {
			get { return LibClang.clang_getTokenKind (source); }
		}
	}

}
