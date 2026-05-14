using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pixel_Cursor.UI
{
    public class CursorCard
    {
        // Texture
        NineSlice _cardSlice;
        NineSlice _iconFrameSlice;
        Texture2D _iconTex;
        Texture2D _actionSheet;
        NineSlice _btnNormal;
        NineSlice _btnPressed;

        // Font
        BitmapFont _font;

        // Data
        public string CursorName {  get; set; }
        public Rectangle Bounds { get; set; }

        // Layout constants
        const int Padding = 2;
        const int IconSize = 17;
        const int FrameSize = 19;
        const int BtnFrame = 8;
        const int BtnIcon = 4;
        const int BtnGap = 1;
        const int Border = 1;
        const int cardWidth = 87;
        const int cardHeight = 25;

        // State
        bool[] _sliceNormal = new bool[3];
        bool[] _slicePressed = new bool[3];
        bool[] _btnPressState = new bool[3];

        // Events
        public event Action OnSet;
        public event Action OnImport;
        public event Action OnCustom;

        public CursorCard(Texture2D cardTex, Texture2D iconFrameTex, Texture2D actionSheet, Texture2D btnNormalTex, Texture2D btnPressedTex, Texture2D iconTex, BitmapFont font, Vector2 pos)
        {
            _cardSlice = new NineSlice(cardTex, Border);
            _iconFrameSlice = new NineSlice(iconFrameTex, Border);
            _btnNormal = new NineSlice(btnNormalTex, Border); // BtnFrame
            _btnPressed = new NineSlice(btnPressedTex, Border);
            _actionSheet = actionSheet;
            _iconTex = iconTex;
            _font = font;

            Bounds = new Rectangle((int)pos.X, (int)pos.Y, cardWidth, cardHeight);
        }

        Rectangle IconFrameRect => new(
            Bounds.X + Border + Padding,
            Bounds.Y + Border + Padding,
            FrameSize,
            FrameSize
        );

        Rectangle IconRect => new(
            IconFrameRect.X + (FrameSize - IconSize) / 2,
            IconFrameRect.Y + (FrameSize - IconSize) / 2,
            IconSize,
            IconSize
        );

        Rectangle ActionBtnRect(int index)
        {
            int totalBtnW = BtnFrame * 3 + BtnGap * 2;
            int startX = Bounds.Right - Border - Padding - totalBtnW;
            int btnY = Bounds.Y + Border + (cardHeight - Border * 2 - BtnFrame) / 2; // Bounds.Y + Border + (FrameSize - _font.CharH) / 2 + Padding; || Bounds.Y + Border + Padding

            return new Rectangle(
                startX + index * (BtnFrame + BtnGap),
                btnY,
                BtnFrame,
                BtnFrame
            );
        }

        Rectangle ActionIconSrc(int index) => new Rectangle(index * BtnIcon, 0, BtnIcon, BtnIcon);

        Rectangle ActionIconDst(int index)
        {
            var btnRect = ActionBtnRect(index);
            return new Rectangle(
                btnRect.X + Border + 1,
                btnRect.Y + Border + 1,
                BtnIcon,
                BtnIcon
            );
        }

        Rectangle NameRect
        {
            get
            {
                int x = IconFrameRect.Right + Padding;
                int w = ActionBtnRect(0).X - Padding - x;
                int y = Bounds.Y + Border + (cardHeight - Border * 2 - _font.CharH) / 2;
                return new Rectangle(x, y, w, _font.CharH);
            }
        }

        public void Update(MouseState ms, int Scale, Point offset)
        {
            int mx = ms.X / Scale - offset.X;
            int my = ms.Y / Scale - offset.Y;

            for (int i = 0; i < 3; i++)
            {
                var btn = ActionBtnRect(i);
                bool hover = btn.Contains(mx, my);
                bool press = hover && ms.LeftButton == ButtonState.Pressed;

                if (hover && _slicePressed[i] && ms.LeftButton == ButtonState.Released)
                {
                    if (i == 0) OnSet?.Invoke();
                    if (i == 1) OnImport?.Invoke();
                    if (i == 2) OnCustom?.Invoke();
                }

                _slicePressed[i] = press;
                _btnPressState[i] = press;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            // Card background
            _cardSlice.Draw(sb, Bounds, Color.White);

            // Icon frame
            _iconFrameSlice.Draw(sb, IconFrameRect, Color.White);

            // Icon cursor
            if (_iconTex != null)
                sb.Draw(_iconTex, IconRect, Color.White);

            // Name
            if (_font != null && !string.IsNullOrEmpty(CursorName))
            {
                var namePos = new Vector2(NameRect.X, NameRect.Y);
                _font.DrawString(sb, CursorName, namePos, Color.Black);
            }

            // Action buttons
            for (int i = 0; i < 3; i++)
            {
                var btnRect = ActionBtnRect(i);
                var slice = _btnPressState[i] ? _btnPressed : _btnNormal;
                slice.Draw(sb, btnRect, Color.White);

                // Icon
                sb.Draw(_actionSheet, ActionIconDst(i), ActionIconSrc(i), _btnPressState[i] ? Color.White : Color.Black);

            }
        }
    }
}