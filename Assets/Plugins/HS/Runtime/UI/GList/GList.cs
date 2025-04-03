using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 滚动方向
    /// </summary>
    public enum ScrollDirection
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 选择方式
    /// </summary>
    public enum ListSelectType
    {
        None,
        Single,
        Multiple
    }

    /// <summary>
    /// Item的Render
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public delegate void ListItemRenderer(GListItem item);

    /// <summary>
    /// Item的Provider
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public delegate GListItem ListItemProvider(int index);

    /// <summary>
    /// 选中委托
    /// </summary>
    /// <param name="index"></param>
    public delegate void OnItemSelected(int index);

    /// <summary>
    /// 点击委托
    /// </summary>
    /// <param name="index"></param>
    public delegate void OnItemClicked(int index);

    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(RectMask2D))]
    /// <summary>
    /// 列表
    /// </summary>
    public class GList : MonoBehaviour
    {
        /// <summary>
        /// 列表刷新回调
        /// </summary>
        public ListItemRenderer ItemRenderer;
        /// <summary>
        /// 根据Item下标获取列表项
        /// </summary>
        public ListItemProvider ItemProvider;
        /// <summary>
        /// 点击回调
        /// </summary>
        public OnItemClicked OnItemClicked;
        /// <summary>
        /// 选中回调
        /// </summary>
        public OnItemSelected OnItemSelected;
        /// <summary>
        /// 四周间距
        /// </summary>
        public RectOffset Padding;
        /// <summary>
        /// 格子间距
        /// </summary>
        public int Spacing;
        /// <summary>
        /// 滚动方向
        /// </summary>
        public ScrollDirection scrollDirection;
        /// <summary>
        /// 循环模式
        /// </summary>
        [SerializeField]
        private bool _loop;
        /// <summary>
        /// 对齐
        /// </summary>
        public bool Snap;
        /// <summary>
        /// 选择模式
        /// </summary>
        public ListSelectType SelectType;
        private bool[] _selectArr; // 选中列表
        private int _selectedIndex;
        private int _numItems;
        private float _scrollPosition;
        private List<GListItem> _itemList;
        private RectTransform _scrollRectTran;
        private HorizontalOrVerticalLayoutGroup _layoutGroup;
        private ScrollRect _scrollRect;
        private RectTransform _content;
        private RectTransform _frontPad;
        private RectTransform _backPad;
        private RectTransform _recycle;
        private int _startIndex;
        private int _endIndex;
        private List<float> _offsetList = new List<float>(); // 下标对应的位置
        private List<float> _sizeList = new List<float>(); //下标对应的尺寸
        private Dictionary<int, Queue<GListItem>> _itemPool;
        //loop专用
        private int _loopFirstIndex;
        private int _loopLastIndex;
        private float _loopFirstPosition;
        private float _loopLastPosition;
        private float _loopFirstTrigger;
        private float _loopLastTrigger;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect.onValueChanged.AddListener(OnValueChanged);


            _itemList = new List<GListItem>();
            _itemPool = new Dictionary<int, Queue<GListItem>>();

            _scrollRect.horizontal = scrollDirection == ScrollDirection.Horizontal;
            _scrollRect.vertical = scrollDirection == ScrollDirection.Vertical;
            _scrollRectTran = _scrollRect.GetComponent<RectTransform>();

            CreateContent();
        }

        private void CreateContent()
        {
            _scrollRect.transform.RemoveChildren();
            //content
            var go = new GameObject("Content", typeof(RectTransform));
            go.transform.SetParent(_scrollRect.transform, false);
            _content = go.GetComponent<RectTransform>();
            _content.anchorMin = new Vector2(0, 1f);
            _content.anchorMax = new Vector2(0, 1f);
            _content.pivot = new Vector2(0, 1f);

            _content.sizeDelta = _scrollRectTran.sizeDelta;
            _scrollRect.content = _content;

            //布局
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                _layoutGroup = _content.gameObject.AddComponent<HorizontalLayoutGroup>();
            }
            else
            {
                _layoutGroup = _content.gameObject.AddComponent<VerticalLayoutGroup>();
            }
            _layoutGroup.padding = Padding;
            _layoutGroup.spacing = Spacing;
            _layoutGroup.childAlignment = TextAnchor.UpperLeft;
            _layoutGroup.childForceExpandWidth = false;
            _layoutGroup.childForceExpandHeight = false;
            _layoutGroup.childControlWidth = false;
            _layoutGroup.childControlHeight = false;

            //前后填塞组件
            _frontPad = new GameObject("FrontPad", typeof(RectTransform)).GetComponent<RectTransform>();
            _frontPad.SetParent(_content, false);
            _frontPad.sizeDelta = Vector2.zero;

            _backPad = new GameObject("BackPad", typeof(RectTransform)).GetComponent<RectTransform>();
            _backPad.SetParent(_content, false);
            _backPad.sizeDelta = Vector2.zero;

            _recycle = new GameObject("Recycle", typeof(RectTransform)).GetComponent<RectTransform>();
            _recycle.SetParent(_scrollRect.transform, false);
            _recycle.sizeDelta = Vector2.zero;
            _recycle.gameObject.SetActive(false);
        }

        /// <summary>
        /// 返回列表尺寸  
        /// </summary>
        public float ScrollSize
        {
            get
            {
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    return _content.sizeDelta.x - _scrollRectTran.sizeDelta.x;
                }
                else
                {
                    return _content.sizeDelta.y - _scrollRectTran.sizeDelta.y;
                }
            }
        }

        /// <summary>
        /// 返回滚动框尺寸
        /// </summary>
        public float ScrollRectSize
        {
            get
            {
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    return _scrollRectTran.sizeDelta.x;
                }
                else
                {
                    return _scrollRectTran.sizeDelta.y;
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;

                OnSelectChanged(_selectedIndex);
            }
        }

        private void OnValueChanged(Vector2 vec2)
        {
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                _scrollPosition = vec2.x * ScrollSize;
            }
            else
            {
                _scrollPosition = (1 - vec2.y) * ScrollSize;
            }
            _scrollPosition = Math.Clamp(_scrollPosition, 0, ScrollSize);

            RefreshItem();
        }

        private void CalculateLoopPosition()
        {
            if (_loop)
            {
                var velocity = Vector2.zero;
                if (_scrollPosition < _loopFirstTrigger)
                {
                    velocity = _scrollRect.velocity;
                    _scrollRect.velocity = Vector2.zero;
                    ScrollPosition = _loopLastPosition - (_loopFirstTrigger - _scrollPosition) + Spacing;
                    _scrollRect.velocity = velocity;
                }
                else if (_scrollPosition > _loopLastTrigger)
                {
                    velocity = _scrollRect.velocity;
                    _scrollRect.velocity = Vector2.zero;
                    ScrollPosition = _loopFirstPosition + (_scrollPosition - _loopLastTrigger) - Spacing;
                    _scrollRect.velocity = velocity;
                }
            }
        }

        public void ForceUpdate()
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                var itemInfo = _itemList[i];
                ItemRenderer(itemInfo);
            }
        }

        private void RefreshItem()
        {
            CalculateLoopPosition();

            var startPosition = _scrollPosition;
            var endPosition = _scrollPosition + (scrollDirection == ScrollDirection.Horizontal ? _scrollRectTran.sizeDelta.x : _scrollRectTran.sizeDelta.y);

            var startIndex = GetIndexByPosition(_scrollPosition);
            var endIndex = GetIndexByPosition(endPosition);
            if (startIndex == _startIndex && endIndex == _endIndex)
                return;
            _startIndex = startIndex;
            _endIndex = endIndex;
            //Debug.Log($"[startIndex]={_startIndex},[endIndex]={_endIndex}");
            var reuseList = new List<GListItem>();
            var index = 0;
            while (index < _itemList.Count)
            {
                var itemInfo = _itemList[index];
                if (itemInfo.ListIndex >= startIndex && itemInfo.ListIndex <= endIndex)
                {
                    reuseList.Add(itemInfo);
                    index++;
                }
                else
                {
                    RemoveChildToPool(itemInfo);
                }
            }

            //两头添加缺少的
            if (reuseList.Count == 0)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    var realIndex = i % _numItems;
                    var itemInfo = AddItemFromPool(i, false);
                    itemInfo.IsSelected = _selectArr[realIndex];
                    ItemRenderer(itemInfo);
                }
            }
            else
            {
                var firstIndex = reuseList[0].ListIndex;
                for (int i = endIndex; i >= startIndex; i--)
                {
                    if (i < firstIndex)
                    {
                        var realIndex = i % _numItems;
                        var itemInfo = AddItemFromPool(i, true);
                        itemInfo.IsSelected = _selectArr[realIndex];
                        ItemRenderer(itemInfo);
                    }
                }
                var lastIndex = reuseList[reuseList.Count - 1].ListIndex;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i > lastIndex)
                    {
                        var realIndex = i % _numItems;
                        var itemInfo = AddItemFromPool(i, false);
                        itemInfo.IsSelected = _selectArr[realIndex];
                        ItemRenderer(itemInfo);
                    }
                }
            }
            CalculatePad();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }

        private void CalculatePad()
        {
            if (_numItems == 0) return;

            var firstSize = _offsetList[_startIndex] - _sizeList[_startIndex];
            var lastSize = _offsetList[_offsetList.Count - 1] - _offsetList[_endIndex];

            if (scrollDirection == ScrollDirection.Horizontal)
            {
                _frontPad.sizeDelta = new Vector2(firstSize, _scrollRectTran.sizeDelta.y);
                _backPad.sizeDelta = new Vector2(lastSize, _scrollRectTran.sizeDelta.y);
            }
            else
            {
                _frontPad.sizeDelta = new Vector2(_scrollRectTran.sizeDelta.x, firstSize);
                _backPad.sizeDelta = new Vector2(_scrollRectTran.sizeDelta.x, lastSize);
            }
        }

        private int GetIndexByPosition(float pos)
        {
            return _GetIndexByPosition(pos, 0, _offsetList.Count - 1);
        }

        private int _GetIndexByPosition(float position, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex) return startIndex;

            var middleIndex = (startIndex + endIndex) / 2;

            var pad = scrollDirection == ScrollDirection.Horizontal ? Padding.left : Padding.top;
            if ((_offsetList[middleIndex] + pad) >= (position + (pad == 0 ? 0 : 1.00001f)))
                return _GetIndexByPosition(position, startIndex, middleIndex);
            else
                return _GetIndexByPosition(position, middleIndex + 1, endIndex);
        }

        /// <summary>
        /// 设置列表Item数量
        /// </summary>
        public int NumItems
        {
            get
            {
                return _numItems;
            }
            set
            {
                _numItems = value;
                RecycleAllItem();
                _startIndex = 0;
                _endIndex = 0;
                _selectArr = new bool[_numItems];
                CalcalateSizes();
                RefreshItem();
            }
        }

        private void RecycleAllItem()
        {
            while (_itemList.Count > 0)
            {
                var itemInfo = _itemList.First();
                RemoveChildToPool(itemInfo);
            }
        }

        public float ScrollPosition
        {
            get
            {
                return _scrollPosition;
            }
            set
            {
                if (!_loop)
                {
                    value = Mathf.Clamp(value, 0, ScrollSize);
                }

                if (_scrollPosition != value)
                {
                    _scrollPosition = value;
                    if (scrollDirection == ScrollDirection.Horizontal)
                    {
                        _scrollRect.horizontalNormalizedPosition = (_scrollPosition / ScrollSize);
                    }
                    else
                    {
                        _scrollRect.verticalNormalizedPosition = 1f - (_scrollPosition / ScrollSize);
                    }
                }
            }
        }

        private void CalcalateSizes()
        {
            _sizeList.Clear();
            var totalSize = 0f;
            for (int i = 0; i < _numItems; i++)
            {
                var item = ItemProvider(i);
                if (item != null)
                {
                    var rect = item.GetComponent<RectTransform>();
                    var size = scrollDirection == ScrollDirection.Horizontal ? rect.sizeDelta.x : rect.sizeDelta.y; ;
                    _sizeList.Add(size);
                    totalSize += size;
                }
                else
                {
                    Debug.LogError($"[GList]You must set ItemProvider.");
                }
            }

            //循环列表扩容,默认3倍尺寸，不够填满一页时继续添加
            if (_loop)
            {
                //判断是否足够撑满ScrollSize
                var scrollRectSize = scrollDirection == ScrollDirection.Horizontal ? _scrollRect.content.sizeDelta.x : _scrollRect.content.sizeDelta.y;
                var mulPower = totalSize < scrollRectSize ? ((int)Math.Ceiling(scrollRectSize / totalSize)) * 2 : 2;

                for (int j = 0; j < mulPower; j++)
                {
                    for (int i = 0; i < _numItems; i++)
                    {
                        _sizeList.Add(_sizeList[i] + (i == 0 ? Spacing : 0));
                    }
                }

                _loopFirstIndex = _numItems * mulPower / 2;
                _loopLastIndex = _loopFirstIndex + _numItems - 1;
            }

            _offsetList.Clear();
            var offset = 0f;
            for (int i = 0; i < _sizeList.Count; i++)
            {
                offset += _sizeList[i];
                _offsetList.Add(offset);
            }

            if (scrollDirection == ScrollDirection.Horizontal)
                _content.sizeDelta = new Vector2(_offsetList[_offsetList.Count - 1] + Padding.left + Padding.right, _content.sizeDelta.y);
            else
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, _offsetList[_offsetList.Count - 1] + Padding.top + Padding.bottom);

            if (_loop)
            {
                _loopFirstPosition = GetScrollPositionForItemIndex(_loopFirstIndex, true) + (Spacing * 0.5f);
                _loopLastPosition = GetScrollPositionForItemIndex(_loopLastIndex, false) - ScrollRectSize + (Spacing * 0.5f);

                _loopFirstTrigger = _loopFirstPosition - ScrollRectSize;
                _loopLastTrigger = _loopLastPosition + ScrollRectSize;
            }
        }

        public float GetScrollPositionForItemIndex(int index, bool insertPosition)
        {
            if (_numItems == 0) return 0;
            if (index < 0) index = 0;

            if (index == 0 && insertPosition == true)
            {
                return 0;
            }
            else
            {
                if (index < _offsetList.Count)
                {
                    if (insertPosition == true)
                    {
                        return _offsetList[index - 1] + Spacing + (scrollDirection == ScrollDirection.Horizontal ? Padding.left : Padding.top);
                    }
                    else
                    {
                        return _offsetList[index] + (scrollDirection == ScrollDirection.Horizontal ? Padding.left : Padding.top);
                    }
                }
                else
                {
                    return _offsetList[_offsetList.Count - 2];
                }
            }
        }

        /// <summary>
        /// 列表循环
        /// </summary>
        public bool Loop
        {
            get { return _loop; }
            set
            {
                _loop = value;
                CalcalateSizes();
                RefreshItem();
            }
        }

        /// <summary>
        /// 刷新Content尺寸
        /// </summary>
        public void RefreshContentSize()
        {
            var viewRect = _scrollRectTran.rect;
            float viewWidth = viewRect.width;
            float viewHeight = viewRect.height;
        }

        private GListItem AddItemFromPool(int index, bool isFront)
        {
            var itemPrefab = ItemProvider(index);
            if (itemPrefab == null)
            {
                Debug.LogError($"[GList]Item is null at {index}");
                return null;
            }
            GListItem itemInfo;
            if (_itemPool.TryGetValue(itemPrefab.PrefabTag, out var items) && items.Count > 0)
            {
                itemInfo = items.Dequeue();
            }
            else
            {
                itemInfo = Instantiate(itemPrefab);
            }
            itemInfo.transform.SetParent(_scrollRect.content, false);
            itemInfo.ListIndex = index;
            itemInfo.Index = index % _numItems;
            //层级处理
            if (isFront)
            {
                _itemList.Insert(0, itemInfo);
                itemInfo.transform.SetSiblingIndex(1);
            }
            else
            {
                _itemList.Add(itemInfo);
                itemInfo.transform.SetSiblingIndex(_content.childCount - 2);
            }

            //选择处理
            if (SelectType != ListSelectType.None)
            {
                itemInfo.OnClick = () =>
                {
                    var realIndex = index % _numItems;
                    _selectedIndex = _selectedIndex == realIndex ? -1 : realIndex;
                    if (OnItemClicked != null)
                        OnItemClicked(realIndex);
                    OnSelectChanged(realIndex);
                };
            }

            return itemInfo;
        }

        private void OnSelectChanged(int index)
        {
            if (_loop)
            {
                index = index % _numItems;
            }
            if (SelectType == ListSelectType.Single)
            {
                for (int i = 0; i < _selectArr.Length; i++)
                {
                    _selectArr[i] = _selectedIndex == i;
                }
            }
            else if (SelectType == ListSelectType.Multiple)
            {
                _selectArr[index] = _selectArr[index] ? false : true;
            }
            for (int i = 0; i < _itemList.Count; i++)
            {
                _itemList[i].IsSelected = _selectArr[_itemList[i].Index];
            }
            OnItemSelected(_selectedIndex);
        }

        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public List<int> GetSelection()
        {
            var list = new List<int>();
            for (int i = 0; i < _selectArr.Length; i++)
            {
                if (_selectArr[i])
                    list.Add(i);
            }
            return list;
        }

        /// <summary>
        /// 清空选中
        /// </summary>
        public void ClearSelection()
        {
            for (int i = 0; i < _selectArr.Length; i++)
            {
                _selectArr[i] = false;
            }
            for (int i = 0; i < _itemList.Count; i++)
            {
                _itemList[i].IsSelected = _selectArr[_itemList[i].Index];
            }
        }

        private void RemoveChildToPool(GListItem item)
        {
            if (!_itemPool.TryGetValue(item.PrefabTag, out var items))
            {
                items = new Queue<GListItem>();
                _itemPool.Add(item.PrefabTag, items);
            }
            item.transform.SetParent(_recycle, false);
            items.Enqueue(item);
            _itemList.Remove(item);
        }

        public void ScrollToIndex(int index, bool playAnimation = false)
        {
            var newScrollPosition = 0f;

            if (_loop)
            {
                var numItems = _numItems;

                var set1CellViewIndex = _loopFirstIndex - (numItems - index);
                var set2CellViewIndex = _loopFirstIndex + index;
                var set3CellViewIndex = _loopFirstIndex + numItems + index;

                var set1Position = GetScrollPositionForItemIndex(set1CellViewIndex, true);
                var set2Position = GetScrollPositionForItemIndex(set2CellViewIndex, true);
                var set3Position = GetScrollPositionForItemIndex(set3CellViewIndex, true);

                var set1Diff = (Mathf.Abs(_scrollPosition - set1Position));
                var set2Diff = (Mathf.Abs(_scrollPosition - set2Position));
                var set3Diff = (Mathf.Abs(_scrollPosition - set3Position));

                if (set1Diff < set2Diff)
                {
                    if (set1Diff < set3Diff)
                    {
                        newScrollPosition = set1Position;
                    }
                    else
                    {
                        newScrollPosition = set3Position;
                    }
                }
                else
                {
                    if (set2Diff < set3Diff)
                    {
                        newScrollPosition = set2Position;
                    }
                    else
                    {
                        newScrollPosition = set3Position;
                    }
                }
            }
            else
            {
                newScrollPosition = GetScrollPositionForItemIndex(index, true);

                newScrollPosition = Mathf.Clamp(newScrollPosition, 0, ScrollSize);
            }

            ScrollPosition = newScrollPosition;
        }
    }
}