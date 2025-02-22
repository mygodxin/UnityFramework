using UnityEngine;
using TMPro;
using System.Collections;

namespace HS
{
    /// <summary>
    /// 自动调整字体大小
    /// </summary>
    public class AutoSizeText : MonoBehaviour
    {
        private TextMeshProUGUI textMeshPro;
        private float padding = 10f;

        private void Start()
        {

            textMeshPro = GetComponent<TextMeshProUGUI>();

            // 延迟一帧后调整TextMeshPro宽度
            StartCoroutine(AdjustTextWidth());
        }

        private IEnumerator AdjustTextWidth()
        {
            yield return null;

            ResizeTextWidth();
        }

        private void ResizeTextWidth()
        {
            float textWidth = textMeshPro.preferredWidth;

            // 计算新的TextMeshPro宽度（加上额外边距）
            float newWidth = textWidth + padding;

            RectTransform rectTransform = textMeshPro.GetComponent<RectTransform>();

            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = newWidth;
            rectTransform.sizeDelta = sizeDelta;
        }
    }
}
