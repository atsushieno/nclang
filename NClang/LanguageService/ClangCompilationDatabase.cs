using System;
using NClang.Natives;

using CXString = NClang.ClangString;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangCompilationDatabase : ClangObject, IDisposable
	{
		public ClangCompilationDatabase (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_CompilationDatabase_dispose (Handle);
		}

		public ClangCompileCommands GetAllCompileCommands ()
		{
			return new ClangCompileCommands (LibClang.clang_CompilationDatabase_getAllCompileCommands (Handle));
		}

		public ClangCompileCommands GetCompileCommands (string completeFileName)
		{
			return new ClangCompileCommands (LibClang.clang_CompilationDatabase_getCompileCommands (Handle, completeFileName));
		}
	}
}
