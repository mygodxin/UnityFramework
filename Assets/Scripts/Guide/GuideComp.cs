using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

namespace UFO
{
    public partial class GuideComp : BaseView, ICanvasRaycastFilter
    {
        public static string path = "GuideComp";

        protected override string[] eventList()
        {
            return new string[]{
            };

        }
        protected override void OnEvent(string eventName, object data)
        {
            switch (eventName)
            {
            }
        }

        protected override void OnInit()
        {
            this.BindComponent(this.gameObject);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return !RectTransformUtility.RectangleContainsScreenPoint(this._bgImage.rectTransform, sp, eventCamera);
        }
        public void SetTarget(RectTransform rect)
        {
            this._bgImage.transform.localPosition = rect.localPosition;
            this._bgImage.rectTransform.sizeDelta = rect.sizeDelta;
        }
    }
}
