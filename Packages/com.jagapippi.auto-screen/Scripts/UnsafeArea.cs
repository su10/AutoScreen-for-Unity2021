using System;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    public sealed class UnsafeArea : SafeAreaBase
    {
        public enum Position
        {
            Top,
            Bottom,
            Left,
            Right,
        }

        [SerializeField] private Position _position;

        public Position position
        {
            get { return _position; }
            set { _position = value; }
        }

        public override void UpdateRect(Rect safeArea, int width, int height)
        {
            if ((safeArea.width == width) && (safeArea.height == height))
            {
                this.ResetRect();
                return;
            }

            var anchorMin = Vector2.zero;
            var anchorMax = Vector2.one;

            switch (this.position)
            {
                case Position.Top:
                    anchorMin = new Vector2(0, safeArea.height + safeArea.y) / height;
                    break;
                case Position.Bottom:
                    anchorMax = new Vector2(1, safeArea.y / height);
                    break;
                case Position.Left:
                    anchorMax = new Vector2(safeArea.x / width, 1);
                    break;
                case Position.Right:
                    anchorMin = new Vector2(safeArea.width + safeArea.x, 0) / width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.rectTransform.anchorMin = anchorMin;
            this.rectTransform.anchorMax = anchorMax;
            this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.sizeDelta = Vector2.zero;
        }
    }
}
