using System;
using NClang.Natives;

using CXString = NClang.ClangString;

namespace NClang
{
	public struct ClangSourceLocation
	{
		public abstract class Location
		{
			protected Location (int line, int column)
			{
				Line = line;
				Column = column;
			}

			public int Line { get; private set; }
			public int Column { get; private set; }
		}

		public class PhysicalLocation : Location
		{
			public PhysicalLocation (ClangFile file, int line, int column, int offset)
				: base (line, column)
			{
				File = file;
				Offset = offset;
			}

			public ClangFile File { get; private set; }

			public int Offset { get; private set; }

			public override string ToString ()
			{
				return string.Format ("[PhysicalLocation: File={0} ({1}, {2}) offset={3}]", File, Line, Column, Offset);
			}
		}

		public class IndexFileLocation : PhysicalLocation
		{
			public IndexFileLocation (IntPtr indexFile, ClangFile file, int line, int column, int offset)
				: base (file, line, column, offset)
			{
			}

			public IntPtr IndexFile { get; private set; }
		}

		public class LogicalLocation : Location
		{
			public LogicalLocation (string filename, int line, int column)
				: base (line, column)
			{
				FileName = filename;
			}

			public string FileName { get; private set; }
		}

		readonly CXSourceLocation source;

		internal ClangSourceLocation (CXSourceLocation source)
		{
			this.source = source;
		}

		internal CXSourceLocation Source {
			get { return source; }
		}

		public override bool Equals (object obj)
		{
			return obj is ClangSourceLocation && Equals ((ClangSourceLocation) obj);
		}

		public bool Equals (ClangSourceLocation other)
		{
			return LibClang.clang_equalLocations (source, other.source) != 0;
		}

		public static bool operator == (ClangSourceLocation o1, ClangSourceLocation o2)
		{
			return o1.Equals (o2);
		}

		public static bool operator != (ClangSourceLocation o1, ClangSourceLocation o2)
		{
			return !o1.Equals (o2);
		}

		public override int GetHashCode ()
		{
			return (int) source.IntData;
		}

		public bool IsInSystemHeader {
			get { return LibClang.clang_Location_isInSystemHeader (source) != 0; }
		}

		public bool IsFromMainFile {
			get { return LibClang.clang_Location_isFromMainFile (source) != 0; }
		}

		public PhysicalLocation ExpansionLocation {
			get {
				IntPtr f;
				uint l, c, o;
				LibClang.clang_getExpansionLocation (source, out f, out l, out c, out o);
				return new PhysicalLocation (f.Wrap (), (int) l, (int) c, (int) o);
			}
		}

		public LogicalLocation PresumedLocation {
			get {
				CXString f;
				uint l, c;
				LibClang.clang_getPresumedLocation (source, out f, out l, out c);
				return new LogicalLocation (f.Unwrap (), (int) l, (int) c);
			}
		}

		public PhysicalLocation InstantiationLocation {
			get {
				IntPtr f;
				uint l, c, o;
				LibClang.clang_getInstantiationLocation (source, out f, out l, out c, out o);
				return new PhysicalLocation (f.Wrap (), (int) l, (int) c, (int) o);
			}
		}

		public PhysicalLocation SpellingLocation {
			get {
				IntPtr f;
				uint l, c, o;
				LibClang.clang_getSpellingLocation (source, out f, out l, out c, out o);
				return new PhysicalLocation (f.Wrap (), (int) l, (int) c, (int) o);
			}
		}

		public PhysicalLocation FileLocation {
			get {
				IntPtr f;
				uint l, c, o;
				LibClang.clang_getFileLocation (source, out f, out l, out c, out o);
				return new PhysicalLocation (f.Wrap (), (int) l, (int) c, (int) o);
			}
		}

		public override string ToString ()
		{
			return ExpansionLocation.ToString ();
		}
	}
}
