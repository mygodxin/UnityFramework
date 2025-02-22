using UnityEngine;

namespace HS
{
    /// <summary>
    /// 
    /// </summary>
    public enum TweenPropType
    {
        None,
        X,
        Y,
        Z,
        XY,
        Position,
        Width,
        Height,
        Size,
        ScaleX,
        ScaleY,
        Scale,
        Rotation,
        RotationX,
        RotationY,
        Alpha,
        Progress
    }

    internal class TweenPropTypeUtils
    {
        internal static void SetProps(object target, TweenPropType propType, TweenValue value)
        {
            RectTransform g = target as RectTransform;
            if (g == null)
                return;

            switch (propType)
            {
                case TweenPropType.X:
                    g.localPosition = new Vector3(value.x, g.localPosition.y, g.localPosition.z);
                    break;

                case TweenPropType.Y:
                    g.localPosition = new Vector3(g.localPosition.x, value.x, g.localPosition.z);
                    break;

                case TweenPropType.Z:
                    g.localPosition = new Vector3(g.localPosition.x, g.localPosition.y, value.x);
                    break;

                case TweenPropType.XY:
                    g.localPosition = value.vec2;
                    break;

                case TweenPropType.Position:
                    g.localPosition = value.vec3;
                    break;

                case TweenPropType.Width:
                    g.sizeDelta = new Vector2(value.x, g.sizeDelta.y);
                    break;

                case TweenPropType.Height:
                    g.sizeDelta = new Vector2(g.sizeDelta.x, value.x);
                    break;

                case TweenPropType.Size:
                    g.sizeDelta = value.vec2;
                    break;

                case TweenPropType.ScaleX:
                    g.localScale = new Vector3(value.x, g.localScale.y, g.localScale.z);
                    break;

                case TweenPropType.ScaleY:
                    g.localScale = new Vector3(g.localScale.x, value.x, g.localScale.z);
                    break;

                case TweenPropType.Scale:
                    g.localScale = value.vec2;
                    break;

                case TweenPropType.Rotation:
                    break;

                case TweenPropType.RotationX:
                    break;

                case TweenPropType.RotationY:
                    break;

                case TweenPropType.Alpha:
                    break;

                case TweenPropType.Progress:
                    break;
            }
        }
    }
}
