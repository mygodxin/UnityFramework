using UnityEditor;
using UnityEngine;

namespace HS
{
    [CustomEditor(typeof(MovieClip))]
    public class MovieClipEditor : Editor
    {
        private MovieClip movieClip;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            movieClip = (MovieClip)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("播放"))
            {
                EditorApplication.update -= UpdateTimer;
                EditorApplication.update += UpdateTimer;
                movieClip.Init();
                movieClip.SetPlaySettings();
            }
            if (GUILayout.Button("停止"))
            {
                EditorApplication.update -= UpdateTimer;
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("测试"))
            {
                MovieClipPreview.OpenMovieClipPreview(movieClip);
            }
        }

        private void UpdateTimer()
        {
            if (movieClip == null) return;
            movieClip.OnTimer();
        }
    }

    public class MovieClipPreview : EditorWindow
    {
        private static MovieClip _movieClip;
        private static Rect rect;

        public static void OpenMovieClipPreview(MovieClip movieClip)
        {
            _movieClip = movieClip;
            movieClip.Init();
            rect = new Rect(0, 0, _movieClip._image.rectTransform.rect.width, _movieClip._image.rectTransform.rect.height);
            movieClip.SetPlaySettings();
            MovieClipPreview window = GetWindow<MovieClipPreview>("MovieClipPreview");
            window.Show();
        }

        private void OnGUI()
        {
            if (_movieClip == null) return;
            _movieClip.OnTimer();
            GUI.DrawTexture(rect, _movieClip._image.sprite.texture);
            EditorUtility.SetDirty(_movieClip);
            this.Repaint();
        }

        private void OnDestroy()
        {
        }
    }
}
