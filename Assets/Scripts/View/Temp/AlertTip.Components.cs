//------------------------------------------------------------
// 此文件由 BindComponent 自动生成，请勿直接修改。
// 生成时间：2023-04-01 12:47:15.17
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    public partial class AlertTip 
    {
        private Image _bgImage;
        private TextMeshProUGUI _contentTMP;

        public void BindComponent(GameObject target)
        {
            var collection = target.GetComponent<BindComponent>();
            _bgImage = collection.GetComponent<Image>(0);
            _contentTMP = collection.GetComponent<TextMeshProUGUI>(1);
        }
    }
}
