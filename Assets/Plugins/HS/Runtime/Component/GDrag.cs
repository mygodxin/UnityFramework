using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HS
{
    public class GDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler
    {
        private RectTransform rectTransform;
        private Vector3 offset;
        private RectTransform dragArea; // 拖拽区域的 RectTransform，如果为空，则表示全屏拖拽
        private Action<RectTransform, PointerEventData> OnDragComplete; // 拖拽完成时触发的事件
        private Action<RectTransform, PointerEventData> OnDragBegin;  // 拖拽开始时触发的事件
        public Action<RectTransform, PointerEventData> OnClick;
        private bool isDragging = false;    //拖拽标识

        public bool CanDrag = false; //是否开启拖拽

        private void Awake()
        {
            rectTransform = transform.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            offset = rectTransform.position - new Vector3(eventData.position.x, eventData.position.y, rectTransform.position.z);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!CanDrag)
                return;
            this.OnDragBegin?.Invoke(rectTransform, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!CanDrag)
                return;
            // 如果有设置拖拽区域，则限制新位置在拖拽区域内
            if (dragArea && !RectTransformUtility.RectangleContainsScreenPoint(dragArea, eventData.position))
                return;
            rectTransform.position = offset + new Vector3(eventData.position.x, eventData.position.y, rectTransform.position.z);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 触发拖拽完成事件
            if (isDragging)
            {
                if (!CanDrag)
                    return;
                OnDragComplete?.Invoke(rectTransform, eventData);
            }
            else
                OnClick?.Invoke(rectTransform, eventData);
            this.isDragging = false;
        }

        public void SetDrag(RectTransform dragArea, Action<RectTransform, PointerEventData> onDragBegin, Action<RectTransform, PointerEventData> onDragComplete, RectTransform follow = null)
        {
            if (follow != null)
                rectTransform = follow;
            else
                transform.GetComponent<RectTransform>();
            this.dragArea = dragArea;
            this.OnDragComplete = onDragComplete;
            this.OnDragBegin = onDragBegin;
        }
    }
}
