using System;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    public partial class AlertTip : BaseView
    {
        public static string path = "AlertTip";

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
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }
    }
}
