using System;
using System.Collections.Generic;
using UnityEngine;

namespace HS
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
                UIManager.Inst.ShowWindow<AlertWin>("从setting打开alert");
            });
        }

        protected override void OnShow()
        {
            Debug.Log("打印SettingWin-openData" + this.data);
        }

        protected override void OnHide()
        {
        }
    }
}
