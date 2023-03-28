using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UnityFramework
{
    /// <summary>
    /// 窗口
    /// </summary>
    public class Window : GComponent
    {
        protected virtual string path()
        {
            throw new System.NotImplementedException();
        }
        protected bool _inited = false;
        protected bool _loading = false;
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
            On("onAddedToStage", onAddedToStage);
            On("onRemovedFromStage", OnRemovedFromStage);
        }

        protected void onAddedToStage(object data)
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

        protected void OnRemovedFromStage(object data)
        {
            OnHide();
            DoHideAnimation();
        }

        public void Show(object data = null)
        {
            UIRoot.inst.ShowWindow(this, data);
        }

        virtual protected void DoShowAnimation()
        {
            this.OnShow();
        }

        protected void init()
        {
            if (_loading)
                return;
            this._inited = true;
            if (view == null)
            {
                var gameObject = Addressables.LoadAssetAsync<GameObject>(ResManager.UIPath + path() + ".prefab").WaitForCompletion();
                if (gameObject == null)
                {
                    Debug.LogError("the path not find window:" + ResManager.UIPath + path() + ".prefab");
                    return;
                }
                view = UnityEngine.Object.Instantiate(gameObject);
                view.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
            view.SetActive(true);

            this.BindComponent();

            OnInit();

            OnInited();
        }

        protected virtual void OnInited()
        {
            view.SetActive(true);
            AddClickCloseLayer();
            DoShowAnimation();
        }

        public void Hide()
        {
            UIRoot.inst.HideWindow(this);
        }

        protected virtual void DoHideAnimation()
        {
            this.HideImmediately();
        }

        public virtual void HideImmediately()
        {
            view.SetActive(false);
            UIRoot.inst.HideWindowImmediately(this);
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
    }

}