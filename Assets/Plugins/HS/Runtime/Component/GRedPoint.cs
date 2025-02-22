using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 红点组件
    /// </summary>
    public class GRedPoint : MonoBehaviour
    {
        public static bool Switch
        {
            get
            {
                return _switch;
            }
            set
            {
                _switch = value;

            }
        }
        private static bool _switch = true;
        private static Dictionary<string, Func<bool>> refreshFuncDic = new Dictionary<string, Func<bool>>();  //刷新方法
        private static List<GRedPoint> redPointList = new List<GRedPoint>();    //红点列表

        private List<string> focusList = new List<string>();
        private TextMeshProUGUI text;
        private Image image;

        private void Awake()
        {
            var redpoint = new GameObject("RedPoint");
            image = redpoint.AddComponent<Image>();
            image.rectTransform.sizeDelta = Vector2.zero;
            image.LoadImageAsync("Assets/GamePackage/UI/Common/Icon/Other/红点.png", true);
            redpoint.transform.SetParent(transform, false);
            redpoint.transform.localPosition = Vector3.zero;
            var rectTransform = redpoint.GetComponent<RectTransform>();
            // 设置锚点到右上角
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            image.gameObject.SetActive(false);
            RefreshFocus();
        }
        /// <summary>
        /// 注册红点
        /// </summary>
        public static void Register(GRedPoint redPoint)
        {
            if (!redPointList.Contains(redPoint))
                redPointList.Add(redPoint);
        }
        /// <summary>
        /// 取消红点
        /// </summary>
        public static void Cancel(GRedPoint redPoint)
        {
            if (redPointList.Contains(redPoint))
                redPointList.Remove(redPoint);
        }
        /// <summary>
        /// 刷新红点
        /// </summary>
        public static void Refresh(string name)
        {
            foreach (var redPoint in redPointList)
            {
                redPoint.RefreshFocus(name);
            }
        }
        /// <summary>
        /// 注册红点刷新函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        public static void RegisterRefreshFunc(string name, Func<bool> func)
        {
            refreshFuncDic.TryAdd(name, func);
        }

        public void AddFocus(string name)
        {
            if (!this.focusList.Contains(name))
            {
                this.focusList.Add(name); RefreshFocus();
            }
        }
        public void AddFocus(GRedPoint redpoint)
        {
            foreach (var name in redpoint.focusList)
            {
                if (!this.focusList.Contains(name))
                    this.focusList.Add(name);
            }
        }
        public bool FocusState(params string[] names)
        {
            if (!Switch)
            {
                image.gameObject.SetActive(false);
                return false;
            }
            var isActive = false;
            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i];
                if (name == null || focusList.Contains(name))
                {
                    if (GRedPoint.refreshFuncDic.TryGetValue(name, out var func) && func())
                    {
                        isActive = true;
                        break;
                    }
                }
            }
            return isActive;
        }
        public void RemoveFocus(string name)
        {
            if (this.focusList.Contains(name))
                this.focusList.Remove(name);
        }
        private void RefreshFocus(string name = null)
        {
            if (!Switch)
            {
                if (image != null)
                    image.gameObject.SetActive(false);
                return;
            }
            if (name == null || focusList.Contains(name))
            {
                if (image == null) return;
                var isActive = false;
                foreach (var focus in focusList)
                {
                    if (GRedPoint.refreshFuncDic.TryGetValue(focus, out var func) && func())
                    {
                        isActive = true;
                        break;
                    }
                }

                image.gameObject.SetActive(isActive);
            }
        }
    }
}