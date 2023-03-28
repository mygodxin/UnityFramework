//------------------------------------------------------------
// 此文件由 ComponentCollection 自动生成，请勿直接修改。
// 生成时间：2023-03-28 16:21:50.75
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework
{
    public partial class AlertWin 
    {
        private Button ssdButton;

        /// <summary>
        /// 初始化组件。
        /// </summary>
        public void InitComponents(GameObject target)
        {
            var collection = target.GetComponent<BindComponent>();
            ssdButton = collection.GetComponent<Button>(0);
        }
    }
}
