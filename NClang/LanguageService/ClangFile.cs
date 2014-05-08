using System;
using NClang.Natives;
using System.Linq;

using CXFile = System.IntPtr;

namespace NClang
{
	public class ClangFile : ClangObject
	{
		internal ClangFile (CXFile handle)
			: base (handle)
		{
		}

		public string FileName {
			get { return LibClang.clang_getFileName (Handle).Unwrap (); }
		}

		/*
		public DateTime FileTime {
			get { return LibClang.clang_getFileTime (handle); }
		}
		*/

		public ClangFileUniqueId FileUniqueId {
			get {
				CXFileUniqueID uid;
				var ret = LibClang.clang_getFileUniqueID (Handle, out uid);
				if (ret != 0)
					throw new ClangServiceException (string.Format ("Failed to acquire file unique ID for \"{0}\" - error code {1}", FileName, ret));
				return new ClangFileUniqueId (uid);
			}
		}

		public override string ToString ()
		{
			return FileName;
		}
	}
}
