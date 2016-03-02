using System;
using System.Runtime.InteropServices;

namespace ImageEffects.Interfaces
{
	[ComImport]
	[Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IMemoryBufferByteAccess
	{
		void GetBuffer(out byte* buffer, out uint capacity);
	}

}
