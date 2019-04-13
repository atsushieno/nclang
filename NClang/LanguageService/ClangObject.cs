using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangObject
	{
		internal ClangObject (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			this.Handle = handle;
		}
		
		public IntPtr Handle { get; set; }
	}

	class NativeArrayHolder : IDisposable
	{
		IntPtr [] native_list;
		IntPtr native_array;
		public NativeArrayHolder (IntPtr [] array)
		{
			native_list = array;
			native_array = native_list.ToHGlobalNativeArray ();
		}

		public IntPtr [] NativeList => native_list;
		public IntPtr NativeArray => native_array;

		public void Dispose ()
		{
			Marshal.FreeHGlobal (native_array);
			foreach (var ptr in native_list)
				Marshal.FreeHGlobal (ptr);
		}
	}

	public class ClangDisposable : ClangObject, IDisposable
	{
		public ClangDisposable (IntPtr handle)
			: base (handle)
		{
		}

		List<IntPtr> to_be_freed_ptr = new List<IntPtr> ();
		List<IDisposable> to_be_freed_disposables = new List<IDisposable> ();

		public void AddToFreeList (IntPtr ptr)
		{
			to_be_freed_ptr.Add (ptr);
		}

		public void AddToFreeList (IDisposable d)
		{
			to_be_freed_disposables.Add (d);
		}

		public virtual void Dispose ()
		{
			foreach (var ptr in to_be_freed_ptr)
				Marshal.FreeHGlobal (ptr);
			to_be_freed_ptr.Clear ();
			foreach (var d in to_be_freed_disposables)
				d.Dispose ();
			to_be_freed_disposables.Clear ();
		}
	}
}
