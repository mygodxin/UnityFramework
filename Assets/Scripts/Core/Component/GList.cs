using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

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
    private LayoutGroup _layout;

    public ScrollRect scrollRect;
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
        //获取布局容器
        _layout = scrollRect.content.GetComponent<LayoutGroup>();
        //监听滚动
        scrollRect.onValueChanged.AddListener(OnScroll);

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
        }
    }

    private void RefreshVirtualList()
    {
        if (_realNumItems <= 0) return;

        int len = Mathf.CeilToInt(_realNumItems / _curLineItemCount * _curLineItemCount);
        int len2 = Math.Min(_curLineItemCount, _realNumItems);
        float cw = 0, ch = 0;
        if (_layout is VerticalLayoutGroup)
        {
            _curLineItemCount = 1;
        }
        else if (_layout is HorizontalLayoutGroup || _layout is GridLayoutGroup)
        {
            _curLineItemCount = Mathf.FloorToInt(scrollRect.content.rect.width);
            if (_curLineItemCount <= 0)
            {
                _curLineItemCount = 1;
            }
        }
        float itemHeight = _itemSize.y;
        float contentY = scrollRect.content.anchoredPosition.y;
        int index = Mathf.FloorToInt(contentY / itemHeight);
        index = index < 0 ? 0 : index;
        float startY = -index * itemHeight;
        int curIndex = 0;
        for (int i = curIndex; i < _numItems; i++)
        {
            var item = _virtualItems[curIndex];
            item.rect.anchoredPosition = new Vector2(item.rect.anchoredPosition.x, startY);
            itemRenderer(curIndex % _numItems, item.obj);
            curIndex++;
        }
    }

    public GameObject AddItemFromPool(GameObject item = null)
    {
        GameObject obj = _pool.Get();
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
