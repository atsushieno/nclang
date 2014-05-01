using System;
using NClang.Natives;

using CXString = NClang.ClangString;

namespace NClang
{
	public static class ClangService
	{
		public static CodeCompleteFlags DefaultCodeCompleteOptions {
			get { return LibClang.clang_defaultCodeCompleteOptions (); }
		}

		public static void ToggleCrashRecovery (bool isEnabled)
		{
			LibClang.clang_toggleCrashRecovery ((uint) (isEnabled ? 1 : 0));
		}

		public static ClangCursor GetNullCursor ()
		{
			return new ClangCursor (LibClang.clang_getNullCursor ());
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
			get { return LibClang.clang_defaultDiagnosticDisplayOptions (); }
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
			get { return LibClang.clang_defaultEditingTranslationUnitOptions (); }
		}

		public static ClangIndex CreateIndex (bool excludeDeclarationsFromPch = false, bool displayDiagnostics = false)
		{
			return new ClangIndex (LibClang.clang_createIndex (excludeDeclarationsFromPch ? 1 : 0, displayDiagnostics ? 1 : 0));
		}

		public static ClangCompilationDatabase CreateDatabaseFromDirectory (string buildDir)
		{
			CompilationDatabaseError error;
			var ret = LibClang.clang_CompilationDatabase_fromDirectory (buildDir, out error);
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
			return new ClangRemapping (LibClang.clang_getRemappingsFromFileList (filePaths, (uint) filePaths.Length));
		}

		// DiagnosticReporting

		public static ClangDiagnosticSet LoadDiagnostics (string file)
		{
			LoadDiagError error;
			CXString errorString;
			var ret = LibClang.clang_loadDiagnostics (file, out error, out errorString);
			if (error != LoadDiagError.None)
				throw new ClangServiceException (string.Format ("Failed to load diagnostics from '{0}'. Error {1}: {2}", file, error, errorString.Unwrap ()));
			return new ClangDiagnosticSet (ret);
		}

		// HighLevelAPI

		public static bool IsEntityObjCContainerKind (IndexEntityKind kind)
		{
			return LibClang.clang_index_isEntityObjCContainerKind (kind) != 0;
		}

		public static class Strings
		{

			public static string ConstructUSRObjCClass (string className)
			{
				return ConstructUSRObjCClassNative (className).Unwrap ();
			}

			public static ClangString ConstructUSRObjCClassNative (string className)
			{
				return LibClang.clang_constructUSR_ObjCClass (className);
			}

			public static string ConstructUSRObjCCategory (string className, string categoryName)
			{
				return ConstructUSRObjCCategoryNative (className, categoryName).Unwrap ();
			}

			public static ClangString ConstructUSRObjCCategoryNative (string className, string categoryName)
			{
				return LibClang.clang_constructUSR_ObjCCategory (className, categoryName);
			}

			public static string ConstructUSRObjCProtocol (string protocolName)
			{
				return ConstructUSRObjCProtocolNative (protocolName).Unwrap ();
			}

			public static ClangString ConstructUSRObjCProtocolNative (string protocolName)
			{
				return LibClang.clang_constructUSR_ObjCProtocol (protocolName);
			}

			public static string ConstructUSRObjCIvar (string name, ClangString classUSR)
			{
				return ConstructUSRObjCIvarNative (name, classUSR).Unwrap ();
			}

			public static ClangString ConstructUSRObjCIvarNative (string name, ClangString classUSR)
			{
				return LibClang.clang_constructUSR_ObjCIvar (name, classUSR);
			}
		}
	}	
}
