using System;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    public partial class BagWin : BaseView
    {
        public static string path = "BagWin";

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
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }
    }
}
