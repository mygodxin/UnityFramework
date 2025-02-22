using HS;
using UnityEngine;

namespace HS
{
    public class RectTransformUtil
    {
        /// <summary>
        /// 本地转世界
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        public static Rect LocalToWorld(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            float width = Mathf.Abs(Vector2.Distance(corners[0], corners[3]));
            float height = Mathf.Abs(Vector2.Distance(corners[0], corners[1]));
            return new Rect(corners[0], new Vector2(width, height));
        }

        public static bool IsClickInArea(RectTransform rect, Vector2 position)
        {
            if (rect != null)
            {
                Rect worldRect = LocalToWorld(rect);
                RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)UIRoot.Inst.Canvas.transform, position, UIRoot.Inst.Camera, out Vector3 worldPosition);
                return worldRect.Contains(worldPosition);
            }
            return false;
        }
    }
}
