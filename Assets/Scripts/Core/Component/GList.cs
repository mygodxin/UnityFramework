using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace HS
{
    public enum ListLayoutType
    {
        SingleColumn,
        SingleRow,
        FlowHorizontal,
        FlowVertical,
        Pagination
    }
    /// <summary>
    /// 列表(支持虚拟化)
    /// </summary>
    public class GList : MonoBehaviour
    {
        public delegate void ListItemRenderer(int index, GameObject item);
        public delegate GameObject ListItemProvider(int index);
        /// <summary>
        /// 列表刷新回调
        /// </summary>
        public ListItemRenderer itemRenderer;
        /// <summary>
        /// 获取列表项
        /// </summary>
        public ListItemProvider itemProvider;
        private bool _virtual;
        private bool _loop;
        private ObjectPool<GameObject> _pool;
        private int _numItems;
        private int _realNumItems;
        private Vector2 _itemSize;
        private List<GameObject> _children;
        public List<GameObject> children
        {
            get
            {
                return _children;
            }
        }
        public object data;
        int _firstIndex; //the top left index
        int _curLineItemCount; //item count in one line
        int _curLineItemCount2; //只用在页面模式，表示垂直方向的项目数
        bool _autoResizeItem;
        [Tooltip("行距")]
        public int lineGap;
        [Tooltip("列距")]
        public int columnGap;
        int _lineCount;
        int _columnCount;
        [Tooltip("布局方式")]
        public ListLayoutType layout;
        public ScrollRect scrollRect;
        [Tooltip("默认Item")]
        public GameObject defaultItem;
        class ItemInfo
        {
            public Vector2 size;
            public GameObject obj;
            public RectTransform rect;
            public uint updateFlag;
            public bool selected;
        }
        private List<ItemInfo> _virtualItems;
        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            if (scrollRect == null)
            {
                throw new Exception("[GList]scrollRect is null");
            }
            if (defaultItem == null)
            {
                throw new Exception("[GList]defaultItem is null");
            }

            //监听滚动
            scrollRect.onValueChanged.AddListener(OnScroll);

            defaultItem.SetActive(false);
            var itemRect = defaultItem.GetComponent<RectTransform>().rect;
            _itemSize.Set(itemRect.width, itemRect.height);
            //Debug.Log("item宽=" + itemRect.width + ",高=" + itemRect.height);
            _pool = new ObjectPool<GameObject>(() =>
            {
                return Instantiate(defaultItem);
            });
            _children = new List<GameObject>();
        }
        private void OnScroll(Vector2 vec2 = new Vector2())
        {
            if (layout == ListLayoutType.SingleColumn || layout == ListLayoutType.FlowHorizontal)
            {
                HandleScroll1();
            }
            else if (layout == ListLayoutType.SingleRow || layout == ListLayoutType.FlowVertical)
            {
                HandleScroll2();
            }
            else
            {
                HandleScroll3();
            }
        }
        private void HandleScroll1()
        {
            if (_realNumItems <= 0) return;

            float itemHeight = _itemSize.y;
            float contentY = scrollRect.content.anchoredPosition.y;
            int index = Mathf.FloorToInt(contentY / (itemHeight + lineGap));
            index = index < 0 ? 0 : index;
            float startY = -index * itemHeight - lineGap * index;
            int curIndex = 0;
            float curX = 0, curY = startY;
            //Debug.Log("contentY=" + contentY + ",index=" + index);
            //没到最后一行时多加一列防止滚动穿帮
            float maxY = Math.Max(curY - scrollRect.GetComponent<RectTransform>().rect.height - itemHeight, -scrollRect.content.sizeDelta.y);

            while (index * _curLineItemCount + curIndex < _realNumItems && (curY > maxY))
            {
                var item = _virtualItems[curIndex];

                if (item.obj == null)
                {
                    var obj = AddItemFromPool();
                    item.obj = obj;
                    item.rect = obj.GetComponent<RectTransform>();
                }

                item.rect.anchoredPosition = new Vector2(curX, curY);
                item.obj.transform.localScale = Vector3.one;

                itemRenderer(index * _curLineItemCount + curIndex, item.obj);

                curX += item.size.x + columnGap;
                if (curIndex % _curLineItemCount == _curLineItemCount - 1)
                {
                    curX = 0;
                    curY -= item.size.y + lineGap;
                }

                curIndex++;
            }
        }

        private void HandleScroll2()
        {
            if (_realNumItems <= 0) return;

            float itemWidth = _itemSize.x;
            float contentX = scrollRect.content.anchoredPosition.x;
            int index = Mathf.FloorToInt(contentX / (itemWidth + columnGap));
            index = index < 0 ? 0 : index;
            float startX = -index * itemWidth - columnGap * index;
            int curIndex = 0;
            float curX = startX, curY = 0;
            //Debug.Log("contentY=" + contentY + ",index=" + index);
            //没到最后一行时多加一列防止滚动穿帮
            float maxX = Math.Max(curX - scrollRect.GetComponent<RectTransform>().rect.width - itemWidth, -scrollRect.content.sizeDelta.x);

            while (curIndex < _realNumItems && (curX > maxX))
            {
                var item = _virtualItems[curIndex];

                if (item.obj == null)
                {
                    var obj = AddItemFromPool();
                    item.obj = obj;
                    item.rect = obj.GetComponent<RectTransform>();
                }

                item.rect.anchoredPosition = new Vector2(curX, curY);
                item.obj.transform.localScale = Vector3.one;

                itemRenderer(index * _curLineItemCount + curIndex, item.obj);

                curY -= item.size.y + lineGap;

                if (curIndex % _curLineItemCount == _curLineItemCount - 1)
                {
                    curY = 0;
                    curX += item.size.x + columnGap;
                }
                curIndex++;
            }
        }

        private void HandleScroll3()
        {

        }

        public void SetVirtual()
        {
            SetVirtual(false);
        }
        void SetVirtual(bool loop)
        {
            if (!_virtual)
            {
                if (loop)
                {

                }

                _virtual = true;
                _loop = loop;
                _virtualItems = new List<ItemInfo>();
                //RemoveChildrenToPool();

                if (_itemSize.x == 0 || _itemSize.y == 0)
                {
                    GameObject obj = _pool.Get();
                    if (obj == null)
                    {
                        Debug.LogError("FairyGUI: Virtual List must have a default list item resource.");
                        _itemSize = new Vector2(100, 100);
                    }
                    else
                    {
                        _itemSize = obj.transform.GetComponent<RectTransform>().rect.size;
                        _itemSize.x = Mathf.CeilToInt(_itemSize.x);
                        _itemSize.y = Mathf.CeilToInt(_itemSize.y);
                        _pool.Release(obj);
                    }
                }
            }
        }
        public int numItems
        {
            get
            {
                if (_virtual)
                    return _numItems;
                else
                    return _children.Count;
            }
            set
            {
                if (_virtual)
                {
                    if (itemRenderer == null)
                        throw new Exception("please set itemRenderer");
                    _numItems = value;

                    if (_loop)
                        _realNumItems = _numItems * 6;//设置6倍数量，用于循环滚动
                    else
                        _realNumItems = _numItems;

                    //_virtualItems的设计是只增不减的
                    int oldCount = _virtualItems.Count;
                    if (_realNumItems > oldCount)
                    {
                        for (int i = oldCount; i < _realNumItems; i++)
                        {
                            ItemInfo ii = new ItemInfo();
                            ii.size = _itemSize;

                            _virtualItems.Add(ii);
                        }
                    }
                    else
                    {
                        for (int i = _realNumItems; i < oldCount; i++)
                            _virtualItems[i].selected = false;
                    }
                    refreshContentSize();
                    //立即刷新
                    OnScroll();
                }
                else
                {
                    int cnt = _children.Count;
                    if (value > cnt)
                    {
                        for (int i = cnt; i < value; i++)
                        {
                            if (itemProvider == null)
                                AddItemFromPool();
                            else
                                AddItemFromPool(itemProvider(i));
                        }
                    }
                    else
                    {
                        RemoveChildrenToPool(value, cnt);
                    }

                    if (itemRenderer != null)
                    {
                        for (int i = 0; i < value; i++)
                            itemRenderer(i, _children[i]);
                    }
                }
                refreshContentSize();
            }
        }

        private void refreshContentSize()
        {
            //计算横向item数量
            var viewRect = this.scrollRect.GetComponent<RectTransform>().rect;
            float viewWidth = viewRect.width;
            float viewHeight = viewRect.height;
            float contentWidth = this.scrollRect.content.rect.width;
            float contentHeight = this.scrollRect.content.rect.height;
            if (layout == ListLayoutType.SingleColumn || layout == ListLayoutType.SingleRow)
                _curLineItemCount = 1;
            else if (layout == ListLayoutType.FlowHorizontal)
            {
                if (_columnCount > 0)
                    _curLineItemCount = _columnCount;
                else
                {
                    _curLineItemCount = Mathf.FloorToInt((viewWidth + columnGap) / (_itemSize.x + columnGap));
                    if (_curLineItemCount <= 0)
                        _curLineItemCount = 1;
                }
            }
            else if (layout == ListLayoutType.FlowVertical)
            {
                if (_lineCount > 0)
                    _curLineItemCount = _lineCount;
                else
                {
                    _curLineItemCount = Mathf.FloorToInt((viewHeight + lineGap) / (_itemSize.y + lineGap));
                    if (_curLineItemCount <= 0)
                        _curLineItemCount = 1;
                }
            }
            else //pagination
            {
                if (_columnCount > 0)
                    _curLineItemCount = _columnCount;
                else
                {
                    _curLineItemCount = Mathf.FloorToInt((viewWidth + columnGap) / (_itemSize.x + columnGap));
                    if (_curLineItemCount <= 0)
                        _curLineItemCount = 1;
                }

                if (_lineCount > 0)
                    _curLineItemCount2 = _lineCount;
                else
                {
                    _curLineItemCount2 = Mathf.FloorToInt((viewHeight + lineGap) / (_itemSize.y + lineGap));
                    if (_curLineItemCount2 <= 0)
                        _curLineItemCount2 = 1;
                }
            }

            float ch = 0, cw = 0;
            if (_realNumItems > 0)
            {
                int len = Mathf.CeilToInt((float)_realNumItems / _curLineItemCount) * _curLineItemCount;
                int len2 = Math.Min(_curLineItemCount, _realNumItems);
                if (layout == ListLayoutType.SingleColumn || layout == ListLayoutType.FlowHorizontal)
                {
                    for (int i = 0; i < len; i += _curLineItemCount)
                        ch += _virtualItems[i].size.y + lineGap;
                    if (ch > 0)
                        ch -= lineGap;

                    if (_autoResizeItem)
                        cw = viewWidth;
                    else
                    {
                        for (int i = 0; i < len2; i++)
                            cw += _virtualItems[i].size.x + columnGap;
                        if (cw > 0)
                            cw -= columnGap;
                    }
                }
                else if (layout == ListLayoutType.SingleRow || layout == ListLayoutType.FlowVertical)
                {
                    for (int i = 0; i < len; i += _curLineItemCount)
                        cw += _virtualItems[i].size.x + columnGap;
                    if (cw > 0)
                        cw -= columnGap;

                    if (_autoResizeItem)
                        ch = viewHeight;
                    else
                    {
                        for (int i = 0; i < len2; i++)
                            ch += _virtualItems[i].size.y + lineGap;
                        if (ch > 0)
                            ch -= lineGap;
                    }
                }
                else
                {
                    int pageCount = Mathf.CeilToInt((float)len / (_curLineItemCount * _curLineItemCount2));
                    cw = pageCount * viewWidth;
                    ch = viewHeight;
                }
            }

            scrollRect.content.sizeDelta = new Vector2(cw, ch);
            //Debug.Log("滚动视图宽高cw=" + cw + ",ch=" + ch);
            //Debug.Log("查看滚动视图宽高cw=" + scrollRect.content.rect.width + ",ch=" + scrollRect.content.rect.height);
        }

        public GameObject AddItemFromPool(GameObject item = null)
        {
            GameObject obj = _pool.Get();
            obj.SetActive(true);
            obj.transform.SetParent(scrollRect.content);
            _children.Add(obj);
            return obj;
        }
        public void RemoveChildrenToPool(int beginIndex, int endIndex)
        {
            if (endIndex < 0 || endIndex >= _children.Count)
                endIndex = _children.Count - 1;

            for (int i = beginIndex; i <= endIndex; ++i)
            {
                _pool.Release(_children[i]);
                _children.RemoveAt(i);
            }
        }
    }
}