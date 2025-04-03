using System.Linq;
using UnityEditor;
using UnityEngine;

enum Align
{
    Left, Right, HCenter, Top, VCenter, Bottom, Horziontal, Vertical, Grid
}
public class AlignTool
{

    [InitializeOnLoadMethod]
    static void Init()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView view)
    {
        if (Application.isPlaying) return;

        Handles.BeginGUI();

        var left = new GUIContent("左", "水平左对齐");
        if (GUILayout.Button(left, GUILayout.Width(25), GUILayout.Height(25)))
        {
            HorziontalAlign(Align.Left);
        }
        var hcenter = new GUIContent("中", "水平居中对齐");
        if (GUILayout.Button(hcenter, GUILayout.Width(25), GUILayout.Height(25)))
        {
            HorziontalAlign(Align.HCenter);
        }
        var right = new GUIContent("右", "水平右对齐");
        if (GUILayout.Button(right, GUILayout.Width(25), GUILayout.Height(25)))
        {
            HorziontalAlign(Align.Right);
        }

        EditorGUILayout.Space(5);

        var top = new GUIContent("顶", "垂直顶对齐");
        if (GUILayout.Button(top, GUILayout.Width(25), GUILayout.Height(25)))
        {
            VerticalAlign(Align.Top);
        }
        var vcenter = new GUIContent("中", "垂直居中对齐");
        if (GUILayout.Button(vcenter, GUILayout.Width(25), GUILayout.Height(25)))
        {
            VerticalAlign(Align.VCenter);
        }
        var bottom = new GUIContent("底", "垂直底对齐");
        if (GUILayout.Button(bottom, GUILayout.Width(25), GUILayout.Height(25)))
        {
            VerticalAlign(Align.Bottom);
        }

        EditorGUILayout.Space(5);

        var width = new GUIContent("宽", "宽对齐");
        if (GUILayout.Button(width, GUILayout.Width(25), GUILayout.Height(25)))
        {
            EqualWidth();
        }
        var height = new GUIContent("高", "高对齐");
        if (GUILayout.Button(height, GUILayout.Width(25), GUILayout.Height(25)))
        {
            EqualHeight();
        }

        EditorGUILayout.Space(5);
        var hor = new GUIContent("行", "水平布局");
        if (GUILayout.Button(hor, GUILayout.Width(25), GUILayout.Height(25)))
        {
            AlignEditorWindow.ShowWindow(Align.Horziontal);
        }
        var ver = new GUIContent("列", "垂直布局");
        if (GUILayout.Button(ver, GUILayout.Width(25), GUILayout.Height(25)))
        {
            AlignEditorWindow.ShowWindow(Align.Vertical);
        }
        var grid = new GUIContent("格", "格子布局");
        if (GUILayout.Button(grid, GUILayout.Width(25), GUILayout.Height(25)))
        {
            AlignEditorWindow.ShowWindow(Align.Grid);
        }

        Handles.EndGUI();
    }

    internal static bool ContainsParent(Transform obj, Transform target)
    {
        if (obj == null) return false; // 防止空引用异常
        if (obj == target) return true; // 如果当前对象就是目标对象，返回true
        return ContainsParent(obj.transform.parent, target); // 否则，递归检查父对象
    }

    internal static void HorziontalAlign(Align align)
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 0) return;

        var firstGO = selects.First();
        RectTransform firstRect = firstGO.GetComponent<RectTransform>();
        var originPosition = firstRect.position;
        if (selects.Length == 1)
        {
            var parentRect = firstRect.parent.GetComponent<RectTransform>();
            var parentX = parentRect.position.x + GetOffset(parentRect, align, true) - GetWidth(parentRect);
            firstGO.transform.position = new Vector2(parentX + GetOffset(firstRect, align, false) + GetWidth(firstRect), firstGO.transform.position.y); // 87.5 75
        }
        else
        {
            float left = firstRect.position.x + GetOffset(firstRect, align, true) - GetWidth(firstRect);
            foreach (GameObject go in Selection.gameObjects)
            {
                var goRect = go.GetComponent<RectTransform>();
                go.transform.position = new Vector2(left + GetOffset(goRect, align, false) + GetWidth(goRect), go.transform.position.y);

                //go为firstGO父节点或者组父节点时位置修正
                if (ContainsParent(firstGO.transform, go.transform))
                {
                    firstGO.transform.position = originPosition;
                }
            }
        }
    }

    internal static float GetWidth(RectTransform rectTransform)
    {
        return rectTransform.rect.width * rectTransform.pivot.x * rectTransform.lossyScale.x;
    }

    internal static float GetHeight(RectTransform rectTransform)
    {
        return rectTransform.rect.height * rectTransform.pivot.y * rectTransform.lossyScale.y;
    }

    internal static void VerticalAlign(Align align)
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 0) return;

        var firstGO = selects.First();
        RectTransform firstRect = firstGO.GetComponent<RectTransform>();
        var originPosition = firstRect.position;
        if (selects.Length == 1)
        {
            var parentRect = firstRect.parent.GetComponent<RectTransform>();
            var parentY = parentRect.position.y + GetOffset(parentRect, align, true) - GetHeight(parentRect);
            firstGO.transform.position = new Vector2(firstGO.transform.position.x, parentY + GetOffset(firstRect, align, false) + GetHeight(firstRect));
        }
        else
        {
            float left = firstRect.position.y + GetOffset(firstRect, align, true) - GetHeight(firstRect);
            foreach (GameObject go in Selection.gameObjects)
            {
                var goRect = go.GetComponent<RectTransform>();
                go.transform.position = new Vector2(go.transform.position.x, left + GetOffset(goRect, align, false) + GetHeight(goRect));

                //go为firstGO父节点或者组父节点时位置修正
                if (ContainsParent(firstGO.transform, go.transform))
                {
                    firstGO.transform.position = originPosition;
                }
            }
        }
    }

    internal static float GetOffset(RectTransform rectTransform, Align align, bool isTarget)
    {
        var scale = rectTransform.lossyScale;
        switch (align)
        {
            case Align.Left:
                return 0;
            case Align.HCenter:
                return isTarget ? rectTransform.rect.width / 2 * scale.x : -rectTransform.rect.width / 2 * scale.x;
            case Align.Right:
                return isTarget ? rectTransform.rect.width * scale.x : -rectTransform.rect.width * scale.x;
            case Align.Top:
                return isTarget ? rectTransform.rect.height * scale.y : -rectTransform.rect.height * scale.y;
            case Align.VCenter:
                return isTarget ? rectTransform.rect.height / 2 * scale.y : -rectTransform.rect.height / 2 * scale.y;
            case Align.Bottom:
                return 0;
            default:
                return 0;
        }
    }

    internal static void EqualWidth()
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 1) return;

        var firstGO = selects.First();
        RectTransform firstRect = firstGO.GetComponent<RectTransform>();

        float targetWidth = firstRect.rect.width * firstRect.lossyScale.x;
        foreach (GameObject go in Selection.gameObjects)
        {
            var goRect = go.GetComponent<RectTransform>();
            goRect.sizeDelta = new Vector2(targetWidth / goRect.lossyScale.x, goRect.sizeDelta.y);
        }
    }

    internal static void EqualHeight()
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 1) return;

        var firstGO = selects.First();
        RectTransform firstRect = firstGO.GetComponent<RectTransform>();

        float targetHeight = firstRect.rect.height * firstRect.lossyScale.y;
        foreach (GameObject go in Selection.gameObjects)
        {
            var goRect = go.GetComponent<RectTransform>();
            goRect.sizeDelta = new Vector2(goRect.sizeDelta.x, targetHeight / goRect.lossyScale.y);
        }
    }
}