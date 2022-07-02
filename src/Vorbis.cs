using System;
using static StbVorbisSharp.StbVorbis;

namespace StbVorbisSharp
{
	public unsafe class Vorbis : IDisposable
	{
		private readonly byte[] _data;
		private readonly stb_vorbis_info _vorbisInfo;

		public stb_vorbis StbVorbis { get; private set; }

		public stb_vorbis_info StbVorbisInfo => _vorbisInfo;

		public int SampleRate => (int) _vorbisInfo.sample_rate;

		public int Channels => _vorbisInfo.channels;

		public float LengthInSeconds { get; }

		public short[] SongBuffer { get; }

		public int Decoded { get; private set; }

		private Vorbis(byte[] data)
		{
			if (data == null) throw new ArgumentNullException("data");

			_data = data;

			stb_vorbis vorbis;
			fixed (byte* b = data)
			{
				vorbis = stb_vorbis_open_memory(b, data.Length, null);
			}

			StbVorbis = vorbis;
			_vorbisInfo = stb_vorbis_get_info(vorbis);
			LengthInSeconds = stb_vorbis_stream_length_in_seconds(StbVorbis);

			SongBuffer = new short[_vorbisInfo.sample_rate];

			Restart();
		}

		public void Dispose()
		{
			if (StbVorbis != null)
			{
				vorbis_deinit(StbVorbis);
				StbVorbis = null;
			}
		}

		~Vorbis()
		{
			Dispose();
		}

		public void Restart()
		{
			stb_vorbis_seek_start(StbVorbis);
		}

		public void SubmitBuffer()
		{
			fixed (short* ptr = SongBuffer)
			{
				Decoded = stb_vorbis_get_samples_short_interleaved(StbVorbis, _vorbisInfo.channels, ptr,
					(int) _vorbisInfo.sample_rate);
			}
		}

		public static Vorbis FromMemory(byte[] data) => new Vorbis(data);
	}
}