using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

namespace NClang
{

	public class ClangResourceUsage : IDisposable
	{
		public class Entry
		{
			internal Entry (CXTUResourceUsageEntry entry)
			{
				this.Kind = entry.Kind;
				this.Amount = entry.Amount;
			}

			public ResourceUsageKind Kind { get; private set; }
			public ulong Amount { get; private set; }
		}

		CXTUResourceUsage source;

		internal ClangResourceUsage (CXTUResourceUsage handle)
		{
			this.source = handle;
		}

		public int Count {
			get { return (int) source.NumEntries; }
		}

		public Entry GetEntry (int i)
		{
			return new Entry (Marshal.PtrToStructure<CXTUResourceUsageEntry> (source.Entries + Marshal.SizeOf<CXTUResourceUsageEntry> () * i));
		}

		public void Dispose ()
		{
			LibClang.clang_disposeCXTUResourceUsage (source);
		}
	}
}
