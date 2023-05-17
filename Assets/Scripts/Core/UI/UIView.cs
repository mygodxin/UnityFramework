using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 面板基类，继承自UIComp，使用必须覆盖path，Show会自动加载
    /// </summary>
    public class UIView : UIComp
    {
        /// <summary>
        /// 是否模态窗
        /// </summary>
        public bool isModal = true;
        /// <summary>
        /// 是否点击空白处关闭
        /// </summary>
        public bool isClickVoidClose = true;
        private GameObject _clickCloseLayer;

        /// <summary>
        /// 添加到舞台时调用
        /// </summary>
        public override void OnAddedToStage(object obj = null)
        {
            this.data = obj;
            this.DoShowAnimation();
        }
        /// <summary>
        /// 面板打开动画
        /// </summary>
        protected virtual void DoShowAnimation()
        {
            this.OnShow();
        }
        /// <summary>
        /// 请使用OnHide，如果必须调用OnDisable，请使用base.OnDisable
        /// </summary>
        public override void OnRemovedFromStage()
        {
            this.OnHide();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public override void Hide()
        {
            if (_clickCloseLayer != null)
                _clickCloseLayer.SetActive(false);
            this.DoHideAnimation();
        }
        /// <summary>
        /// 关闭动画
        /// </summary>
        protected virtual void DoHideAnimation()
        {
            this.HideImmediately();
        }
        /// <summary>
        /// 立即关闭，不执行关闭动画
        /// </summary>
        public virtual void HideImmediately()
        {
            base.Hide();
            UIRoot.Inst.HideWindowImmediately(this);
        }
    }
}