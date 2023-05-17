using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// UI根节点
    /// </summary>
    public class UIRoot
    {
        /// <summary>
        /// 设计宽度
        /// </summary>
        public static float designWidth = 1280;
        /// <summary>
        /// 设计高度
        /// </summary>
        public static float designHeight = 720;
        /// <summary>
        /// UI缓存字典
        /// </summary>
        public Dictionary<string, UIView> cacheDict;
        /// <summary>
        /// 已打开节点列表
        /// </summary>
        public List<UIView> openList;
        /// <summary>
        /// modal层
        /// </summary>
        private GameObject _modalLayer;
        /// <summary>
        /// 当前场景
        /// </summary>
        private UIScene _curScene;
        /// <summary>
        /// 当前场景
        /// </summary>
        public UIScene CurScene
        {
            get
            {
                return this._curScene;
            }
        }

        private static UIRoot _inst = null;
        public static UIRoot Inst
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
            cacheDict = new Dictionary<string, UIView>();
            openList = new List<UIView>();
        }

        public void ShowScene(Type type, object data = null)
        {
            if (this._curScene != null)
            {
                this._curScene.Hide();
            }
            this._curScene = (UIScene)this.ShowWindow(type, data);
        }

        public UIView ShowWindow(Type type, object data = null)
        {
            cacheDict.TryGetValue(type.Name, out var view);
            if (view == null)
            {
                //加载
                //var act = Activator.CreateInstance(type);
                var path = ResManager.UIPath + type.GetFields().FirstOrDefault(field => field.Name == "path").GetValue(null) + ".prefab";
                var go = Loader.LoadAssetAsync<GameObject>(path);
                if (go == null)
                {
                    Debug.LogError("the path not find window:" + path);
                    return null;
                }
                view = (UIView)UnityEngine.Object.Instantiate(go).GetComponent(type);

                var key = type.Name;
                if (this.cacheDict.ContainsKey(key))
                {
                    this.cacheDict[key] = view;
                }
                else
                {
                    this.cacheDict.Add(key, view);
                }

                view.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
            view.gameObject.SetActive(true);

            view.OnAddedToStage(data);

            //新开启面板放最上面
            view.transform.SetAsLastSibling();

            openList.Add(view);

            AdjustModalLayer();

            return view;
        }

        public void HideWindow(UIView view)
        {
            view.Hide();
        }
        public void HideWindowImmediately(UIView view, bool dispose = false)
        {
            openList.Remove(view);
            AdjustModalLayer();
        }

        public void ScreenUISelfAdptation(Transform scaleUI)
        {
            float widthrate = UIRoot.designWidth / Screen.width;
            float heightrate = UIRoot.designHeight / Screen.height;
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

        private void CreateModalLayer()
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
            rectTran.anchorMin = new Vector2(0, 0);
            rectTran.anchorMax = new Vector2(1, 1);
            _modalLayer.AddComponent<Button>();
        }

        private void AdjustModalLayer()
        {
            if (_modalLayer == null)
                CreateModalLayer();
            var canvas = GameObject.Find("Canvas").transform;
            int cnt = canvas.childCount;
            _modalLayer.transform.SetSiblingIndex(cnt - 1);
            var btn = _modalLayer.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();

            for (int i = cnt - 1; i >= 0; i--)
            {
                var go = canvas.GetChild(i);
                var name = go.name.Replace("(Clone)", "");
                this.cacheDict.TryGetValue(name, out var win);
                if (win != null && win.isModal && go.gameObject.activeInHierarchy)
                {
                    if (win.isClickVoidClose)
                    {
                        btn.onClick.AddListener(() =>
                        {
                            win.Hide();
                        });
                    }
                    _modalLayer.SetActive(true);
                    _modalLayer.transform.SetSiblingIndex(i);
                    return;
                }
            }
            _modalLayer.SetActive(false);
        }
    }
}