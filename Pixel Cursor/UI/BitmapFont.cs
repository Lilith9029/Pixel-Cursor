using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pixel_Cursor.UI
{
    public class BitmapFont
    {
        Texture2D _sheet;

        int _charW;
        int _charH;
        int _cols;
        char _startChar;

        public int CharW => _charW;
        public int CharH => _charH;

        public int SpaceWidth { get; set; } = 1;
        public int LetterSpacing { get; set; } = 1;

        public BitmapFont(Texture2D sheet, int charW, int charH, int cols, char startChar = ' ')
        {
            _sheet = sheet;
            _charW = charW;
            _charH = charH;
            _cols = cols;
            _startChar = startChar;
        }

        Rectangle GetSourceRect(char c)
        {
            int index = c - _startChar;
            int col = index % _cols;
            int row = index / _cols;
            return new Rectangle(col * _charW, row * _charH, _charW, _charH);
        }

        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 pos, Color color)
        {
            float x = pos.X;

            foreach (char c in text)
            {
                if (c == ' ') { x += SpaceWidth; continue; }
                var src = GetSourceRect(c);
                var dst = new Rectangle((int)x, (int)pos.Y, _charW, _charH);
                spriteBatch.Draw(_sheet, dst, src, color);
                x += (_charW - 1) + LetterSpacing;
            }
        }

        public Vector2 MeasureString(string text)
        {
            if (string.IsNullOrEmpty(text)) return Vector2.Zero;

            int width = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                    width += SpaceWidth;
                else
                    width += (i == text.Length - 1) ? (_charW - 1) : ((_charW - 1) + LetterSpacing);
            }

            return new Vector2(width, _charH);
        }
    }
}
