using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    public static class SliderExtend
    {
        /// <summary>
        /// 设置进度条的值，并设置进度条上的进度文本
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        public static void SetValue(this Slider slider, float value, float maxValue = default, bool animation = false)
        {
            if (maxValue != default)
                slider.maxValue = maxValue;
            var red = ColorUtility.ToHtmlStringRGB(Color.red);
            var green = ColorUtility.ToHtmlStringRGB(Color.green);
            var title = slider.transform.GetChildWithName("Title")?.GetComponent<TextMeshProUGUI>();
            if (title != null)
                title.text = $"<color=#{(value < slider.maxValue ? red : green)}>{value}</color>/{slider.maxValue}";
            //if (animation)
            //{
                //slider.value = 0;
                //DOTween.To(() => slider.value, x => slider.value = x, value, 1);
            //}
            //else
            {
                slider.value = value;

            }
        }
    }
}