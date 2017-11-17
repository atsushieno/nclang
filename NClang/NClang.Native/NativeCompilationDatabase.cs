using System;

using CXCompilationDatabase = System.IntPtr; // void*
using CXCompileCommands = System.IntPtr; // void*
using CXCompileCommand = System.IntPtr; // void*
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCompilationDatabase clang_CompilationDatabase_fromDirectory (string buildDir, out CompilationDatabaseError errorCode);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_CompilationDatabase_dispose (CXCompilationDatabase _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCompileCommands clang_CompilationDatabase_getCompileCommands (CXCompilationDatabase _, string completeFileName);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCompileCommands clang_CompilationDatabase_getAllCompileCommands (CXCompilationDatabase _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_CompileCommands_dispose (CXCompileCommands _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_CompileCommands_getSize (CXCompileCommands _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXCompileCommand clang_CompileCommands_getCommand (CXCompileCommands _, uint l);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_CompileCommand_getDirectory (CXCompileCommand _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXString clang_CompileCommand_getFilename (CXCompileCommand _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_CompileCommand_getNumArgs (CXCompileCommand _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_CompileCommand_getArg (CXCompileCommand _, uint i);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_CompileCommand_getNumMappedSources (CXCompileCommand _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_CompileCommand_getMappedSourcePath (CXCompileCommand _, uint i);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_CompileCommand_getMappedSourceContent (CXCompileCommand _, uint i);
	}
}

