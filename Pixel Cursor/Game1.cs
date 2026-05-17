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

        int Width = 193;
        int Height = 112;

        public static int Scale = 5;

        RenderTarget2D _canvas;

        Button _importBtn;
        Button _defaultBtn;
        BitmapFont _bitmapFont;
        CursorCard _card;
        CardGrid _cardGrid;
        Texture2D _pixel;
        Slider _slider;

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
            var cardTex = Content.Load<Texture2D>("Sprites/button_normal");
            var iconFrameTex = Content.Load<Texture2D>("Sprites/button_normal");
            var actionSheet = Content.Load<Texture2D>("Sprites/action_icons");
            var btnNormal = Content.Load<Texture2D>("Sprites/button_normal");
            var btnPressed = Content.Load<Texture2D>("Sprites/button_pressed");
            var placeholder = Content.Load<Texture2D>("Sprites/cursor_placeholder");
            var sliderTrack = Content.Load<Texture2D>("Sprites/slider_track");
            var sliderNotch = Content.Load<Texture2D>("Sprites/slider_notch");
            var sliderHandle = Content.Load<Texture2D>("Sprites/slider_handle");

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            string[] names = {
                "ARROW","HAND","TEXT","MOVE","CROSS",
                "WAIT","APP START","NO","HELP","SIZE NS",
                "SIZE WE","SIZE NWSE","SIZE NESW",
                "PEN","PIN","PERSON","UP ARROW"
            };

            _bitmapFont = new BitmapFont(
                sheet: fontSheet,
                charW: 4,
                charH: 6,
                cols: 16,
                startChar: ' '
            );
            _bitmapFont.SpaceWidth = 1;
            _bitmapFont.LetterSpacing = 1;

            var gridBounds = new Rectangle(5, 15, 188, 81);
            _cardGrid = new CardGrid(GraphicsDevice, _spriteBatch, gridBounds, _canvas);

            foreach (var name in names)
            {
                var card = new CursorCard(
                    cardTex, iconFrameTex, actionSheet,
                    btnNormal, btnPressed, placeholder,
                    _bitmapFont, Vector2.Zero

                );
                card.CursorName = name;
                _cardGrid.AddCard(card);
            }

            int bottomY = Height - _bitmapFont.CharH - 7;
            int margin = 8;

            _importBtn = new Button(
                texNormal, texPressed, _bitmapFont,
                new Vector2(margin, bottomY),
                "IMPORT PACK",
                2, 1
            );
            _importBtn.OnClick += () => { Console.WriteLine("Import Pack"); };

            int defaultX = Width - margin - GetButtonWidth("DEFAULT", 2, 1);
            _defaultBtn = new Button(
                texNormal, texPressed, _bitmapFont,
                new Vector2(defaultX, bottomY),
                "DEFAULT",
                2, 1
            );
            _defaultBtn.OnClick += () => { Console.WriteLine("Default"); };

            _card = new CursorCard(
                cardTex, iconFrameTex, actionSheet,
                btnNormal, btnPressed, placeholder,
                _bitmapFont,
                new Vector2(10, 10)
            );
            _card.CursorName = "SIZE NESW";
            _card.OnSet += () => Console.WriteLine("Set clicked!");
            _card.OnImport += () => Console.WriteLine("Import clicked!");
            _card.OnCustom += () => Console.WriteLine("Custom clicked!");

            _slider = new Slider(
                sliderTrack, sliderNotch, sliderHandle,
                _pixel,
                texNormal,
                _bitmapFont,
                new Vector2(115, 5)
            );
            _slider.OnValueChanged += (val) => Console.WriteLine($"Slider value: {val}px");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var ms = Mouse.GetState();
            _importBtn.Update(ms, Scale);
            _defaultBtn.Update(ms, Scale);
            _cardGrid.Update(ms, Keyboard.GetState(), Scale);
            _slider.Update(ms, Scale);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
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
            _cardGrid.Draw(_pixel);
            _cardGrid.DrawToCanvas(_spriteBatch, _pixel);

            _importBtn.Draw(_spriteBatch);
            _defaultBtn.Draw(_spriteBatch);
            _slider.Draw(_spriteBatch);
        }

        int GetButtonWidth(string label, int padding, int corner)
        {
            var size = _bitmapFont.MeasureString(label);
            return (int)size.X + (padding * 2) + (corner * 2);
        }
    }
}