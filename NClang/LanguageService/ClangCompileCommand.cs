using System;
using NClang.Natives;

using CXString = NClang.ClangString;

namespace NClang
{
	public class ClangCompileCommand : ClangObject
	{
		public ClangCompileCommand (IntPtr handle)
			: base (handle)
		{
			// no need to dispose the handle. CXCompileCommands shoulta take care of it.
		}

		public string Directory {
			get { return LibClang.clang_CompileCommand_getDirectory (Handle).Unwrap (); }
		}

		public int ArgumentCount {
			get { return (int)LibClang.clang_CompileCommand_getNumArgs (Handle); }
		}

		public string GetArgument (int index)
		{
			return LibClang.clang_CompileCommand_getArg (Handle, (uint) index).Unwrap ();
		}

		public int MappedSourceCount {
			get { return (int) LibClang.clang_CompileCommand_getNumMappedSources (Handle); }
		}

		public string GetMappedSourcePath (int index)
		{
			return LibClang.clang_CompileCommand_getMappedSourcePath (Handle, (uint) index).Unwrap ();
		}

		public string GetMappedSourceContent (int index)
		{
			return LibClang.clang_CompileCommand_getMappedSourceContent (Handle, (uint) index).Unwrap ();
		}
	}

}
