using System;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    public enum GameViewSizeType
    {
        AspectRatio,
        FixedResolution
    }

    [Serializable]
    public sealed class GameViewSize
    {
        [SerializeField] private string m_BaseText;
        [SerializeField] private GameViewSizeType m_SizeType;
        [SerializeField] private int m_Width;
        [SerializeField] private int m_Height;

        public string baseText => m_BaseText;
        public GameViewSizeType sizeType => m_SizeType;
        public int width => m_Width;
        public int height => m_Height;

        public GameViewSize(string baseText, GameViewSizeType sizeType, int width, int height)
        {
            m_BaseText = baseText;
            m_SizeType = sizeType;
            m_Width = width;
            m_Height = height;
        }
    }
}