using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace UnityFramework
{
    /// <summary>
    /// –Ú¡–÷°≤•∑≈
    /// </summary>
    public class MovieClip : MonoBehaviour
    {
        public float interval;
        public float timeScale;

        public Image image;
        public SpriteRenderer spriteRenderer;

        public MovieClip()
        {
            interval = 0.1f;
        }
    }
}
