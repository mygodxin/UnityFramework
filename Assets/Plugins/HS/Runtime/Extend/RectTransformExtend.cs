using System.Threading.Tasks;
using UnityEngine;

namespace HS
{
    public static class RectTransformExtensions
    {
        public static Vector2 GetPosition(this RectTransform rectTran)
        {
            return rectTran.anchoredPosition;
        }

        public static void SetPosition(this RectTransform rectTran, Vector2 pos)
        {
            rectTran.anchoredPosition = pos;
        }

        public static float GetX(this RectTransform rectTran)
        {
            return rectTran.anchoredPosition.x;
        }

        public static void SetX(this RectTransform rectTran, float x)
        {
            rectTran.anchoredPosition = new Vector2(x, rectTran.anchoredPosition.y);
        }

        public static float GetY(this RectTransform rectTran)
        {
            return rectTran.anchoredPosition.y;
        }

        public static void SetY(this RectTransform rectTran, float y)
        {
            rectTran.anchoredPosition = new Vector2(rectTran.anchoredPosition.x, y);
        }

        public static Vector2 GetSize(this RectTransform rectTran)
        {
            return rectTran.sizeDelta;
        }

        public static void SetSize(this RectTransform rectTran, Vector2 size)
        {
            rectTran.sizeDelta = size;
        }

        public static float GetWidth(this RectTransform rectTran)
        {
            return rectTran.rect.width;
        }

        public static void SetWidth(this RectTransform rectTran, float w)
        {
            rectTran.sizeDelta = new Vector2(w, rectTran.sizeDelta.y);
        }

        public static float GetHeight(this RectTransform rectTran)
        {
            return rectTran.rect.height;
        }

        public static void SetHeight(this RectTransform rectTran, float height)
        {
            rectTran.sizeDelta = new Vector2(rectTran.sizeDelta.x, height);
        }

        public static float GetAlpha(this RectTransform rectTran)
        {
            return rectTran.GetComponent<CanvasRenderer>().GetAlpha();
        }

        public static void SetAlpha(this RectTransform rectTran, float alpha)
        {
            rectTran.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        }
        public static bool IsRectInside(this RectTransform rectTransform1, RectTransform rectTransform2)
        {
            Rect rect1 = rectTransform1.rect;
            Rect rect2 = rectTransform2.rect;

            Vector3[] corners1 = new Vector3[4];
            Vector3[] corners2 = new Vector3[4];

            rectTransform1.GetWorldCorners(corners1);
            rectTransform2.GetWorldCorners(corners2);

            return (corners1[2].x > corners2[0].x && corners1[0].x < corners2[2].x && corners1[2].y > corners2[0].y && corners1[0].y < corners2[2].y);
        }
    }
}
