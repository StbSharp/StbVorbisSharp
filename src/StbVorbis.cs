using Hebron.Runtime;
using System;
using System.Runtime.InteropServices;

namespace StbVorbisSharp
{
	public static unsafe partial class StbVorbis
	{
		public static int NativeAllocations => MemoryStats.Allocations;

		public class Residue
		{
			public uint begin;
			public uint end;
			public uint part_size;
			public byte classifications;
			public byte classbook;
			public byte** classdata;
			public short[,] residue_books;
		}

		public class stb_vorbis
		{
			public uint sample_rate;
			public int channels;
			public sbyte* vendor;
			public int comment_list_length;
			public sbyte** comment_list;
			public byte* stream;
			public byte* stream_start;
			public byte* stream_end;
			public uint stream_len;
			public byte push_mode;
			public uint first_audio_page_offset;
			public ProbedPage p_first;
			public ProbedPage p_last;
			public int setup_offset;
			public int temp_offset;
			public int eof;

			public STBVorbisError error;
			public int[] blocksize = new int[2];
			public int blocksize_0;
			public int blocksize_1;
			public int codebook_count;
			public Codebook* codebooks;
			public int floor_count;
			public ushort[] floor_types = new ushort[64];

			public Floor* floor_config;
			public int residue_count;
			public ushort[] residue_types = new ushort[64];
			public Residue[] residue_config;
			public int mapping_count;
			public Mapping* mapping;
			public int mode_count;
			public Mode[] mode_config = new Mode[64];
			public uint total_samples;
			public float*[] channel_buffers = new float*[16];
			public float*[] outputs = new float*[16];
			public float*[] previous_window = new float*[16];
			public int previous_length;
			public short*[] finalY = new short*[16];
			public uint current_loc;
			public int current_loc_valid;
			public float*[] A = new float*[2];
			public float*[] B = new float*[2];
			public float*[] C = new float*[2];
			public float*[] window = new float*[2];
			public ushort*[] bit_reverse = new ushort*[2];
			public uint serial;
			public int last_page;
			public int segment_count;
			public byte[] segments = new byte[255];
			public byte page_flag;
			public byte bytes_in_seg;
			public byte first_decode;
			public int next_seg;
			public int last_seg;
			public int last_seg_which;
			public uint acc;
			public int valid_bits;
			public int packet_bytes;
			public int end_seg_with_known_loc;
			public uint known_loc_for_packet;
			public int discard_samples_deferred;
			public uint samples_output;
			public int page_crc_tests;
			public CRCscan[] scan = new CRCscan[4];
			public int channel_buffer_start;
			public int channel_buffer_end;

			internal ArrayBuffer<float> FloatBuffer = new ArrayBuffer<float>(1024);
			internal ArrayBuffer2D<IntPtr> PtrBuffer2D = new ArrayBuffer2D<IntPtr>(8, 256);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Floor1
		{
			public byte partitions;
			public fixed byte partition_class_list[32];
			public fixed byte class_dimensions[16];
			public fixed byte class_subclasses[16];
			public fixed byte class_masterbooks[16];
			public fixed short subclass_books[16 * 8];
			public fixed ushort Xlist[31 * 8 + 2];
			public fixed byte sorted_order[31 * 8 + 2];
			public fixed byte neighbors[(31 * 8 + 2) * 2];
			public byte floor1_multiplier;
			public byte rangebits;
			public int values;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct Floor
		{
			[FieldOffset(0)]
			public Floor0 floor0;

			[FieldOffset(0)]
			public Floor1 floor1;
		}

		public static sbyte[,] channel_position =
		{
			{0, 0, 0, 0, 0, 0},
			{2 | 4 | 1, 0, 0, 0, 0, 0},
			{2 | 1, 4 | 1, 0, 0, 0, 0},
			{2 | 1, 2 | 4 | 1, 4 | 1, 0, 0, 0},
			{2 | 1, 4 | 1, 2 | 1, 4 | 1, 0, 0},
			{2 | 1, 2 | 4 | 1, 4 | 1, 2 | 1, 4 | 1, 0},
			{2 | 1, 2 | 4 | 1, 4 | 1, 2 | 1, 4 | 1, 2 | 4 | 1}
		};

		public static short[] decode_vorbis_from_memory(byte[] input, out int sampleRate, out int chan)
		{
			short* result = null;
			int length = 0;
			fixed (byte* b = input)
			{
				int c, s;
				length = stb_vorbis_decode_memory(b, input.Length, &c, &s, ref result);

				chan = c;
				sampleRate = s;
			}

			var output = new short[length];
			Marshal.Copy(new IntPtr(result), output, 0, output.Length);
			CRuntime.free(result);

			return output;
		}
	}
}