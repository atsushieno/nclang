using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

namespace NClang
{
	public class ClangDiagnostic : ClangObject, IDisposable
	{
		public struct CommandLineOptions
		{
			public CommandLineOptions (string enable, string disable)
			{
				this.enable = enable;
				this.disable = disable;
			}

			readonly string enable, disable;

			public string Enable {
				get { return enable; }
			}
			public string Disable {
				get { return disable; }
			}
		}

		public struct FixIt
		{
			public FixIt (ClangSourceRange replacementRange, string replatementText)
			{
				replacement_range = replacementRange;
				replatement_text = replatementText;
			}

			readonly ClangSourceRange replacement_range;
			readonly string replatement_text;

			public ClangSourceRange ReplacementRange {
				get { return replacement_range; }
			}

			public string ReplacementText {
				get { return replatement_text; }
			}
		}

		public ClangDiagnostic (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_disposeDiagnostic (Handle);
		}

		public ClangDiagnosticSet ChildDiagnostics {
			get { return new ClangDiagnosticSet (LibClang.clang_getChildDiagnostics (Handle)); }
		}

		public string Format (DiagnosticDisplayOptions options)
		{
			return LibClang.clang_formatDiagnostic (Handle, options).Unwrap ();
		}

		public DiagnosticSeverity Severity {
			get { return LibClang.clang_getDiagnosticSeverity (Handle); }
		}

		public ClangSourceLocation Location {
			get { return new ClangSourceLocation (LibClang.clang_getDiagnosticLocation (Handle)); }
		}

		public string Spelling {
			get { return LibClang.clang_getDiagnosticSpelling (Handle).Unwrap (); }
		}

		public CommandLineOptions Options {
			get {
				IntPtr d = IntPtr.Zero;
				var e = LibClang.clang_getDiagnosticOption (Handle, ref d);
				return new CommandLineOptions (e.Unwrap (), d != IntPtr.Zero ? Marshal.PtrToStructure<ClangString> (d).Unwrap () : null);
			}
		}

		// no corresponding enum for this...
		public uint Category {
			get { return LibClang.clang_getDiagnosticCategory (Handle); }
		}

		public string CategoryText {
			get { return LibClang.clang_getDiagnosticCategoryText (Handle).Unwrap (); }
		}

		public int RangeCount {
			get { return (int) LibClang.clang_getDiagnosticNumRanges (Handle); }
		}

		public ClangSourceRange GetDiagnosticRange (int range)
		{
			return new ClangSourceRange (LibClang.clang_getDiagnosticRange (Handle, (uint) range));
		}

		public int FixItCount {
			get { return (int) LibClang.clang_getDiagnosticNumFixIts (Handle); }
		}

		public FixIt GetFixIt (int index)
		{
			CXSourceRange range;
			var ret = LibClang.clang_getDiagnosticFixIt (Handle, (uint) index, out range).Unwrap ();
			return new FixIt (new ClangSourceRange (range), ret);
		}
	}
}
