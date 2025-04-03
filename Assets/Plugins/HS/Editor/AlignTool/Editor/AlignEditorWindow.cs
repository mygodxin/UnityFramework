using System.Linq;
using UnityEditor;
using UnityEngine;

internal class AlignEditorWindow : EditorWindow
{
    private static float HorziontatlSpacing;
    private static float VerticalSpacing;
    private static int ColumnCount;
    private static Align Align;
    public static void ShowWindow(Align align)
    {
        var window = GetWindow<AlignEditorWindow>("布局");
        window.minSize = new Vector2(150, 100);
        window.maxSize = new Vector2(150, 100);

        HorziontatlSpacing = 0;
        VerticalSpacing = 0;
        Align = align;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("水平间距");
        HorziontatlSpacing = float.Parse(GUILayout.TextField(HorziontatlSpacing + ""));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("垂直间距");
        VerticalSpacing = float.Parse(GUILayout.TextField(VerticalSpacing + ""));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("列数");
        ColumnCount = int.Parse(GUILayout.TextField(ColumnCount + ""));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("排列"))
        {
            SetLayout();
        }
    }

    private void SetLayout()
    {
        switch (Align)
        {
            case Align.Horziontal:
                Horziontal();
                break;
            case Align.Vertical:
                Vertical();
                break;
            case Align.Grid:
                Grid();
                break;
        }
    }

    private void Horziontal()
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 1) return;

        if (selects == null || selects.Length == 0)
            return;
        var rectTransforms = new RectTransform[selects.Length];
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i] = selects[i].GetComponent<RectTransform>();
        }
        if (rectTransforms == null || rectTransforms.Length == 0)
            return;

        float baseXPosition = rectTransforms[0].position.x - AlignTool.GetWidth(rectTransforms[0]);
        float currentXPosition = baseXPosition;

        for (int i = 1; i < rectTransforms.Length; i++)
        {
            var rectTransform = rectTransforms[i];
            if (rectTransform == null)
                continue;

            float previousWidth = rectTransforms[i - 1].rect.width * rectTransforms[i - 1].lossyScale.x;
            currentXPosition += previousWidth + HorziontatlSpacing;

            float newXPosition = currentXPosition + AlignTool.GetWidth(rectTransform);
            Vector3 newPosition = new Vector3(newXPosition, rectTransform.position.y);
            rectTransform.position = newPosition;
        }
    }

    private void Vertical()
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 1) return;

        if (selects == null || selects.Length == 0)
            return;
        var rectTransforms = new RectTransform[selects.Length];
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i] = selects[i].GetComponent<RectTransform>();
        }
        if (rectTransforms == null || rectTransforms.Length == 0)
            return;

        float baseYPosition = rectTransforms[0].position.y + AlignTool.GetHeight(rectTransforms[0]);
        float currentYPosition = baseYPosition;

        for (int i = 1; i < rectTransforms.Length; i++)
        {
            var rectTransform = rectTransforms[i];
            if (rectTransform == null)
                continue;

            float previousHeight = rectTransforms[i - 1].rect.height * rectTransforms[i - 1].lossyScale.y;
            currentYPosition -= previousHeight + VerticalSpacing;

            float newYPosition = currentYPosition - AlignTool.GetHeight(rectTransform);
            Vector3 newPosition = new Vector3(rectTransform.position.x, newYPosition, rectTransform.position.z);
            rectTransform.position = newPosition;
        }
    }

    private void Grid()
    {
        var selects = Selection.gameObjects;
        if (selects.Length <= 1) return;

        if (selects == null || selects.Length == 0)
            return;
        var rectTransforms = new RectTransform[selects.Length];
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i] = selects[i].GetComponent<RectTransform>();
        }
        if (rectTransforms == null || rectTransforms.Length == 0 || ColumnCount <= 0)
            return;

        float startX = rectTransforms[0].position.x - AlignTool.GetWidth(rectTransforms[0]);
        float startY = rectTransforms[0].position.y + AlignTool.GetHeight(rectTransforms[0]);

        for (int row = 0; row < Mathf.CeilToInt((float)rectTransforms.Length / ColumnCount); row++)
        {
            float maxRowHeight = 0; // ��ǰ�е����߶�
            var currentXPosition = startX;
            for (int col = 0; col < ColumnCount; col++)
            {
                int index = row * ColumnCount + col;
                if (index >= rectTransforms.Length)
                    break;

                var rectTransform = rectTransforms[index];
                if (rectTransform == null)
                    continue;

                float elementWidth = rectTransform.rect.width * rectTransform.lossyScale.x;
                float elementHeight = rectTransform.rect.height * rectTransform.lossyScale.y;

                if (col != 0)
                {
                    float previousWidth = rectTransforms[index - 1].rect.width * rectTransforms[index - 1].lossyScale.x;
                    currentXPosition += previousWidth + HorziontatlSpacing;
                }
                float newX = currentXPosition + AlignTool.GetWidth(rectTransform);
                float newY = startY - (row * VerticalSpacing) - (1 - rectTransform.pivot.y) * elementHeight;
                rectTransform.position = new Vector3(newX, newY, rectTransform.position.z);
                maxRowHeight = Mathf.Max(maxRowHeight, elementHeight);
            }

            startY -= maxRowHeight + VerticalSpacing;
        }
    }
}