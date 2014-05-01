using System;
using NClang.Natives;

using CXString = NClang.ClangString;
using System.Collections.Generic;

namespace NClang
{
	public class ClangRemapping : ClangObject, IDisposable
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

		public void Dispose ()
		{
			LibClang.clang_remap_dispose (Handle);
		}

		public int FileCount {
			get { return (int) LibClang.clang_remap_getNumFiles (Handle); }
		}

		public FileNameMap GetFileNames (int index)
		{
			ClangString original, transformed;
			LibClang.clang_remap_getFilenames (Handle, (uint) index, out original, out transformed);
			return new FileNameMap (original.Unwrap (), transformed.Unwrap ());
		}
	}	
}
