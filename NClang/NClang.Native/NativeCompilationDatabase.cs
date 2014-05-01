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
		[DllImport (LibraryName)]
		 internal static extern CXCompilationDatabase clang_CompilationDatabase_fromDirectory (string buildDir, out CompilationDatabaseError errorCode);

		[DllImport (LibraryName)]
		 internal static extern void clang_CompilationDatabase_dispose (CXCompilationDatabase _);

		[DllImport (LibraryName)]
		 internal static extern CXCompileCommand clang_CompilationDatabase_getCompileCommands (CXCompilationDatabase _, string completeFileName);

		[DllImport (LibraryName)]
		 internal static extern CXCompileCommand clang_CompilationDatabase_getAllCompileCommands (CXCompilationDatabase _);

		[DllImport (LibraryName)]
		 internal static extern void clang_CompileCommands_dispose (CXCompileCommands _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_CompileCommands_getSize (CXCompileCommands _);

		[DllImport (LibraryName)]
		 internal static extern CXCompileCommand clang_CompileCommands_getCommand (CXCompileCommands _, [MarshalAs (UnmanagedType.SysUInt)] uint i);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_CompileCommand_getDirectory (CXCompileCommand _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_CompileCommand_getNumArgs (CXCompileCommand _);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_CompileCommand_getArg (CXCompileCommand _, [MarshalAs (UnmanagedType.SysUInt)] uint i);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_CompileCommand_getNumMappedSources (CXCompileCommand _);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_CompileCommand_getMappedSourcePath (CXCompileCommand _, [MarshalAs (UnmanagedType.SysUInt)] uint i);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_CompileCommand_getMappedSourceContent (CXCompileCommand _, [MarshalAs (UnmanagedType.SysUInt)] uint i);
	}
}

