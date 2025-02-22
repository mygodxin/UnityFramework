namespace GameFramework
{
    /// <summary>
    /// 场景
    /// </summary>
    public class BaseScene : BaseWindow
    {
        protected override UILayer Layer => UILayer.Scene;
        protected override void OnInit()
        {
            base.OnInit();
            this.IsClickVoidClose = false;
        }
        protected override void DoShowAnimation()
        {
            RegisterEvent();
            this.OnShow();
        }

        protected override void DoHideAnimation()
        {
            RemoveEvent();
            this.HideImmediately();
        }
    }
}