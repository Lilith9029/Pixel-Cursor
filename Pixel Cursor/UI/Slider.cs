using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pixel_Cursor.UI
{
    public class Slider
    {
        // Texture
        Texture2D _track;
        Texture2D _notch;
        Texture2D _handle;
        Texture2D _pixel;
        NineSlice _labelBox;

        // Font
        BitmapFont _font;

        // Layout
        Vector2 _pos;
        const int TrackW = 46;
        const int TrackH = 4;
        const int NotchW = 2;
        const int NotchH = 4;
        const int HandleW = 4;
        const int HandleH = 6;

        // Snap values
        int[] _steps = { 16, 24, 32, 48, 64 };
        int _currentStep = 2;

        // State
        bool _isDragging = false;
        bool _wasPressed = false;

        // Events
        public event Action<int> OnValueChanged;
        public int Value => _steps[_currentStep];

        public Slider(Texture2D track, Texture2D notch, Texture2D handle,Texture2D pixel,Texture2D boxTex, BitmapFont font, Vector2 pos)
        {
            _track = track;
            _notch = notch;
            _handle = handle;
            _pixel = pixel;
            _labelBox = new NineSlice(boxTex, 1);
            _font = font;
            _pos = pos;
        }

        int NotchX (int index)
        {
            return (int)_pos.X + index * (TrackW - NotchW) / (_steps.Length - 1);
        }

        int HandleX => NotchX(_currentStep) + (NotchW - HandleW) / 2;

        Rectangle HandleRect => new Rectangle(
            HandleX,
            (int)_pos.Y + (TrackH - HandleH) / 2,
            HandleW,
            HandleH
        );

        public void Update(MouseState ms, int scale)
        {
            int mx = ms.X / scale;
            int my = ms.Y / scale;

            bool pressing = ms.LeftButton == ButtonState.Pressed;

            if (pressing && !_wasPressed && HandleRect.Contains(mx, my))
                _isDragging = true;

            if (!pressing)
                _isDragging = false;

            if (_isDragging)
            {
                int closest = 0;
                int closestDist = int.MaxValue;

                for (int i = 0; i < _steps.Length; i++)
                {
                    int dist = Math.Abs(mx - (NotchX(i) + NotchW / 2));
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = i;
                    }
                }

                if (closest != _currentStep)
                {
                    _currentStep = closest;
                    OnValueChanged?.Invoke(Value);
                }
            }

            _wasPressed = pressing;
        }

        public void Draw(SpriteBatch sb)
        {
            _font.DrawString(sb, "SIZE", new Vector2(_pos.X - 17, _pos.Y - 1), Color.Black);

            sb.Draw(_track, new Vector2(_pos.X, _pos.Y), Color.Black);

            for (int i = 0; i < _steps.Length; i++)
                sb.Draw(_notch, new Vector2(NotchX(i), _pos.Y), Color.Black);

            sb.Draw(_pixel, HandleRect, Color.White);
            sb.Draw(_handle, new Rectangle(HandleRect.X, HandleRect.Y, HandleW, HandleH), Color.White);

            var valText = $"{Value} PX";
            var valSize = _font.MeasureString(valText);
            var boxRect = new Rectangle(
                (int)(_pos.X + TrackW + 2),
                (int)(_pos.Y + (TrackH - valSize.Y) / 2) - 2,
                (int)valSize.X + 6,
                (int)valSize.Y + 4
            );

            _labelBox.Draw(sb, boxRect, Color.White);
            _font.DrawString(sb, valText, new Vector2(boxRect.X + 3, boxRect.Y + 2), Color.Black);
        }
    }
}
