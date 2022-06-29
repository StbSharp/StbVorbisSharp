using System;

namespace Hebron.Runtime
{
	internal unsafe static class Utility
	{
		public static byte *ToBytePointer(this IntPtr ptr) => (byte*)ptr.ToPointer();
	}
}
