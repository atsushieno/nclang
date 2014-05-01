using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using CXCompletionString = System.IntPtr;

namespace NClang
{
	
	public class ClangCompletionString
	{
		public struct Chunk
		{
			readonly CXCompletionString source;
			readonly uint index;

			internal Chunk (CXCompletionString source, int chunkNumber)
			{
				this.source = source;
				this.index = (uint) chunkNumber;
			}

			public CompletionChunkKind Kind {
				get { return LibClang.clang_getCompletionChunkKind (source, index); }
			}

			public string Text {
				get { return LibClang.clang_getCompletionChunkText (source, index).Unwrap (); }
			}

			public ClangCompletionString ChunkCompletionString {
				get { return new ClangCompletionString (LibClang.clang_getCompletionChunkCompletionString (source, index)); }
			}
		}

		readonly IntPtr source;

		internal ClangCompletionString (IntPtr source)
		{
			this.source = source;
		}

		// CodeCompletion

		public int ChunkCount {
			get { return (int) LibClang.clang_getNumCompletionChunks (source); }
		}

		public IEnumerable<Chunk> Chunks {
			get { return Enumerable.Range (0, ChunkCount).Select (i => new Chunk (source, i)); }
		}

		public uint Priority {
			get { return LibClang.clang_getCompletionPriority (source); }
		}

		public AvailabilityKind Availability {
			get { return LibClang.clang_getCompletionAvailability (source); }
		}

		public int AnnotationCount {
			get { return (int) LibClang.clang_getCompletionNumAnnotations (source); }
		}

		public string GetAnnotation (int index)
		{
			return LibClang.clang_getCompletionAnnotation (source, (uint) index).Unwrap ();
		}

		public string Parent {
			get { return LibClang.clang_getCompletionParent (source, IntPtr.Zero).Unwrap (); }
		}

		public string BriefComment {
			get { return LibClang.clang_getCompletionBriefComment (source).Unwrap (); }
		}
	}
}
