using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using CXFile = System.IntPtr;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangFile : ClangObject
	{
		public static bool Equals (ClangFile file1, ClangFile file2)
		{
			return LibClang.clang_File_isEqual (file1.Handle, file2.Handle) != 0;
		}

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
				Pointer<CXFileUniqueID> uid = default (Pointer<CXFileUniqueID>);
				var ret = LibClang.clang_getFileUniqueID (Handle, uid);
				if (ret != 0)
					throw new ClangServiceException (string.Format ("Failed to acquire file unique ID for \"{0}\" - error code {1}", FileName, ret));
				return new ClangFileUniqueId (Marshal.PtrToStructure<CXFileUniqueID> (uid));
			}
		}

		public override string ToString ()
		{
			return FileName;
		}

		public string TryGetRealPathName ()
		{
			return LibClang.clang_File_tryGetRealPathName (Handle).Unwrap ();
		}
	}
}
