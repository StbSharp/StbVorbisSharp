using System;
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

		private Vorbis _vorbis;
		private DynamicSoundEffectInstance _effect;
		private bool _startedPlaying;
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
		
		private void SubmitBuffer()
		{
			_vorbis.SubmitBuffer();

			if (_vorbis.Decoded == 0)
			{
				// Restart
				_vorbis.Restart();
				_vorbis.SubmitBuffer();
			}

			var audioShort = _vorbis.SongBuffer;
			byte[] audioData = new byte[_vorbis.Decoded * _vorbis.Channels * 2];
			for (var i = 0; i < _vorbis.Decoded * _vorbis.Channels; ++i)
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

			_effect.SubmitBuffer(audioData);
		}

		private void LoadSong()
		{
			var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			path = Path.Combine(path, "music.ogg");
			var buffer = File.ReadAllBytes(path);

			_vorbis = Vorbis.FromMemory(buffer);

			_effect = new DynamicSoundEffectInstance(_vorbis.SampleRate, (AudioChannels)_vorbis.Channels)
			{
				Volume = 0.5f
			};

			_effect.BufferNeeded += (s, a) => SubmitBuffer();

			SubmitBuffer();
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
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
			_vorbis.Dispose();
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
			if (!_startedPlaying)
			{
				_effect.Play();
				_startedPlaying = true;
			}

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