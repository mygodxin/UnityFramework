using UnityEngine;

namespace HS
{
    /// <summary>
    /// 数学工具类
    /// </summary>
    public class MathUtil
    {
        /// <summary>
        /// 获取向量的模
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public float Length(Vector2 v1, Vector2 v2)
        {
            return Mathf.Sqrt(Vector2.Dot(v1, v2));
        }
    }
}
