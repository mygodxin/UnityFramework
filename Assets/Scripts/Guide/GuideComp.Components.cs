//------------------------------------------------------------
// 此文件由 BindComponent 自动生成，请勿直接修改。
// 生成时间：2023-04-03 13:18:28.52
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    public partial class GuideComp 
    {
        private Image _bgImage;
        private Image _targetImage;

        public void BindComponent(GameObject target)
        {
            var collection = target.GetComponent<BindComponent>();
            _bgImage = collection.GetComponent<Image>(0);
            _targetImage = collection.GetComponent<Image>(1);
        }
    }
}
