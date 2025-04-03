using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HS
{
    /// <summary>
    /// 列表Item
    /// </summary>
    public class GListItem : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// 列表项下标
        /// </summary>
        internal int ListIndex;
        /// <summary>
        /// 点击事件
        /// </summary>
        internal Action OnClick;
        /// <summary>
        /// Prefab标识，主要用于多个Item时的区分
        /// </summary>
        public int PrefabTag;
        /// <summary>
        /// 数据下标
        /// </summary>
        public int Index;

        private bool _isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnSelectChange();
                }
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (OnClick != null)
                OnClick();
        }

        /// <summary>
        /// 选中变化
        /// </summary>
        protected virtual void OnSelectChange()
        {
        }
    }
}