using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

namespace NClang
{
	
	public class ClangTranslationUnit : ClangObject, IDisposable
	{
		// TranslationUnitManipulation

		public static TranslationUnitFlags DefaultEditingTranslationUnitOptions {
			get { return ClangService.DefaultEditingTranslationUnitOptions; }
		}

		public static string GetResourceUsageName (ResourceUsageKind kind)
		{
			return LibClang.clang_getTUResourceUsageName (kind);
		}

		internal ClangTranslationUnit (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_disposeTranslationUnit (Handle);
		}

		public ReparseTranslationUnitFlags DefaultReparseOptions {
			get { return LibClang.clang_defaultReparseOptions (Handle); }
		}

		public SaveTranslationUnitFlags DefaultSaveOptions {
			get { return LibClang.clang_defaultSaveOptions (Handle); }
		}

		public override bool Equals (object obj)
		{
			return Equals (obj as ClangTranslationUnit);
		}

		public bool Equals (ClangTranslationUnit other)
		{
			return other != null && Handle == other.Handle;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
		
		public string TranslationUnitSpelling {
			get { return LibClang.clang_getTranslationUnitSpelling (Handle).Unwrap (); }
		}

		public ClangResourceUsage GetResourceUsage ()
		{
			return new ClangResourceUsage (LibClang.clang_getCXTUResourceUsage (Handle));
		}

		public void Save (string filename, SaveTranslationUnitFlags options)
		{
			var ret = LibClang.clang_saveTranslationUnit (Handle, filename, options);
			if (ret != SaveError.None)
				throw new InvalidOperationException ("Failed to save translation unit: " + ret);
		}

		public void Reparse (ClangUnsavedFile [] unsavedFiles, ReparseTranslationUnitFlags options)
		{
			var cx = unsavedFiles.Select (o => new CXUnsavedFile (o.FileName, o.Contents)).ToArray ();
			var ret = LibClang.clang_reparseTranslationUnit (Handle, (uint) unsavedFiles.Length, cx, options);
			if (ret != ErrorCode.Success)
				throw new InvalidOperationException ("Failed to reparse translation unit: " + ret);
		}

		// FileManipulation

		public ClangFile GetFile (string filename)
		{
			return LibClang.clang_getFile (Handle, filename).Wrap ();
		}


		public bool IsMultipleIncludeGuarded (ClangFile file)
		{
			return LibClang.clang_isFileMultipleIncludeGuarded (Handle, file.Handle) != 0;
		}

		// CursorManipulation
		public ClangCursor GetCursor ()
		{
			return new ClangCursor (LibClang.clang_getTranslationUnitCursor (Handle));
		}

		// CodeCompletion
		public ClangCodeCompleteResults CodeCompleteAt (string completeFilename, int completeLine, int completeColumn, ClangUnsavedFile [] unsavedFiles, CodeCompleteFlags options)
		{
			var cx = unsavedFiles.Select (o => new CXUnsavedFile (o.FileName, o.Contents)).ToArray ();
			return new ClangCodeCompleteResults (LibClang.clang_codeCompleteAt (Handle, completeFilename, (uint) completeLine, (uint) completeColumn, cx, (uint) unsavedFiles.Length, options));
		}

		public ClangSourceLocation GetLocation (ClangFile file, int line, int column)
		{
			return new ClangSourceLocation (LibClang.clang_getLocation (Handle, file.Handle, (uint) line, (uint) column));
		}

		public ClangSourceLocation GetLocationForOffset (ClangFile file, int offset)
		{
			return new ClangSourceLocation (LibClang.clang_getLocationForOffset (Handle, file.Handle, (uint) offset));
		}

		// PhysicalSourceLocations

		int? range_size;

		public ClangSourceRange [] GetSkippedRanges (ClangFile file)
		{
			int size = (int) (range_size ?? (range_size = Marshal.SizeOf (typeof (ClangSourceRange))));
			var ret = LibClang.clang_getSkippedRanges (Handle, file.Handle);
			try {
				var rl = Marshal.PtrToStructure<CXSourceRangeList> (ret);
				return Enumerable.Range (0, (int) rl.Count).Select (i => new ClangSourceRange (Marshal.PtrToStructure<CXSourceRange> (rl.Ranges + size * i))).ToArray ();
			} finally {
				LibClang.clang_disposeSourceRangeList (ret);
			}
		}

		// MappingBetweenCursorAndLocation

		public ClangCursor GetCursor (ClangSourceLocation location)
		{
			return new ClangCursor (LibClang.clang_getCursor (Handle, location.Source));
		}

		// MiscellaneousUtilityFunctions

		static readonly int loc_size = Marshal.SizeOf<CXSourceLocation> ();

		public void GetInclusions (Action<ClangFile,ClangSourceLocation[],IntPtr> visitor, IntPtr clientData)
		{
			CXInclusionVisitor v = (file, locations, len, cd) => visitor (new ClangFile (file), Enumerable.Range (0, (int) len).Select (i => new ClangSourceLocation (Marshal.PtrToStructure<CXSourceLocation> (locations + loc_size * i))).ToArray (), cd);
			LibClang.clang_getInclusions (Handle, v, clientData);
		}

		// ModuleIntrospection
		public int GetTopLevelHeaderCount (ClangModule module)
		{
			return (int) LibClang.clang_Module_getNumTopLevelHeaders (Handle, module.Handle);
		}

		public ClangFile GetTopLevelHeader (ClangModule module, int index)
		{
			return new ClangFile (LibClang.clang_Module_getTopLevelHeader (Handle, module.Handle, (uint) index));
		}

		// DiagnosticReporting

		public int DiagnosticCount {
			get { return (int)LibClang.clang_getNumDiagnostics (Handle); }
		}

		public ClangDiagnostic GetDiagnostic (int index)
		{
			return new ClangDiagnostic (LibClang.clang_getDiagnostic (Handle, (uint) index));
		}

		public ClangDiagnosticSet DiagnosticSet {
			get { return new ClangDiagnosticSet (LibClang.clang_getDiagnosticSetFromTU (Handle)); }
		}

		// TokenExtractionAndManipulation

		public ClangTokenSet Tokenize (ClangSourceRange range)
		{
			IntPtr tokens;
			uint count;
			LibClang.clang_tokenize (Handle, range.Source, out tokens, out count);
			return new ClangTokenSet (Handle, tokens, (int) count);
		}

		public string GetTokenSpelling (ClangToken token)
		{
			return LibClang.clang_getTokenSpelling (Handle, token.Source).Unwrap ();
		}

		public ClangSourceLocation GetTokenLocation (ClangToken token)
		{
			return new ClangSourceLocation (LibClang.clang_getTokenLocation (Handle, token.Source));
		}

		public ClangSourceRange GetTokenExtent (ClangToken token)
		{
			return new ClangSourceRange (LibClang.clang_getTokenExtent (Handle, token.Source));
		}

		// HighLevelAPI

		public FindResult FindIncludesInFile (ClangFile file, Func<object,ClangCursor,ClangSourceRange,VisitorResult> visitor)
		{
			return LibClang.clang_findIncludesInFile (Handle, file.Handle, new ClangCursorAndRangeVisitor (new Object (), (ctx, cursor, range) => visitor (ctx, new ClangCursor (cursor), new ClangSourceRange (range))));
		}
	}
}
