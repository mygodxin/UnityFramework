using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 进度条组件
    /// </summary>
    [AddComponentMenu("UI/GProgressBar", 71)]
    [DisallowMultipleComponent]
    public class GProgressBar : UIBehaviour
    {
        /// <summary>
        /// 进度条图
        /// </summary>
        public Image BarImage;
        /// <summary>
        /// 进度条值
        /// </summary>
        public TextMeshProUGUI ValueText;


        private double _value = 0.5f;
        /// <summary>
        /// 当前值
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RefreshValue();
            }
        }

        private double _max = 1;
        /// <summary>
        /// 最大值
        /// </summary>
        public double Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }

        private void RefreshValue()
        {
            if (BarImage != null)
                BarImage.fillAmount = (float)(_value / _max);
            if (ValueText != null)
                ValueText.text = $"{_value}/{_max}";
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            RefreshValue();
        }
#endif
    }
}