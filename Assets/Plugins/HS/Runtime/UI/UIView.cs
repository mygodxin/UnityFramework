using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 面板基类，继承自UIComp，使用必须覆盖path，Show会自动加载
    /// </summary>
    public class UIView : UIComp
    {
        protected override UILayer Layer => UILayer.Window;
        /// <summary>
        /// 是否模态窗
        /// </summary>
        public bool IsModal = true;
        /// <summary>
        /// 是否点击空白处关闭
        /// </summary>
        public bool IsClickVoidClose = true;

        internal override void OnAddedToStage()
        {
            if (!_isInit)
            {
                OnInit();
                _isInit = true;
            }
            DoShowAnimation();
        }

        /// <summary>
        /// 面板打开动画
        /// </summary>
        protected virtual void DoShowAnimation()
        {
            OnShow();
        }

        internal override void OnRemovedFromStage()
        {
            OnHide();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public override void Hide()
        {
            DoHideAnimation();
        }
        /// <summary>
        /// 关闭动画
        /// </summary>
        protected virtual void DoHideAnimation()
        {
            HideImmediately();
        }
        /// <summary>
        /// 立即关闭，不执行关闭动画
        /// </summary>
        public override void HideImmediately(bool dispose = false)
        {
            UIRoot.Inst.HideWindowImmediately(this, dispose);
        }
    }
}