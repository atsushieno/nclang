using System;
using System.Runtime.InteropServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public static class ClangService
	{
		public static CodeCompleteFlags DefaultCodeCompleteOptions {
			get { return (CodeCompleteFlags) LibClang.clang_defaultCodeCompleteOptions (); }
		}

		public static void ToggleCrashRecovery (bool isEnabled)
		{
			LibClang.clang_toggleCrashRecovery ((uint) (isEnabled ? 1 : 0));
		}

		public static ClangCursor GetNullCursor ()
		{
			return new ClangCursor (LibClang.clang_getNullCursor ());
		}

		public static bool IsAttribute (CursorKind kind)
		{
			return LibClang.clang_isAttribute ((CXCursorKind) kind) != 0;
		}

		public static bool IsDeclaration (CursorKind kind)
		{
			return LibClang.clang_isDeclaration ((CXCursorKind) kind) != 0;
		}

		public static bool IsExpression (CursorKind kind)
		{
			return LibClang.clang_isExpression ((CXCursorKind) kind) != 0;
		}

		public static bool IsInvalid (CursorKind kind)
		{
			return LibClang.clang_isInvalid ((CXCursorKind) kind) != 0;
		}

		public static bool IsPreprocessing (CursorKind kind)
		{
			return LibClang.clang_isPreprocessing ((CXCursorKind) kind) != 0;
		}

		public static bool IsReference (CursorKind kind)
		{
			return LibClang.clang_isReference ((CXCursorKind) kind) != 0;
		}

		public static bool IsStatement (CursorKind kind)
		{
			return LibClang.clang_isStatement ((CXCursorKind) kind) != 0;
		}

		public static bool IsTranslationUnit (CursorKind kind)
		{
			return LibClang.clang_isTranslationUnit ((CXCursorKind) kind) != 0;
		}

		public static bool IsUnexposed (CursorKind kind)
		{
			return LibClang.clang_isUnexposed ((CXCursorKind) kind) != 0;
		}

		public static ClangCursorSet CreateCursorSet ()
		{
			return new ClangCursorSet (LibClang.clang_createCXCursorSet ());
		}

		public static void EnableStackTraces ()
		{
			LibClang.clang_enableStackTraces ();
		}

		public static DiagnosticDisplayOptions DefaultDiagnosticDisplayOptions {
			get { return (DiagnosticDisplayOptions) LibClang.clang_defaultDiagnosticDisplayOptions (); }
		}

		public static string ClangVersion {
			get { return LibClang.clang_getClangVersion ().Unwrap (); }
		}

		public static ClangSourceLocation GetNullLocation ()
		{
			return new ClangSourceLocation (LibClang.clang_getNullLocation ());
		}

		public static ClangSourceRange GetNullRange ()
		{
			return new ClangSourceRange (LibClang.clang_getNullRange ());
		}

		public static TranslationUnitFlags DefaultEditingTranslationUnitOptions {
			get { return (TranslationUnitFlags) LibClang.clang_defaultEditingTranslationUnitOptions (); }
		}

		public static ClangIndex CreateIndex (bool excludeDeclarationsFromPch = false, bool displayDiagnostics = false)
		{
			return new ClangIndex (LibClang.clang_createIndex (excludeDeclarationsFromPch ? 1 : 0, displayDiagnostics ? 1 : 0));
		}

		public static ClangCompilationDatabase CreateDatabaseFromDirectory (string buildDir)
		{
			var e = IntPtr.Zero;
			var ret = LibClang.clang_CompilationDatabase_fromDirectory (buildDir, e);
			var error = (CompilationDatabaseError) Marshal.ReadInt32 (e);
			if (error != CompilationDatabaseError.NoError)
				throw new ClangServiceException (string.Format ("Failed to create compilation database from directory '{0}': {1}", buildDir, error));
			return new ClangCompilationDatabase (ret);
		}

		// RemappingFunctions

		public static ClangRemapping GetRemappings (string path)
		{
			return new ClangRemapping (LibClang.clang_getRemappings (path));
		}

		public static ClangRemapping GetRemappingsFromList (string [] filePaths)
		{
			var fps = new NativeArrayHolder(filePaths.ToHGlobalAllocatedArray ());
			var ret = new ClangRemapping (LibClang.clang_getRemappingsFromFileList (fps.NativeArray, (uint) filePaths.Length));
			ret.AddToFreeList (fps);
			return ret;
		}

		// DiagnosticReporting

		public static ClangDiagnosticSet LoadDiagnostics (string file)
		{
			IntPtr e = IntPtr.Zero;
			IntPtr errorString = IntPtr.Zero;
			var ret = LibClang.clang_loadDiagnostics (file, e, errorString);
			var error = (LoadDiagError) Marshal.ReadInt32 (e);
			if (error != LoadDiagError.None) {
				
				throw new ClangServiceException (string.Format (
					"Failed to load diagnostics from '{0}'. Error {1}: {2}", file, error,
					Marshal.PtrToStructure<CXString> (Marshal.ReadIntPtr (errorString)).Unwrap ()));
			}

			return new ClangDiagnosticSet (ret);
		}

		// HighLevelAPI

		public static bool IsEntityObjCContainerKind (IndexEntityKind kind)
		{
			return LibClang.clang_index_isEntityObjCContainerKind ((CXIdxEntityKind) kind) != 0;
		}

		public static class Strings
		{

			public static string ConstructUSRObjCClass (string className)
			{
				return LibClang.clang_constructUSR_ObjCClass (className).Unwrap ();
			}

			public static string ConstructUSRObjCCategory (string className, string categoryName)
			{
				return LibClang.clang_constructUSR_ObjCCategory (className, categoryName).Unwrap ();
			}

			public static string ConstructUSRObjCProtocol (string protocolName)
			{
				return LibClang.clang_constructUSR_ObjCProtocol (protocolName).Unwrap ();
			}

			public static string ConstructUSRObjCIvar (string name, ClangString classUSR)
			{
				return LibClang.clang_constructUSR_ObjCIvar (name, classUSR).Unwrap ();
			}
		}
	}	
}
