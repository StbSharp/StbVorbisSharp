#define DECODER_TYPE_VORBIS
// #define DECODER_TYPE_VORBIS_NATIVE
// #define DECODER_TYPE_NVORBIS

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StbVorbisSharp.MonoGame.Test
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;

		private SoundEffect soundEffect;
		private SoundEffectInstance _instance;
		private FontSystem _fontSystem;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1400,
				PreferredBackBufferHeight = 960
			};

			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = true;
		}
		
		private void LoadSong()
		{
			var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			path = Path.Combine(path, "music.ogg");
			var buffer = File.ReadAllBytes(path);

#if DECODER_TYPE_VORBIS || DECODER_TYPE_VORBIS_NATIVE
			int sampleRate, channels;

#if DECODER_TYPE_VORBIS
			var audioShort = StbVorbis.decode_vorbis_from_memory(buffer, out sampleRate, out channels);
#else
			var audioShort = StbNative.Vorbis.decode_vorbis_from_memory(buffer, out sampleRate, out channels);
#endif

			byte[] audioData = new byte[audioShort.Length * 2];
			for (var i = 0; i < audioShort.Length; ++i)
			{
				if (i * 2 >= audioData.Length)
				{
					break;
				}

				var b1 = (byte)(audioShort[i] >> 8);
				var b2 = (byte)(audioShort[i] & 256);

				audioData[i * 2 + 0] = b2;
				audioData[i * 2 + 1] = b1;
			}
#elif DECODER_TYPE_NVORBIS
			int sampleRate, channels;
			var allSamples = new List<float>();
			using (var vorbis = new NVorbis.VorbisReader("music.ogg"))
			{
				// get the channels & sample rate
				channels = vorbis.Channels;
				sampleRate = vorbis.SampleRate;

				// OPTIONALLY: get a TimeSpan indicating the total length of the Vorbis stream
				var totalTime = vorbis.TotalTime;

				// create a buffer for reading samples
				var readBuffer = new float[channels * sampleRate / 5];  // 200ms

				// get the initial position (obviously the start)
				var position = TimeSpan.Zero;

				// go grab samples
				int cnt;
				while ((cnt = vorbis.ReadSamples(readBuffer, 0, readBuffer.Length)) > 0)
				{
					for(var i = 0; i < cnt; ++i)
					{
						allSamples.Add(readBuffer[i]);
					}
				}
			}

			var audioData = new byte[allSamples.Count * channels];
			for (var i = 0; i < allSamples.Count; ++i)
			{
				var temp = (int)(32767f * allSamples[i]);

				if (temp > short.MaxValue) temp = short.MaxValue;
				else if (temp < short.MinValue) temp = short.MinValue;

				audioData[i * 2 + 0] = (byte)(temp & 256);
				audioData[i * 2 + 1] = (byte)(temp >> 8);
			}
			#endif

			soundEffect = new SoundEffect(audioData, sampleRate, (AudioChannels)channels);
			_instance = soundEffect.CreateInstance();
			_instance.Volume = 0.5f;
			_instance.Play();

/*			string songFileName = @"music.ogg";
			var uri = new Uri(songFileName, UriKind.Relative);
			var song = Song.FromUri("SongName", uri);
			MediaPlayer.Play(song);*/
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_fontSystem = new FontSystem();
			_fontSystem.AddFont(File.ReadAllBytes(@"DroidSans.ttf"));

			// TODO: use this.Content to load your game content here
			LoadSong();

			GC.Collect();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			var font = _fontSystem.GetFont(32);
			_spriteBatch.Begin();

			_spriteBatch.DrawString(font, $"Allocations: {StbVorbis.NativeAllocations}", Vector2.Zero, Color.White);

			_spriteBatch.End();
			
			base.Draw(gameTime);
		}
	}
}