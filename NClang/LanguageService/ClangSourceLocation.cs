using System;
using System.Runtime.InteropServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

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
				return Line > 0 ? string.Format ("{0} ({1}, {2})", File, Line, Column) : string.Format ("{0} (offset: {1})", File, Offset);
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
			return (int) source.int_data;
		}

		public bool IsInSystemHeader {
			get { return LibClang.clang_Location_isInSystemHeader (source) != 0; }
		}

		public bool IsFromMainFile {
			get { return LibClang.clang_Location_isFromMainFile (source) != 0; }
		}

		unsafe T Extract<M,N,T> (CXSourceLocation source, Action<CXSourceLocation, IntPtr, IntPtr, IntPtr, IntPtr> func, Func<IntPtr,M> conv, Func<M,int,int,int,T> gen)
		{
			int f = 0, l = 0, c = 0, o = 0;
			IntPtr fp = new IntPtr (&f),
				lp = new IntPtr (&l),
				cp = new IntPtr (&c),
				op = new IntPtr (&o);
			func (source, fp, lp, cp, op);
			var m = conv (fp);
			return gen (m, Marshal.ReadInt32 (lp), Marshal.ReadInt32 (cp), Marshal.ReadInt32 (op));
		}

		public PhysicalLocation ExpansionLocation {
			get
			{
				IntPtr dummy = IntPtr.Zero;
				return Extract<ClangFile,IntPtr,PhysicalLocation> (source, LibClang.clang_getExpansionLocation, f => f.Wrap (),
					(f, l, c, o) => new PhysicalLocation (f, l, c, o));
			}
		}

		public LogicalLocation PresumedLocation {
			get {
				return Extract<string,CXString,LogicalLocation> (source, (x,y,z,a,b) => LibClang.clang_getPresumedLocation (x, y,z,a), f => Marshal.PtrToStructure<CXString> (Marshal.ReadIntPtr (f)).Unwrap (),
					(f, l, c, o) => new LogicalLocation (f, l, c));
			}
		}

		public PhysicalLocation InstantiationLocation {
			get {
				return Extract<ClangFile,IntPtr,PhysicalLocation> (source, LibClang.clang_getInstantiationLocation, f => f.Wrap (),
					(f, l, c, o) => new PhysicalLocation (f, l, c, o));
			}
		}

		public PhysicalLocation SpellingLocation {
			get {
				return Extract<ClangFile,IntPtr,PhysicalLocation> (source, LibClang.clang_getSpellingLocation, f => f.Wrap (),
					(f, l, c, o) => new PhysicalLocation (f, l, c, o));
			}
		}

		public PhysicalLocation FileLocation {
			get {
				return Extract<ClangFile,IntPtr,PhysicalLocation> (source, LibClang.clang_getFileLocation, f => f.Wrap (),
					(f, l, c, o) => new PhysicalLocation (f, l, c, o));
			}
		}

		public override string ToString ()
		{
			return ExpansionLocation.ToString ();
		}
	}
}
