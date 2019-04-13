using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangPlatformAvailability : ClangObject, IDisposable
	{
		CXPlatformAvailability source;

		internal ClangPlatformAvailability (IntPtr handle)
			: base (handle)
		{
			this.source = (CXPlatformAvailability)Marshal.PtrToStructure (handle, typeof(CXPlatformAvailability));
		}

		public void Dispose ()
		{
			LibClang.clang_disposeCXPlatformAvailability (Handle);
		}

		public string Platform {
			get { return source.Platform.Unwrap (); }
		}

		public ClangVersion Introduced {
			get { return new ClangVersion (source.Introduced); }
		}

		public ClangVersion Deprecated {
			get { return new ClangVersion (source.Deprecated); }
		}

		public ClangVersion Obsoleted {
			get { return new ClangVersion (source.Obsoleted); }
		}

		public bool IsUnavailable {
			get { return source.Unavailable != 0; }
		}

		public string Message {
			get { return source.Message.Unwrap (); }
		}
	}
}
