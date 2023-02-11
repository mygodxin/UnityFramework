using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Drawing.Drawing2D;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public enum LayoutType
{
    Horizontal,
    Vertical,
    Grid
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
    public object data;
    private int _curLineItemCount = 1;

    [Tooltip("行距")]
    public int lineGap;
    [Tooltip("列距")]
    public int columnGap;
    [Tooltip("布局方式")]
    public LayoutType layout;
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
        Debug.Log("item宽=" + itemRect.width + ",高=" + itemRect.height);
        _pool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(defaultItem);
        });
        _children = new List<GameObject>();
    }
    private void OnScroll(Vector2 vec2)
    {
        RefreshVirtualList();
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

                //立即刷新
                RefreshVirtualList();
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
        if (layout == LayoutType.Vertical)
        {
            _curLineItemCount = 1;
        }
        else if (layout == LayoutType.Horizontal || layout == LayoutType.Grid)
        {
            _curLineItemCount = Mathf.FloorToInt(scrollRect.content.rect.width / (_itemSize.x + lineGap));
            if (_curLineItemCount <= 0)
            {
                _curLineItemCount = 1;
            }
        }

        float cw = 0, ch = 0;
        int len = Mathf.CeilToInt((float)_realNumItems / _curLineItemCount) * _curLineItemCount;
        int len2 = Math.Min(_curLineItemCount, _realNumItems);
        if (layout == LayoutType.Horizontal || layout == LayoutType.Grid)
        {
            for (int i = 0; i < len; i += _curLineItemCount)
                ch += _virtualItems[i].size.y + lineGap;
            if (ch > 0)
                ch -= lineGap;


            for (int i = 0; i < len2; i++)
                cw += _virtualItems[i].size.x + columnGap;
            if (cw > 0)
                cw -= columnGap;
        }
        else if (layout == LayoutType.Vertical)
        {
            for (int i = 0; i < len; i += _curLineItemCount)
                cw += _virtualItems[i].size.x + columnGap;
            if (cw > 0)
                cw -= columnGap;

            for (int i = 0; i < len2; i++)
                ch += _virtualItems[i].size.y + lineGap;
            if (ch > 0)
                ch -= lineGap;
        }

        scrollRect.content.sizeDelta = new Vector2(cw, ch);
        //Debug.Log("滚动视图宽高cw=" + cw + ",ch=" + ch);
        //Debug.Log("查看滚动视图宽高cw=" + scrollRect.content.rect.width + ",ch=" + scrollRect.content.rect.height);
    }

    private void RefreshVirtualList()
    {
        if (_realNumItems <= 0) return;

        float itemHeight = _itemSize.y;
        float contentY = scrollRect.content.anchoredPosition.y;
        int index = Mathf.FloorToInt(contentY / itemHeight);
        index = index < 0 ? 0 : index;
        float startY = -index * itemHeight - lineGap * (index - 1);
        int curIndex = 0;
        float curX = 0, curY = startY;
        Debug.Log("contentY=" + contentY + ",index=" + index);
        //多加一个防止穿帮
        float maxY = curY - scrollRect.GetComponent<RectTransform>().rect.height - itemHeight;

        while (curIndex < _realNumItems && (curY > maxY))
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
            //Debug.Log("查看宽高" + item.obj.transform.localScale.x + ",y=" + item.obj.transform.localScale.y);
            //item.obj.transform.position.Set(curX, curY, 0);
            Debug.Log("curIndex=" + curIndex + ",curX=" + curX + ",curY=" + curY);

            itemRenderer(index + curIndex, item.obj);

            curX += item.size.x + columnGap;

            if (curIndex % _curLineItemCount == _curLineItemCount - 1)
            {
                curX = 0;
                curY -= item.size.y + lineGap;
            }

            curIndex++;
        }
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
