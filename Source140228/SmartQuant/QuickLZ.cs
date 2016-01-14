using System;
using System.Runtime.InteropServices;
namespace SmartQuant
{
	public class QuickLZ
	{
		private byte[] state_compress;
		private byte[] state_decompress;
		public uint QLZ_COMPRESSION_LEVEL
		{
			get
			{
				return (uint)QuickLZ.qlz_get_setting(0);
			}
		}
		public uint QLZ_SCRATCH_COMPRESS
		{
			get
			{
				return (uint)QuickLZ.qlz_get_setting(1);
			}
		}
		public uint QLZ_SCRATCH_DECOMPRESS
		{
			get
			{
				return (uint)QuickLZ.qlz_get_setting(2);
			}
		}
		public uint QLZ_VERSION_MAJOR
		{
			get
			{
				return (uint)QuickLZ.qlz_get_setting(7);
			}
		}
		public uint QLZ_VERSION_MINOR
		{
			get
			{
				return (uint)QuickLZ.qlz_get_setting(8);
			}
		}
		public int QLZ_VERSION_REVISION
		{
			get
			{
				return QuickLZ.qlz_get_setting(9);
			}
		}
		public uint QLZ_STREAMING_BUFFER
		{
			get
			{
				return (uint)QuickLZ.qlz_get_setting(3);
			}
		}
		public bool QLZ_MEMORY_SAFE
		{
			get
			{
				return QuickLZ.qlz_get_setting(6) == 1;
			}
		}
		[DllImport("quicklz150_64_3.dll")]
		public static extern IntPtr qlz_compress(byte[] source, byte[] destination, IntPtr size, byte[] scratch);
		[DllImport("quicklz150_64_3.dll")]
		public static extern IntPtr qlz_decompress(byte[] source, byte[] destination, byte[] scratch);
		[DllImport("quicklz150_64_3.dll")]
		public static extern IntPtr qlz_size_compressed(byte[] source);
		[DllImport("quicklz150_64_3.dll")]
		public static extern IntPtr qlz_size_decompressed(byte[] source);
		[DllImport("quicklz150_64_3.dll")]
		public static extern int qlz_get_setting(int setting);
		public QuickLZ()
		{
			this.state_compress = new byte[QuickLZ.qlz_get_setting(1)];
			if (this.QLZ_STREAMING_BUFFER == 0u)
			{
				this.state_decompress = this.state_compress;
				return;
			}
			this.state_decompress = new byte[QuickLZ.qlz_get_setting(2)];
		}
		public byte[] Compress(byte[] Source)
		{
			byte[] array = new byte[Source.Length + 400];
			uint num = (uint)((int)QuickLZ.qlz_compress(Source, array, (IntPtr)Source.Length, this.state_compress));
			byte[] array2 = new byte[num];
			Array.Copy(array, array2, (long)((ulong)num));
			return array2;
		}
		public byte[] Decompress(byte[] Source)
		{
			byte[] array = new byte[(int)QuickLZ.qlz_size_decompressed(Source)];
			qlz_decompress(Source, array, state_decompress);
			return array;
		}
		public uint SizeCompressed(byte[] Source)
		{
			return (uint)((int)QuickLZ.qlz_size_compressed(Source));
		}
		public uint SizeDecompressed(byte[] Source)
		{
			return (uint)((int)QuickLZ.qlz_size_decompressed(Source));
		}
	}
}
