//------------------------------------------------------------
// 此文件由 BindComponent 自动生成，请勿直接修改。
// 生成时间：2023-05-13 17:50:18.06
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    public partial class LoginScene 
    {
        private Button _exitButton;
        private Button _startButton;
        private Button _settingButton;
        private TextMeshProUGUI _startTMP;

        public void BindComponent(GameObject target)
        {
            var collection = target.GetComponent<BindComponent>();
            _exitButton = collection.GetComponent<Button>(0);
            _startButton = collection.GetComponent<Button>(1);
            _settingButton = collection.GetComponent<Button>(2);
            _startTMP = collection.GetComponent<TextMeshProUGUI>(3);
        }
    }
}
