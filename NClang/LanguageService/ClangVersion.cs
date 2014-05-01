using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

namespace NClang
{
	public class ClangVersion
	{
		CXVersion source;

		internal ClangVersion (CXVersion source)
		{
			this.source = source;
		}

		public int Major {
			get { return source.Major; }
		}

		public int Minor {
			get { return source.Minor; }
		}

		public int SubMinor {
			get { return source.SubMinor; }
		}
	}
}
