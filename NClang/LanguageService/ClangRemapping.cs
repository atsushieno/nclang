using System;
using System.Runtime.InteropServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangRemapping : ClangDisposable
	{
		public struct FileNameMap
		{
			readonly string original, transformed;

			public FileNameMap (string original, string transformed)
			{
				this.original = original;
				this.transformed = transformed;
			}

			public string Original {
				get { return original; }
			}
			public string Transformed {
				get { return transformed; }
			}
		}

		public ClangRemapping (IntPtr handle)
			: base (handle)
		{
		}

		public override void Dispose ()
		{
			LibClang.clang_remap_dispose (Handle);
			base.Dispose ();
		}

		public int FileCount {
			get { return (int) LibClang.clang_remap_getNumFiles (Handle); }
		}

		public FileNameMap GetFileNames (int index)
		{
			Pointer<CXString> original = IntPtr.Zero, transformed = IntPtr.Zero;
			LibClang.clang_remap_getFilenames (Handle, (uint) index, original, transformed);
			return new FileNameMap (Marshal.PtrToStructure<CXString> (Marshal.ReadIntPtr (original)).Unwrap (), Marshal.PtrToStructure<CXString> (Marshal.ReadIntPtr (transformed)).Unwrap ());
		}
	}	
}
