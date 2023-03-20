
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UnityFramework
{
    /// <summary>
    /// 窗口
    /// </summary>
    public class Window : GComponent
    {
        protected override string path()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 是否展示模态窗
        /// </summary>
        public bool modal = true;
        /// <summary>
        /// 点击空白处关闭
        /// </summary>
        protected bool clickClose = true;
        private GameObject _clickCloseLayer;

        public Window()
        {
        }

        protected override void onAddedToStage(object data)
        {
            this.data = data;
            //场景切换时view会被清空，重新进行初始化
            if (!_inited || view == null)
            {
                init();
            }
            else
            {
                OnInited();
            }
        }

        protected override void onRemovedFromStage(object data)
        {
            OnHide();
            DoHideAnimation();
        }

        public void Show(object data = null)
        {
            GRoot.inst.ShowWindow(this, data);
        }

        virtual protected void DoShowAnimation()
        {
            view.transform.localScale.Set(.1f, .1f, .1f);
            var tween = view.transform.DOScale(Vector3.one, 0.25f);
            tween.SetEase(Ease.OutBack);
            tween.onComplete += OnShow;
        }

        void init()
        {
            if (_loading)
                return;
            this._inited = true;
            if (view == null)
            {
                var gameObject = Addressables.LoadAssetAsync<GameObject>(ResManager.UIPath + path() + ".prefab").WaitForCompletion();
                Debug.Log(ResManager.UIPath + path() + ".prefab");
                view = UnityEngine.Object.Instantiate(gameObject);
                view.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
            view.SetActive(true);

            this.BindComponent();

            OnInit();

            OnInited();
        }

        private void OnInited()
        {
            view.SetActive(true);
            registerEvent();
            AddClickCloseLayer();
            DoShowAnimation();
        }

        public void Hide()
        {
            GRoot.inst.HideWindow(this);
        }

        protected virtual void DoHideAnimation()
        {
            var tween = view.transform.DOScale(new Vector3(.1f, .1f, .1f), 0.25f);
            tween.SetEase(Ease.InBack);
            tween.onComplete += HideImmediately;
        }

        public void HideImmediately()
        {
            removeEvent();
            view.SetActive(false);
            GRoot.inst.HideWindowImmediately(this);
        }

        private void AddClickCloseLayer()
        {
            if (clickClose)
            {
                if (_clickCloseLayer == null)
                {
                    _clickCloseLayer = new GameObject("clickCloseLayer");
                    var img = _clickCloseLayer.AddComponent<Image>();
                    img.color = new Color(0f, 0f, 0f, 0f);
                    var btn = _clickCloseLayer.AddComponent<Button>();
                    var rectTran = _clickCloseLayer.GetComponent<RectTransform>();
                    if (!rectTran)
                    {
                        rectTran = _clickCloseLayer.AddComponent<RectTransform>();
                    }
                    rectTran.sizeDelta = new Vector2(Screen.width, Screen.height);
                    _clickCloseLayer.transform.SetParent(view.transform);
                    _clickCloseLayer.transform.SetSiblingIndex(0);
                    btn.onClick.AddListener(() =>
                    {
                        Hide();
                    });
                }
                _clickCloseLayer.transform.localPosition = Vector3.zero;
                _clickCloseLayer.SetActive(true);
            }
            else
            {
                if (_clickCloseLayer != null)
                    _clickCloseLayer.SetActive(false);
            }
        }

        Dictionary<string, EventCallback> callbackDic;
        protected virtual string[] eventList()
        {
            return null;
        }
        protected void registerEvent()
        {
            string[] eventList = this.eventList();
            if (eventList != null)
            {
                callbackDic = new Dictionary<string, EventCallback>();
                foreach (var str in eventList)
                {
                    EventCallback callback = delegate (object data)
                    {
                        onEvent(str, data);
                    };
                    Facade.inst.On(str, callback);
                    callbackDic.Add(str, callback);
                }
            }
        }
        protected void removeEvent()
        {
            string[] eventList = this.eventList();
            if (eventList != null)
            {
                foreach (var str in callbackDic)
                {
                    Facade.inst.Off(str.Key, str.Value);
                }
            }
        }
        protected virtual void onEvent(string eventName, object data)
        {
        }
    }

}