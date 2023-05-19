
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    /// <summary>
    /// ³¡¾°
    /// </summary>
    public class BaseScene : BaseView
    {
        protected override void DoShowAnimation()
        {
            this.OnShow();
        }

        protected override void DoHideAnimation()
        {
            this.HideImmediately();
        }
    }
}