using UnityEditor;
using UnityEngine;

namespace HS
{
    [CustomEditor(typeof(GList)), CanEditMultipleObjects]
    public class GListEditor : Editor
    {

        protected virtual void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            //EditorGUILayout.PropertyField(_renders);
            serializedObject.ApplyModifiedProperties();
        }
        [MenuItem("GameObject/UI/GList")]
        public static void GListCreator(MenuCommand command)
        {
            var go = new GameObject("GList");
            go.AddComponent<RectTransform>();
            go.AddComponent<GList>();

            UIEditorUtil.PlaceUIElementRoot(go, command);
        }
    }
}
