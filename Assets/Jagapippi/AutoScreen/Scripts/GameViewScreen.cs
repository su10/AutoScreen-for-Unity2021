using System;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    [Serializable]
    public sealed class GameViewScreen : IEquatable<GameViewScreen>
    {
        [SerializeField] private Sprite _frame = null;
        [SerializeField] private string _baseText = "";
        [SerializeField] private Vector2 _size = Vector2.zero;
        [SerializeField] private Rect _safeArea = Rect.zero;

        public Sprite frame => _frame;
        public string baseText => _baseText;
        public Vector2 size => _size;
        public Rect safeArea => _safeArea;

        #region IEquatable

        public bool Equals(GameViewScreen other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_frame, other._frame) && string.Equals(_baseText, other._baseText) && _size.Equals(other._size) && _safeArea.Equals(other._safeArea);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameViewScreen) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_frame != null ? _frame.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_baseText != null ? _baseText.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _size.GetHashCode();
                hashCode = (hashCode * 397) ^ _safeArea.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}