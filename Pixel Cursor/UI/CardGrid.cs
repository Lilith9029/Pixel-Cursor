using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pixel_Cursor.UI
{
    public class CardGrid
    {
        // Canvas
        RenderTarget2D _canvasTarget;

        // Cards
        List<CursorCard> _cards = new();

        //Layout
        const int Cols = 2;
        const int CardW = 87;
        const int CardH = 25;
        const int Gap = 3;
        const int ScrollW = 2;
        const int VisibleH = 81;
        const int ScrollMargin = 3;

        // Scroll
        int _scrollY = 0;

        // Bounds
        Rectangle _bounds;

        // RenderTarget
        RenderTarget2D _clipTarget;
        GraphicsDevice _gd;
        SpriteBatch _sb;

        public CardGrid(GraphicsDevice gd, SpriteBatch sb, Rectangle bounds, RenderTarget2D canvasTarget)
        {
            _gd = gd;
            _sb = sb;
            _bounds = bounds;
            _canvasTarget = canvasTarget;

            _clipTarget = new RenderTarget2D(gd, bounds.Width, bounds.Height);
        }

        public void AddCard(CursorCard card) => _cards.Add(card);

        Vector2 CardLocalPos(int index)
        {
            int col = index % Cols;
            int row = index / Cols;
            int x = Gap + col * (CardW + Gap);
            int y = row * (CardH + Gap) - _scrollY;
            return new Vector2(x, y);
        }

        int TotalHeight => Gap + (_cards.Count / Cols + (_cards.Count % Cols > 0 ? 1 : 0)) * (CardH + Gap) - (Gap * 2);

        Rectangle ScrollTrack => new(
            _bounds.Right - ScrollMargin - ScrollW,
            _bounds.Y,
            ScrollW,
            VisibleH
        );

        Rectangle ScrollHandle
        {
            get
            {
                int total = TotalHeight;
                if (total <= VisibleH) return ScrollTrack;

                int handleH = (int)((float)VisibleH / total * VisibleH);
                handleH = System.Math.Max(handleH, 4);
                int handleY = (int)((float)_scrollY / (total - VisibleH) * (VisibleH - handleH));

                return new Rectangle(
                    ScrollTrack.X,
                    _bounds.Y + handleY,
                    ScrollW,
                    handleH
                );
            }
        }

        int _prevScroll = 0;
        public void Update(MouseState ms, KeyboardState ks, int scale)
        {
            int wheel = ms.ScrollWheelValue - _prevScroll;
            _prevScroll = ms.ScrollWheelValue;
            _scrollY -= wheel / 120 * (CardH + Gap);

            int maxScroll = System.Math.Max(0, TotalHeight - VisibleH);
            _scrollY = System.Math.Clamp(_scrollY, 0, maxScroll);

            for (int i = 0; i < _cards.Count; i++)
            {
                var localPos = CardLocalPos(i);
                _cards[i].Bounds = new Rectangle(
                    (int)localPos.X,
                    (int)localPos.Y,
                    CardW, CardH
                );

                // Chỉ update card đang visible
                if (localPos.Y + CardH > 0 && localPos.Y < VisibleH)
                    _cards[i].Update(ms, scale, _bounds.Location);
            }
        }

        Texture2D _pixel;
        public void Draw(Texture2D pixel)
        {
            _sb.End();

            _gd.SetRenderTarget(_clipTarget);
            _gd.Clear(Color.White);
            _sb.Begin(samplerState: SamplerState.PointClamp);
            for (int i = 0; i < _cards.Count; i++)
            {
                var localPos = CardLocalPos(i);
                if (localPos.Y + CardH > 0 && localPos.Y < VisibleH)
                    _cards[i].Draw(_sb);
            }
            _sb.End();

            _gd.SetRenderTarget(_canvasTarget);
            _gd.Clear(Color.White);
            _sb.Begin(samplerState: SamplerState.PointClamp);
        }

        public void DrawToCanvas(SpriteBatch sb, Texture2D pixel)
        {
            sb.Draw(_clipTarget, _bounds, Color.White);

            sb.Draw(pixel, ScrollTrack, Color.White);

            sb.Draw(pixel, ScrollHandle, Color.Black);
        }
    }
}
