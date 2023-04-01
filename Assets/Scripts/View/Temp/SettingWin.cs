using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFO
{
    public partial class SettingWin : BaseView
    {
        public static string path = "SettingWin";

        protected override string[] eventList()
        {
            return new string[]{
            };

        }
        protected override void OnEvent(string eventName, object data)
        {
            switch (eventName)
            {
            }
        }

        protected override void OnInit()
        {
            this.BindComponent(this.gameObject);
            this._closeButton.onClick.AddListener(() =>
            {
                this.Hide();
            });
            this._openButton.onClick.AddListener(() =>
            {
                UIManager.inst.ShowWindow<AlertWin>("从setting打开alert");
            });
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }
    }
}
