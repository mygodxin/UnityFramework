using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS
{
    [Serializable]
    public class TabViewRender
    {
        public Toggle Toggle;
        public GameObject Page;
    }

    [AddComponentMenu("UI/GTabView", 70)]
    [DisallowMultipleComponent]
    public class GTabView : UIBehaviour
    {
        [SerializeField]
        private List<TabViewRender> _renders = new List<TabViewRender>();

        /// <summary>
        /// 监听选择改变
        /// </summary>
        public Action<int> OnSelectedChanged;
        [SerializeField]
        private int _selectedIndex = 0;
        /// <summary>
        /// 当前选中的下标
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                _selectedIndex = value;
                RefreshSelect();
            }
            get
            {
                return _selectedIndex;
            }
        }

        protected GTabView()
        { }

        protected override void Awake()
        {
            SelectedIndex = _selectedIndex;
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            for (var i = 0; i < _renders.Count; i++)
            {
                var item = _renders[i];
                var toggle = item.Toggle;
                if (toggle != null)
                {
                    var index = i;
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                            if (_selectedIndex != index)
                            {
                                _selectedIndex = index;
                                RefreshSelect();
                            }
                        }
                        //else
                        //{
                        //    if (_selectedIndex == index)
                        //    {
                        //        toggle.SetIsOnWithoutNotify(true);
                        //    }
                        //}
                    });
                }
            }
        }

        private void RefreshSelect()
        {
            for (var i = 0; i < _renders.Count; i++)
            {
                var isSelect = i == _selectedIndex;
                var item = _renders[i];
                var toggle = item.Toggle;
                if (toggle != null)
                    toggle.SetIsOnWithoutNotify(isSelect);
                var page = item.Page;
                if (page != null)
                    page.gameObject.SetActive(isSelect);
            }
            OnSelectedChanged?.Invoke(_selectedIndex);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            RegisterEvent();
            RefreshSelect();
        }
#endif
    }
}
