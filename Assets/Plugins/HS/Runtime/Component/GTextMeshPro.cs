using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HS
{
    /// <summary>
    /// 文本链接点击
    /// </summary>
    public class GTextMeshPro : MonoBehaviour, IPointerClickHandler
    {
        public event Action<string> OnLinkClicked;
        public void OnPointerClick(PointerEventData eventData)
        {
            var tmp = GetComponent<TMP_Text>();
            var index = TMP_TextUtilities.FindIntersectingLink(tmp, eventData.position, null);
            if (index != -1)
            {
                OnLinkClicked?.Invoke(tmp.textInfo.linkInfo[index].GetLinkID());
            }
        }
    }
}