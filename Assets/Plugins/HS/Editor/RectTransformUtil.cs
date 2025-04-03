using UnityEngine;

namespace HS
{
    public static class RectTransformUtil
    {
        /// <summary>
        /// 将RectTransform的宽高都设置为Stretch模式
        /// </summary>
        /// <param name="rect"></param>
        public static void SetStretchModel(this RectTransform rect)
        {
            rect.anchorMin = Vector3.zero;
            rect.anchorMax = Vector3.one;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        }

        /// <summary>
        /// 设置RectTransform的Left和Bottom(仅当RectTransform的宽高都为Stretch模式时生效)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="left"></param>
        /// <param name="buttom"></param>
        public static void SetLeftAndBottom(this RectTransform rect, float left, float bottom)
        {
            rect.offsetMin = new Vector2(left, bottom);
        }

        /// <summary>
        /// 设置RectTransform的Right和Top(仅当RectTransform的宽高都为Stretch模式时生效,并且是相反的值)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        public static void SetRightAndTop(this RectTransform rect, float right, float top)
        {
            rect.offsetMax = new Vector2(right, top);
        }
    }
}