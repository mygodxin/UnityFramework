using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public enum Platform
    {
        Android,
        IOS,
        Web,
        Window
    }
    /// <summary>
    /// 平台适配器
    /// </summary>
    class PlatformAdapter
    {
        private IPlatform _adapter;
        private static PlatformAdapter _inst = null;
        public static PlatformAdapter Inst
        {
            get
            {
                if (_inst == null)
                    _inst = new PlatformAdapter();
                return _inst;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="platform"></param>
        public void Init(Platform platform)
        {
            switch (platform)
            {
                case Platform.Android:
                    this._adapter = new Android();
                    break;
                case Platform.IOS:
                    this._adapter = new IOS();
                    break;
            }
            this._adapter.Init();
        }
        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            this._adapter.Login();
        }
        /// <summary>
        /// 充值
        /// </summary>
        public void Pay()
        {
            this._adapter.Pay();
        }
        /// <summary>
        /// 展示Banner广告
        /// </summary>
        public void ShowBannerAd()
        {
            this._adapter.ShowBannerAd();
        }
        /// <summary>
        /// 隐藏Banner广告
        /// </summary>
        public void HideBannerAd()
        {
            this._adapter.HideBannerAd();
        }
        /// <summary>
        /// 展示激励视频
        /// </summary>
        public void ShowVideoAd()
        {
            this._adapter.ShowVideoAd();
        }
        /// <summary>
        /// 调起分享
        /// </summary>
        public void Share()
        {
            this._adapter.Share();
        }
    }
}
