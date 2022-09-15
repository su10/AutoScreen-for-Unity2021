using System;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    public sealed class SafeArea : SafeAreaBase
    {
        [Flags]
        public enum Padding
        {
            Top = 1 << 0,
            Bottom = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
        }

        [SerializeField, EnumFlags] private Padding _padding = (Padding)Enum.Parse(typeof(Padding), (-1).ToString());

        public Padding padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        public override void ResetRect()
        {
            base.ResetRect();

            this.rectTransform.anchorMin = Vector2.zero;
            this.rectTransform.anchorMax = Vector2.one;
        }

        public override void UpdateRect(Rect safeArea, int width, int height)
        {
            if ((safeArea.width == width) && (safeArea.height == height))
            {
                this.ResetRect();
                return;
            }

            var paddingTop = 0f;
            var paddingRight = 0f;
            var paddingLeft = 0f;
            var paddingBottom = 0f;

            if (this.padding.HasFlag(Padding.Top)) paddingTop = height - (safeArea.height + safeArea.y);
            if (this.padding.HasFlag(Padding.Right)) paddingRight = width - (safeArea.width + safeArea.x);
            if (this.padding.HasFlag(Padding.Bottom)) paddingBottom = safeArea.y;
            if (this.padding.HasFlag(Padding.Left)) paddingLeft = safeArea.x;

            this.rectTransform.sizeDelta = this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.anchorMin = new Vector2(paddingLeft / width, paddingBottom / height);
            this.rectTransform.anchorMax = new Vector2((width - paddingRight) / width, (height - paddingTop) / height);
        }
    }
}
