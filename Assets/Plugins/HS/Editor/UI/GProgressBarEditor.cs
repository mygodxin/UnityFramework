using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    [CustomEditor(typeof(GProgressBar)), CanEditMultipleObjects]
    public class GProgressBarEditor : Editor
    {
        protected virtual void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
        [MenuItem("GameObject/UI/GProgressBar")]
        public static void GProgressBarCreator(MenuCommand command)
        {
            var go = new GameObject("GProgressBar");
            go.AddComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(UIEditorUtil.kBackgroundSpriteResourcePath);
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 50);
            var progressbar = go.AddComponent<GProgressBar>();

            var barGO = new GameObject("Bar");
            var barRect = barGO.AddComponent<RectTransform>();
            barGO.transform.SetParent(go.transform, false);
            barRect.SetStretchModel();
            barRect.offsetMin = Vector2.zero;
            barRect.offsetMax = Vector2.zero;
            //设置进度条
            var barImage = barGO.AddComponent<Image>();
            barImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(UIEditorUtil.kStandardSpritePath);
            barImage.type = Image.Type.Filled;
            barImage.fillMethod = Image.FillMethod.Horizontal;
            barImage.fillAmount = (float)(progressbar.Value / progressbar.Max);

            var valueGO = new GameObject("Text");
            var valueRect = valueGO.AddComponent<RectTransform>();
            valueGO.transform.SetParent(go.transform, false);
            valueRect.SetStretchModel();
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            //设置进度条值
            var valueText = valueGO.AddComponent<TextMeshProUGUI>();
            valueText.text = $"{progressbar.Value}/{progressbar.Max}";
            valueText.alignment = TextAlignmentOptions.Center;

            progressbar.BarImage = barImage;
            progressbar.ValueText = valueText;

            UIEditorUtil.PlaceUIElementRoot(go, command);
        }
    }
}