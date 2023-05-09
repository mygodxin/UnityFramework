//------------------------------------------------------------
// 此文件由 BindComponent 自动生成，请勿直接修改。
// 生成时间：2023-03-31 13:39:01.69
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace HS
{
    public partial class AlertWin 
    {
        private Image _bgImage;
        private TextMeshProUGUI _titleTMP;
        private TextMeshProUGUI _contentTMP;
        private Button _leftButton;
        private Button _rightButton;
        private Button _closeButton;

        public void BindComponent(GameObject target)
        {
            var collection = target.GetComponent<BindComponent>();
            _bgImage = collection.GetComponent<Image>(0);
            _titleTMP = collection.GetComponent<TextMeshProUGUI>(1);
            _contentTMP = collection.GetComponent<TextMeshProUGUI>(2);
            _leftButton = collection.GetComponent<Button>(3);
            _rightButton = collection.GetComponent<Button>(4);
            _closeButton = collection.GetComponent<Button>(5);
        }
    }
}
