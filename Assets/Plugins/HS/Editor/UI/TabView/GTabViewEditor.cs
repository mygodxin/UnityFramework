using UnityEditor;
using UnityEngine;

namespace HS
{
    [CustomEditor(typeof(GTabView)), CanEditMultipleObjects]
    public class GTabViewEditor : Editor
    {
        SerializedProperty _renders;

        protected virtual void OnEnable()
        {
            _renders = serializedObject.FindProperty("_renders");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            //EditorGUILayout.PropertyField(_renders);
            serializedObject.ApplyModifiedProperties();
        }
        [MenuItem("GameObject/UI/GTabView")]
        public static void GTabViewCreator(MenuCommand command)
        {
            var go = new GameObject("GTabView");
            go.AddComponent<RectTransform>();
            go.AddComponent<GTabView>();

            UIEditorUtil.PlaceUIElementRoot(go, command);
        }
    }
}
