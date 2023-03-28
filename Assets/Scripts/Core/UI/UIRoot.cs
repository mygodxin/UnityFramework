using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework
{
    /// <summary>
    /// UI¸ù½Úµã
    /// </summary>
    public class UIRoot
    {
        public readonly float designWidth = 1280;
        public readonly float designHeight = 720;
        public Dictionary<string, Window> winCache;
        public List<Window> winOpen;
        private GameObject _modalLayer;
        private static UIRoot _inst = null;
        public static UIRoot inst
        {
            get
            {
                if (_inst == null)
                    _inst = new UIRoot();
                return _inst;
            }
        }
        public UIRoot()
        {
            winCache = new Dictionary<string, Window>();
            winOpen = new List<Window>();
        }

        public Window GetWindow<T>()
        {
            Type type = typeof(T);
            winCache.TryGetValue(type.ToString(), out var win);
            if (win == null)
            {
                var inst = Activator.CreateInstance(type);
                // instHistory.push({ type: type, inst: inst });
                winCache.Add(type.ToString(), inst as Window);
                win = inst as Window;
            }
            return win;
        }

        public void ShowWindow(Window win, object data = null)
        {
            win.Emit("onAddedToStage", data);
            winOpen.Add(win);
            AdjustModalLayer();
        }

        public void HideWindow(Window win)
        {
            win.Emit("onRemovedFromStage");
            winOpen.Remove(win);
        }
        public void HideWindowImmediately(Window win, bool dispose = false)
        {
            AdjustModalLayer();
        }

        public void ScreenUISelfAdptation(Transform scaleUI)
        {
            float widthrate = this.designWidth / Screen.width;
            float heightrate = this.designHeight / Screen.height;
            float postion_x = scaleUI.GetComponent<RectTransform>().anchoredPosition.x * widthrate;
            float postion_y = scaleUI.GetComponent<RectTransform>().anchoredPosition.y * heightrate;
            scaleUI.localScale = new Vector3(widthrate, heightrate, 1);

            scaleUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(postion_x, postion_y);
        }

        public GameObject modalLayer
        {
            get
            {
                if (_modalLayer == null)
                    CreateModalLayer();

                return _modalLayer;
            }
        }

        void CreateModalLayer()
        {
            _modalLayer = new GameObject("modalLayer");
            var canvas = GameObject.Find("Canvas").transform;
            var img = _modalLayer.AddComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0.5f);
            var rectTran = _modalLayer.GetComponent<RectTransform>();
            rectTran.sizeDelta = new Vector2(Screen.width, Screen.height);
            _modalLayer.transform.SetParent(canvas, false);
            _modalLayer.transform.SetAsFirstSibling();
            _modalLayer.transform.localPosition = Vector3.zero;
            this.ScreenUISelfAdptation(_modalLayer.transform);
            //GameObject.Instantiate
        }

        private void AdjustModalLayer()
        {
            if (_modalLayer == null)
                CreateModalLayer();
            var canvas = GameObject.Find("Canvas").transform;
            int cnt = canvas.childCount;
            _modalLayer.transform.SetSiblingIndex(cnt - 1);
            for (int i = cnt - 1; i >= 0; i--)
            {
                var go = canvas.GetChild(i);
                var name = go.name.Replace("(Clone)", "");
                this.winCache.TryGetValue(name, out var win);
                if (win != null && win.modal && go.gameObject.activeInHierarchy)
                {
                    _modalLayer.SetActive(true);
                    _modalLayer.transform.SetSiblingIndex(i);
                    return;
                }
            }
            _modalLayer.SetActive(false);
        }
    }

}