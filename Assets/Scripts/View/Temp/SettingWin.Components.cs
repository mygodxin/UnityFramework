//------------------------------------------------------------
// 此文件由 BindComponent 自动生成，请勿直接修改。
// 生成时间：2023-04-01 12:26:13.16
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UFO
{
    public partial class SettingWin 
    {
        private Image _bgImage;
        private Button _closeButton;
        private TextMeshProUGUI _nameTMP;
        private Button _openButton;
        private TextMeshProUGUI _contentTMP;

        public void BindComponent(GameObject target)
        {
            var collection = target.GetComponent<BindComponent>();
            _bgImage = collection.GetComponent<Image>(0);
            _closeButton = collection.GetComponent<Button>(1);
            _nameTMP = collection.GetComponent<TextMeshProUGUI>(2);
            _openButton = collection.GetComponent<Button>(3);
            _contentTMP = collection.GetComponent<TextMeshProUGUI>(4);
        }
    }
}
