using System;
using NClang.Natives;

namespace NClang
{
	public class ClangCursorSet : ClangObject, IDisposable
	{
		public ClangCursorSet (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_disposeCXCursorSet (Handle);
		}

		public bool Contains (ClangCursor cursor)
		{
			return LibClang.clang_CXCursorSet_contains (Handle, cursor.Source) != 0;
		}

		public bool Insert (ClangCursor cursor)
		{
			return LibClang.clang_CXCursorSet_insert (Handle, cursor.Source) != 0;
		}
	}	
}
