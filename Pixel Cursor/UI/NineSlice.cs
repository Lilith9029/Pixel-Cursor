using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pixel_Cursor.UI
{
    public class NineSlice
    {
        Texture2D _texture;
        int _coner;

        public NineSlice(Texture2D texture, int coner = 3)
        {
            _texture = texture;
            _coner = coner;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle dest, Color color)
        {
            int c = _coner;
            int tw = _texture.Width;
            int th = _texture.Height;

            var src = new Rectangle[9]
            {
                new(0,      0,      c,       c),       // TL
                new(c,      0,      tw-c*2,  c),       // TC
                new(tw-c,   0,      c,       c),       // TR
                new(0,      c,      c,       th-c*2),  // ML
                new(c,      c,      tw-c*2,  th-c*2),  // MC
                new(tw-c,   c,      c,       th-c*2),  // MR
                new(0,      th-c,   c,       c),       // BL
                new(c,      th-c,   tw-c*2,  c),       // BC
                new(tw-c,   th-c,   c,       c),       // BR
            };

            int dw = dest.Width;
            int dh = dest.Height;
            var dst = new Rectangle[9]
            {
                new(dest.X,          dest.Y,           c,       c),       // TL
                new(dest.X+c,        dest.Y,           dw-c*2,  c),       // TC
                new(dest.Right-c,    dest.Y,           c,       c),       // TR
                new(dest.X,          dest.Y+c,         c,       dh-c*2),  // ML
                new(dest.X+c,        dest.Y+c,         dw-c*2,  dh-c*2),  // MC
                new(dest.Right-c,    dest.Y+c,         c,       dh-c*2),  // MR
                new(dest.X,          dest.Bottom-c,    c,       c),       // BL
                new(dest.X+c,        dest.Bottom-c,    dw-c*2,  c),       // BC
                new(dest.Right-c,    dest.Bottom-c,    c,       c),       // BR
            };

            for (int i = 0; i < 9; i++)
                spriteBatch.Draw(_texture, dst[i], src[i], color);
        }
    }
}
