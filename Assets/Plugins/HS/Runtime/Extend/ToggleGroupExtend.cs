using System;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{








    public static class ToggleGroupExtend
    {
        /// <summary>
        /// 适用于特定情况的ToggleGroup扩展，子Toggle名字必须非重复递增0,1,2,3...
        /// </summary>
        /// <param name="toggleGroup"></param>
        /// <param name="onToggleValue"></param>
        public static void OnValueChanged(this ToggleGroup toggleGroup, Action<int> onToggleValue)
        {
            var toggles = toggleGroup.GetComponentsInChildren<Toggle>(true);
            foreach (var toggle in toggles)
            {
                toggle.onValueChanged.AddListener((bool value) =>
                {
                    var index = int.Parse(toggle.name);
                    if (toggle.isOn)
                        onToggleValue(index);
                });
            }
        }

        public static int GetActiveIndex(this ToggleGroup toggleGroup)
        {
            var toggles = toggleGroup.GetComponentsInChildren<Toggle>(true);
            for (var i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].isOn && toggles[i].gameObject.activeInHierarchy)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void SwitchToggle(this ToggleGroup toggleGroup, int index, bool sendMsg = true)
        {
            var toggles = toggleGroup.GetComponentsInChildren<Toggle>(true);
            if (index > toggles.Length)
            {
                Debug.LogError($"{toggleGroup.name}组件的下标{index}超出范围");
                return;
            };
            //_toggleGroup.NotifyToggleOn(toggles[index], sendMsg);
            for (var i = 0; i < toggles.Length; i++)
            {
                var toggle = toggles[i];
                var select = i == index;
                if (sendMsg)
                    toggles[index].isOn = select;
                else
                    toggles[index].SetIsOnWithoutNotify(select);
            }
        }
    }
}