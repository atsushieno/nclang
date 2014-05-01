using System;
using System.Runtime.InteropServices;
using NClang.Natives;

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
			return new ClangDiagnostic (LibClang.clang_getDiagnostic (Handle, (uint) index));
		}
	}
}
