using System;
using UnityEngine;

namespace HS
{
    public class VectorUtil
    {
        /// <summary>
        /// 获取两个点的角度(360度)，180度用Vector2.Angle
        /// </summary>
        public Quaternion GetRotation(Vector2 p1, Vector2 p2)
        {
            var direction = p1 - p2;
            var angleRadians = MathF.Atan2(direction.y, direction.x);
            var angleDegress = angleRadians * Mathf.Rad2Deg;
            angleDegress = (angleDegress + 360f) % 360f;
            return Quaternion.Euler(0f, 0f, angleDegress);
        }
    }
}