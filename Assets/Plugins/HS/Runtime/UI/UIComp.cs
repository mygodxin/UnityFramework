using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 组件基类，继承自MonoBehaviour，使用必须覆盖path，Show会自动加载
    /// </summary>
    public abstract class UIComp : MonoBehaviour
    {
        /// <summary>
        /// Subview子组件
        /// </summary>
        List<UIComp> _subviews;
        /// <summary>
        /// Subview控制器，激活子视图时会自动切换Toggle状态，并且不发送Toggle状态变化
        /// </summary>
        ToggleGroup _toggleGroup;

        /// <summary>
        /// 父对象
        /// </summary>
        public UIComp Owner => this;

        /// <summary>
        /// UI层级
        /// </summary>
        protected virtual UILayer Layer { get; }

        /// <summary>
        /// 绑定数据
        /// </summary>
        public object Data;

        /// <summary>
        /// 是否初始化
        /// </summary>
        protected bool _isInit = false;

        internal virtual void OnAddedToStage()
        {
            if (!_isInit)
            {
                OnInit();
                _isInit = true;
            }
            OnShow();
        }

        internal virtual void OnRemovedFromStage()
        {
            OnHide();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInit()
        {

        }

        /// <summary>
        /// 打开
        /// </summary>
        protected virtual void OnShow()
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        protected virtual void OnHide()
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Hide()
        {
            HideImmediately(false);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void HideImmediately(bool disposed)
        {
            OnRemovedFromStage();
            if (disposed == true)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 注册控制器
        /// </summary>
        /// <param name="group"></param>
        public void RegisterToggleGroup(ToggleGroup group)
        {
            _toggleGroup = group;
            _toggleGroup.OnValueChanged((index) =>
            {
                ActiveSubview(index, Data);
            });
        }
        /// <summary>
        /// 注册子视图
        /// </summary>
        /// <param name="component"></param>
        public void RegisterSubview(params UIComp[] component)
        {
            if (_subviews == null)
            {
                _subviews = new List<UIComp>();
            }
            _subviews.AddRange(component);
            for (int i = 0; i < _subviews.Count; i++)
            {
                var comp = _subviews[i];
                comp.OnInit();
            }
        }

        /// <summary>
        /// 激活子视图
        /// </summary>
        /// <param name="index"></param>
        /// <param name="param"></param>
        public void ActiveSubview(int index, object param = null, bool sendMsg = false)
        {
            if (_toggleGroup != null)
                _toggleGroup.SwitchToggle(index, sendMsg);
            if (_subviews.Count < index)
                return;
            for (int i = 0; i < _subviews.Count; i++)
            {
                var comp = _subviews[i];
                if (index == i)
                {
                    comp.gameObject.SetActive(true);
                    comp.Data = param;
                    comp.OnShow();
                }
                else
                {
                    comp.gameObject.SetActive(false);
                }
            }
        }
    }
}