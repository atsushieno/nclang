using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NClang
{
	
	public class ClangToken
	{
		readonly IntPtr tu;
		readonly CXToken source;

		internal ClangToken (IntPtr translationUnit, CXToken source)
		{
			this.tu = translationUnit;
			this.source = source;
		}

		public TokenKind Kind {
			get { return LibClang.clang_getTokenKind (source); }
		}

		public string Spelling {
			get { return LibClang.clang_getTokenSpelling (tu, source).Unwrap (); }
		}

		public ClangSourceLocation Location {
			get { return new ClangSourceLocation (LibClang.clang_getTokenLocation (tu, source)); }
		}

		public ClangSourceRange Extent {
			get { return new ClangSourceRange (LibClang.clang_getTokenExtent (tu, source)); }
		}
	}

}
