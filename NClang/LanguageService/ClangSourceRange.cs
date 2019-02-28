using System;
using System.Runtime.CompilerServices;
using NClang.Natives;

namespace NClang
{
	public struct ClangSourceRange
	{
		public static ClangSourceRange GetRange (ClangSourceLocation begin, ClangSourceLocation end)
		{
			return new ClangSourceRange (LibClang.clang_getRange (begin.Source, end.Source));
		}
		
		readonly CXSourceRange source;

		internal ClangSourceRange (CXSourceRange source)
		{
			this.source = source;
		}

		public ClangSourceRange (int begin, int end)
		{
			this.source = new CXSourceRange (begin, end);
		}

		internal CXSourceRange Source {
			get { return source; }
		}

		public override bool Equals (object obj)
		{
			return obj is ClangSourceRange && Equals ((ClangSourceRange) obj);
		}

		public bool Equals (ClangSourceRange other)
		{
			return LibClang.clang_equalRanges (source, other.source) != 0;
		}

		public static bool operator == (ClangSourceRange o1, ClangSourceRange o2)
		{
			return o1.Equals (o2);
		}

		public static bool operator != (ClangSourceRange o1, ClangSourceRange o2)
		{
			return !o1.Equals (o2);
		}

		public override int GetHashCode ()
		{
			return (int) ((source.BeginIntData << 16) + source.EndIntData);
		}

		public bool IsNull {
			get { return LibClang.clang_Range_isNull (source) != 0; }
		}

		public ClangSourceLocation Start {
			get { return new ClangSourceLocation (LibClang.clang_getRangeStart (source)); }
		}

		public ClangSourceLocation End {
			get { return new ClangSourceLocation (LibClang.clang_getRangeEnd (source)); }
		}

		public override string ToString ()
		{
			return IsNull ? "[Null]" : string.Format ("[{0} - {1}]", Start, End);
		}
	}	
}
