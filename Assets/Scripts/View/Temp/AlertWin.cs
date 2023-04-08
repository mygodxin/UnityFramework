using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFO
{
    public class AlertParam
    {
        public string content;
    }
    public partial class AlertWin : BaseView
    {
        public static string path = "AlertWin";

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
            this._contentTMP.text = (string)this.data;
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }
    }
}
