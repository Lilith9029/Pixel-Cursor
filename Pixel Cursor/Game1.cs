using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Cursor.UI;
using System;

namespace Pixel_Cursor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int Width = 300;
        int Height = 200;

        public static int Scale = 4;

        RenderTarget2D _canvas;

        Button _importBtn;
        Button _defaultBtn;
        BitmapFont _bitmapFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        void ApplyScale(int scale)
        {
            Scale = scale;
            _graphics.PreferredBackBufferWidth = Width * Scale;
            _graphics.PreferredBackBufferHeight = Height * Scale;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ApplyScale(Scale);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _canvas = new RenderTarget2D(GraphicsDevice, Width, Height);

            var texNormal = Content.Load<Texture2D>("Sprites/button_normal");
            var texPressed = Content.Load<Texture2D>("Sprites/button_pressed");
            var fontSheet = Content.Load<Texture2D>("Fonts/4x3Font");

            _bitmapFont = new BitmapFont(
                sheet: fontSheet,
                charW: 4,
                charH: 6,
                cols: 16,
                startChar: ' '
            );
            _bitmapFont.SpaceWidth = 1;
            _bitmapFont.LetterSpacing = 1;


            _importBtn = new Button(
                texNormal, texPressed, _bitmapFont,
                new Vector2(10, 10),
                "IMPORT PACK",
                2,
                1
            );

            _defaultBtn = new Button(
                texNormal, texPressed, _bitmapFont,
                new Vector2(10, 10),
                "DefaultPack",
                1
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var ms = Mouse.GetState();
            _importBtn.Update(ms, Scale);
            _defaultBtn.Update(ms, Scale);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.SetRenderTarget(_canvas);
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            DrawUI();
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(
                _canvas,
                new Rectangle(0, 0, Width * Scale, Height * Scale),
                Color.White
            );
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawUI()
        {
            _importBtn.Draw(_spriteBatch);
            /*_defaultBtn.Draw(_spriteBatch);*/
        }
    }
}