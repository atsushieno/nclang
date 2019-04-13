using System;
using NClang.Natives;

using CXString = NClang.ClangString;
using System.Collections.Generic;
using System.Linq;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangCompileCommands : ClangObject, IDisposable
	{
		public ClangCompileCommands (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_CompileCommands_dispose (Handle);
		}

		public int Count {
			get { return (int) LibClang.clang_CompileCommands_getSize (Handle); }
		}

		public ClangCompileCommand GetCommand (int index)
		{
			return new ClangCompileCommand (LibClang.clang_CompileCommands_getCommand (Handle, (uint) index));
		}

		public IEnumerable<ClangCompileCommand> Commands {
			get { return Enumerable.Range (0, Count).Select (i => GetCommand (i)); }
		}
	}
}
