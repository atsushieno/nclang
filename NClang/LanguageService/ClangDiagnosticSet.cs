using System;
using System.Runtime.InteropServices;
using NClang.Natives;
using System.Collections.Generic;
using System.Linq;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangDiagnosticSet : ClangObject, IDisposable
	{
		public ClangDiagnosticSet (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_disposeDiagnosticSet (Handle);
		}

		public int Count {
			get { return (int) LibClang.clang_getNumDiagnosticsInSet (Handle); }
		}

		public ClangDiagnostic Get (int index)
		{
			return new ClangDiagnostic (LibClang.clang_getDiagnosticInSet (Handle, (uint) index));
		}

		public IEnumerable<ClangDiagnostic> Items {
			get { return Enumerable.Range (0, Count).Select (i => Get (i)); }
		}
	}
}
