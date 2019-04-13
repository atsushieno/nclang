using System;
using System.Linq;
using System.Runtime.InteropServices;
using NClang.Natives;

using CXIndex = System.IntPtr;
using CXTranslationUnit = System.IntPtr;

namespace NClang
{
	static class Extensions
	{
		public static IntPtr [] ToHGlobalAllocatedArray (this string [] srcArr)
		{
			return srcArr.Select (s => Marshal.StringToHGlobalAnsi (s)).ToArray ();

		}

		public static IntPtr ToHGlobalNativeArray<T> (this T [] srcArr)
		{
			var arr = Marshal.AllocHGlobal (Marshal.SizeOf<T> () * srcArr.Length);
			for (int i = 0; i < srcArr.Length; i++)
				Marshal.StructureToPtr (srcArr [i], arr + i * Marshal.SizeOf<T> (), false);
			return arr;
		}

		public static IntPtr ToHGlobalNativeArray (this IntPtr [] srcArr)
		{
			var arr = Marshal.AllocHGlobal (Marshal.SizeOf<IntPtr> () * srcArr.Length);
			
			for (int i = 0; i < srcArr.Length; i++)
				Marshal.WriteIntPtr (arr + i * Marshal.SizeOf<IntPtr> (), srcArr [i]);
			return arr;
		}
		
		public static ClangType ToManaged (this CXType type)
		{
			return type.kind != CXTypeKind.CXType_Invalid ? new ClangType (type) : null;
		}

		internal static string Unwrap (ClangString s)
		{
			return Unwrap (new CXString () {data = s.Data, private_flags = s.PrivateFlags});
		}

		internal static string Unwrap (this CXString s)
		{
			return Natives.Natives.clang_getCString (s);
			/*
			// Normal marshalling causes double free crash at mono runtime. So I manually process marshaling here.
			IntPtr p = Natives.Natives.clang_getCString (s);
			if (p == IntPtr.Zero)
				return null;
			int x = 0;
			unsafe {
				byte* ptr = (byte*) p;
				while (ptr [x] != 0)
					x++;
				var e = System.Text.Encoding.Default;
				var l = e.GetCharCount (ptr, x);
				if (l == 0)
				{
					return string.Empty;
				}
				char* buf = stackalloc char [l];
				e.GetChars (ptr, x, buf, l);
				return new string (buf, 0, l);
			}
			*/
		}
	}

	namespace Natives
	{
		partial struct CXString
		{

			public static implicit operator CXString (ClangString src)
			{
				return new CXString () {data = src.Data, private_flags = src.PrivateFlags};
			}
		}

		partial struct CXUnsavedFile
		{
			public CXUnsavedFile (string filename, string contents)
			{
				Filename = filename;
				Contents = contents;
				Length = (ulong) contents.Length;
			}
		}
	}
}

