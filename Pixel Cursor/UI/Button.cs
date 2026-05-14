using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pixel_Cursor.UI
{
    public class Button
    {
        NineSlice _sliceNornal;
        NineSlice _slicePressed;
        BitmapFont _font;
        int _padding;
        int _coner;

        public int BoundsWidth => Bounds.Width;
        public int BoundsHeight => Bounds.Height;

        public Rectangle Bounds { get; set; }
        public string Label { get; set; }

        public bool IsHoverd { get; private set; }
        public bool IsPressed { get; private set; }
        bool _isPressed;

        public event Action OnClick;

        public Button(Texture2D texNormal, Texture2D texPressed, BitmapFont font, Vector2 pos, string label, int padding = 2, int coner = 1)
        {
            _font = font;
            _padding = padding;
            _coner = coner;
            Label = label;

            _sliceNornal = new NineSlice(texNormal, coner);
            _slicePressed = new NineSlice(texPressed, coner);

            var size = font.MeasureString(Label);

            Bounds = new Rectangle(
                (int)pos.X,
                (int)pos.Y,
                (int)size.X + (_padding * 2) + (_coner * 2),
                (int)size.Y + (_padding * 1) + (_coner * 2)
            );
        }

        public void Update(MouseState ms, int scale)
        {
            int mx = ms.X / scale;
            int my = ms.Y / scale;

            IsHoverd = Bounds.Contains(mx, my);
            IsPressed = IsHoverd && ms.LeftButton == ButtonState.Pressed;

            if (IsHoverd && _isPressed && ms.LeftButton == ButtonState.Released)
                OnClick?.Invoke();

            _isPressed = IsPressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var slice = IsPressed ? _slicePressed : _sliceNornal;
            slice.Draw(spriteBatch, Bounds, Color.White);

            if (_font != null && !string.IsNullOrEmpty(Label))
            {
                Vector2 size = _font.MeasureString(Label);

                int textX = Bounds.X + (Bounds.Width - (int)size.X) / 2;
                int textY = Bounds.Y + (Bounds.Height - (int)size.Y) / 2;

                Vector2 textPos = new Vector2(textX, textY);
                Color textColor = IsPressed ? Color.White : Color.Black;

                _font.DrawString(spriteBatch, Label, textPos, textColor);
            }
        }
    }
}
