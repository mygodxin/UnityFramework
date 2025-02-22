using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 滚动方向
    /// </summary>
    public enum ScrollDirection
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// 水平对齐方式
    /// </summary>
    public enum ListHorizontalAlign
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// 垂直对齐方式
    /// </summary>
    public enum ListVerticalAlign
    {
        Top,
        Middle,
        Bottom
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
    public delegate void ListItemRenderer(int index, RectTransform item);

    /// <summary>
    /// Item的Provider
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public delegate RectTransform ListItemProvider(int index);

    /// <summary>
    /// Item信息
    /// </summary>
    class ItemInfo
    {
        public Vector2 Size;
        public RectTransform Item;
        public uint UpdateFlag;
    }

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
        /// 间距
        /// </summary>
        public RectOffset Padding;
        /// <summary>
        /// 列距
        /// </summary>
        public int Spacing;
        /// <summary>
        /// 列距
        /// </summary>
        public int SpacingX;
        public int SpacingY;
        /// <summary>
        /// 布局组件
        /// </summary>
        //public HorizontalOrVerticalLayoutGroup layoutGroup;
        /// <summary>
        /// 滚动方向
        /// </summary>
        public ScrollDirection scrollDirection;
        /// <summary>
        /// 水平对齐
        /// </summary>
        public ListHorizontalAlign HorizontalAlign;
        /// <summary>
        /// 垂直对齐
        /// </summary>
        public ListVerticalAlign VerticalAlign;
        /// <summary>
        /// 循环模式
        /// </summary>
        [SerializeField]
        private bool loop;
        /// <summary>
        /// 绑定数据
        /// </summary>
        public object Data;
        /// <summary>
        /// 滚动窗
        /// </summary>
        private ScrollRect scrollRect;
        /// <summary>
        /// 滚动容器
        /// </summary>
        private RectTransform content;
        /// <summary>
        /// 默认列表项
        /// </summary>
        public RectTransform DefaultItem;
        ///// <summary>
        ///// 默认Item尺寸
        ///// </summary>
        private Vector2 itemSize;
        /// <summary>
        /// Item对象池
        /// </summary>
        private ObjectPool itemPool;
        /// <summary>
        /// 内部维护的item总数量
        /// </summary>
        private int itemCount;
        /// <summary>
        /// 外部传入的item实际数量
        /// </summary>
        private int numItems;
        /// <summary>
        /// 当前渲染的首个Item下标
        /// </summary>
        private int firstIndex;
        /// <summary>
        /// 当前行Item个数
        /// </summary>
        private int curLineItemCount;
        /// <summary>
        /// 渲染批次标识
        /// </summary>
        private uint itemInfoVer;
        /// <summary>
        /// ItemInfo列表
        /// </summary>
        private List<ItemInfo> itemList;
        /// <summary>
        /// 对齐
        /// </summary>
        public bool Snap;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnValueChanged);

            if (scrollRect.content != null)
            {
                DestroyImmediate(scrollRect.content.gameObject);
            }
            //处理content的RectTransform，防止误操作
            var go = new GameObject("Content", typeof(RectTransform));
            go.transform.SetParent(scrollRect.transform, false);
            content = go.GetComponent<RectTransform>();
            content.anchorMin = new Vector2(0, 1f);
            content.anchorMax = new Vector2(0, 1f);
            content.pivot = new Vector2(0, 1f);
            content.offsetMax = Vector2.zero;
            content.offsetMin = Vector2.zero;
            content.localPosition = Vector3.zero;
            content.localRotation = Quaternion.identity;
            content.localScale = Vector3.one;
            content.sizeDelta = scrollRect.GetComponent<RectTransform>().sizeDelta;

            //if (scrollDirection == ScrollDirection.Vertical)
            //    go.AddComponent<VerticalLayoutGroup>();
            //else
            //    go.AddComponent<HorizontalLayoutGroup>();

            //layoutGroup = content.GetComponent<HorizontalOrVerticalLayoutGroup>();
            //layoutGroup.spacing = Spacing;
            //layoutGroup.padding = Padding;
            //layoutGroup.childAlignment = TextAnchor.UpperLeft;
            //layoutGroup.childForceExpandHeight = true;
            //layoutGroup.childForceExpandWidth = true;

            scrollRect.content = content;

            itemList = new List<ItemInfo>();

            itemPool = new ObjectPool();

            if (DefaultItem != null)
            {
                itemSize = DefaultItem.sizeDelta;
            }

            scrollRect.horizontal = scrollDirection == ScrollDirection.Horizontal;
            scrollRect.vertical = scrollDirection == ScrollDirection.Vertical;
        }
        private void FixedUpdate()
        {

        }
        private void OnValueChanged(Vector2 vec2)
        {
            if (scrollRect.content.anchoredPosition.y < 3060 && scrollRect.content.anchoredPosition.y > 1000)
                Debug.Log(scrollRect.content.anchoredPosition.y);
            RefreshLoopPosition();
            OnScroll(false);
        }

        private void OnScroll(bool forceUpdate)
        {
            if (scrollDirection == ScrollDirection.Horizontal)
            {//水平
                HandleHorizontal(forceUpdate);
            }
            else if (scrollDirection == ScrollDirection.Vertical)
            {//垂直
                HandleVertical(forceUpdate);
            }
        }

        private float lastY = 0;
        private void RefreshLoopPosition()
        {
            if (!loop) return;
            var pos = scrollRect.content.anchoredPosition;
            var size = scrollRect.content.sizeDelta;

            float loopSize;
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                loopSize = size.x / 2;
                if (pos.x > 0.001f)
                {
                    pos.x -= loopSize;
                    ResetLoopPos(pos);
                }
                else if (pos.x < -(size.x - scrollRect.GetComponent<RectTransform>().sizeDelta.x))
                {
                    pos.x += loopSize;
                    ResetLoopPos(pos);
                }
            }
            else if (scrollDirection == ScrollDirection.Vertical)
            {
                loopSize = size.y / 2;
                if (pos.y < 0.001f)
                {
                    pos.y += loopSize;
                    ResetLoopPos(pos);
                }
                else if (pos.y > (size.y - scrollRect.GetComponent<RectTransform>().sizeDelta.y))
                {
                    pos.y -= loopSize;
                    ResetLoopPos(pos);
                }
            }
        }

        //loop模式设置新位置时给惯性动点手脚,防止过大出现位置回弹的情况
        private void ResetLoopPos(Vector2 pos)
        {
            var lastVelocity = scrollRect.velocity;
            scrollRect.content.anchoredPosition = pos;
            scrollRect.velocity = new Vector2(lastVelocity.x % scrollRect.content.sizeDelta.x / 4, lastVelocity.y % scrollRect.content.sizeDelta.y / 4);
        }


        private void HandleHorizontal(bool forceUpdate)
        {
            if (itemCount <= 0) return;
            float pos = scrollRect.content.anchoredPosition.x;
            bool end = pos == scrollRect.content.sizeDelta.x;
            float maxX = scrollRect.GetComponent<RectTransform>().rect.width - pos;
            int newFirstIndex = GetIndexOnPos2(ref pos, forceUpdate);

            //根据第一个下标判断是否需要刷新renderer
            if (this.firstIndex == newFirstIndex && !forceUpdate)
                return;
            //Debug.Log("Horizontal刷新首个" + newFirstIndex);
            itemInfoVer++;
            var oldFirstIndex = this.firstIndex;
            this.firstIndex = newFirstIndex;
            bool forward = oldFirstIndex > newFirstIndex;
            int lastIndex = oldFirstIndex + ActiveChildren - 1;
            int reuseIndex = forward ? lastIndex : oldFirstIndex;

            var render = DefaultItem;
            int curIndex = this.firstIndex;
            float curX = pos, curY = 0;
            var needUpdate = false;
            var contentOffsetX = 0;
            var firstOffsetX = 0;
            while (curIndex < itemCount && (end || curX < maxX))
            {
                var item = itemList[curIndex];


                if (item.Item == null || forceUpdate)
                {
                    if (ItemProvider != null)
                    {
                        render = ItemProvider(curIndex % numItems);
                    }

                    if (item.Item != null && item.Item.name != render.name)
                    {
                        RemoveChildToPool(item.Item);
                        item.Item = null;
                    }
                }

                if (item.Item == null)
                {
                    if (forward)
                    {
                        for (int j = reuseIndex; j >= oldFirstIndex; j--)
                        {
                            ItemInfo ii2 = itemList[j];
                            if (ii2.Item != null && ii2.UpdateFlag != itemInfoVer && ii2.Item.name == render.name)
                            {
                                item.Item = ii2.Item;
                                ii2.Item = null;
                                if (j == reuseIndex)
                                    reuseIndex--;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = reuseIndex; j <= lastIndex; j++)
                        {
                            ItemInfo ii2 = itemList[j];
                            if (ii2.Item != null && ii2.UpdateFlag != itemInfoVer && ii2.Item.name == render.name)
                            {
                                item.Item = ii2.Item;
                                ii2.Item = null;
                                if (j == reuseIndex)
                                    reuseIndex++;
                                break;
                            }
                        }
                    }
                    needUpdate = true;
                }
                else
                {
                    needUpdate = forceUpdate;
                }

                if (item.Item == null)
                {
                    var obj = AddItemFromPool(render);
                    item.Item = obj;
                }
                ResetAnchor(item.Item);
                item.Item.SetAsLastSibling();
                item.Item.anchoredPosition = new Vector2(curX, curY);
                item.UpdateFlag = itemInfoVer;

                ItemRenderer?.Invoke(curIndex % numItems, item.Item);

                if (item.Size != item.Item.sizeDelta)
                {

                    if (curIndex == newFirstIndex && oldFirstIndex > newFirstIndex)
                    {
                        // firstOffsetX = 
                    }
                    item.Size = item.Item.sizeDelta;
                }

                curY -= item.Size.y + SpacingY;

                if (curIndex % curLineItemCount == curLineItemCount - 1)
                {
                    curY = 0;
                    curX += item.Size.x + SpacingX;
                }

                if (curIndex == newFirstIndex)
                {
                    maxX += item.Size.x;
                }

                curIndex++;
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                ItemInfo ii = itemList[i];
                if (ii.Item != null && ii.UpdateFlag != itemInfoVer)
                {
                    RemoveChildToPool(ii.Item);
                    ii.Item = null;
                }
            }
            if (needUpdate)
            {
                RefreshContentSize();
                if (loop)
                {
                    // RefreshLoopPosition(contentOffsetX, 0, firstOffsetX, 0);
                }
            }
        }

        private void HandleVertical(bool forceUpdate)
        {
            if (itemCount <= 0) return;
            float pos = scrollRect.content.anchoredPosition.y;
            bool end = pos == -scrollRect.content.sizeDelta.y;
            float maxY = -(pos + scrollRect.GetComponent<RectTransform>().rect.height);
            var newFirstIndex = GetIndexOnPos1(ref pos, forceUpdate);
            //根据第一个下标判断是否需要刷新renderer
            if (this.firstIndex == newFirstIndex && !forceUpdate)
                return;
            Debug.Log("Vertical刷新首个" + newFirstIndex);
            itemInfoVer++;
            var oldFirstIndex = this.firstIndex;
            this.firstIndex = newFirstIndex;
            bool forward = oldFirstIndex > newFirstIndex;
            int lastIndex = oldFirstIndex + ActiveChildren - 1;
            int reuseIndex = forward ? lastIndex : oldFirstIndex;

            var render = DefaultItem;
            int curIndex = newFirstIndex;
            float curX = 0, curY = -pos;
            var updateTag = false;
            var contentOffsetY = 0;
            var firstOffsetY = 0f;
            while (curIndex < itemCount && (end || curY > maxY))
            {
                var item = itemList[curIndex];

                if (item.Item == null || forceUpdate)
                {
                    if (ItemProvider != null)
                    {
                        render = ItemProvider(curIndex % numItems);
                    }

                    if (item.Item != null && item.Item.name != render.name)
                    {
                        RemoveChildToPool(item.Item);
                        item.Item = null;
                    }
                }

                if (item.Item == null)
                {
                    if (forward)
                    {
                        for (int j = reuseIndex; j >= oldFirstIndex; j--)
                        {
                            ItemInfo ii2 = itemList[j];
                            if (ii2.Item != null && ii2.UpdateFlag != itemInfoVer && ii2.Item.name == render.name)
                            {
                                item.Item = ii2.Item;
                                ii2.Item = null;
                                if (j == reuseIndex)
                                    reuseIndex--;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = reuseIndex; j <= lastIndex; j++)
                        {
                            ItemInfo ii2 = itemList[j];
                            if (ii2.Item != null && ii2.UpdateFlag != itemInfoVer && ii2.Item.name == render.name)
                            {
                                item.Item = ii2.Item;
                                ii2.Item = null;
                                if (j == reuseIndex)
                                    reuseIndex++;
                                break;
                            }
                        }
                    }
                }

                if (item.Item == null)
                {
                    var obj = AddItemFromPool(render);
                    item.Item = obj;
                }

                ResetAnchor(item.Item);

                //位置补偿，防止露馅
                if (curIndex == newFirstIndex && newFirstIndex >= oldFirstIndex)
                {
                    firstOffsetY = item.Item.sizeDelta.y - item.Size.y;
                }

                item.Item.SetAsLastSibling();
                item.Item.anchoredPosition = new Vector2(curX, curY);
                item.UpdateFlag = itemInfoVer;

                ItemRenderer?.Invoke(curIndex % numItems, item.Item);

                //列表项动态宽高处理
                if (item.Size != item.Item.sizeDelta)
                {
                    item.Size = item.Item.sizeDelta;
                    if (loop)
                    {
                        for (int i = curIndex + numItems; i < itemCount; i += numItems)
                        {
                            var ii = itemList[i];
                            ii.Size = item.Size;
                        }
                    }
                    updateTag = true;
                }

                curX += item.Size.x + SpacingX;
                if (curIndex % curLineItemCount == curLineItemCount - 1)
                {
                    curX = 0;
                    curY -= item.Size.y + SpacingY;
                }
                if (curIndex == newFirstIndex) //要多显示一条才不会穿帮
                {
                    maxY -= item.Size.y;
                }

                curIndex++;
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                ItemInfo ii = itemList[i];
                if (ii.Item != null && ii.UpdateFlag != itemInfoVer)
                {
                    RemoveChildToPool(ii.Item);
                    ii.Item = null;
                }
            }
            if (updateTag)
            {
                RefreshContentSize();
                if (loop)
                {
                    //位置补偿
                    if (firstOffsetY != 0)
                        ResetLoopPos(new Vector2(scrollRect.content.anchoredPosition.x, scrollRect.content.anchoredPosition.y + firstOffsetY));
                }
            }
        }

        private int GetIndexOnPos1(ref float pos, bool forceUpdate)
        {
            if (itemCount < curLineItemCount)
            {
                pos = 0;
                return 0;
            }

            if (ActiveChildren > 0 && !forceUpdate)
            {
                float pos2 = -itemList[firstIndex].Item.anchoredPosition.y;
                //往下移动时pos
                if (pos2 + (SpacingY > 0 ? 0 : -SpacingY) > pos)
                {
                    for (int i = firstIndex - curLineItemCount; i >= 0; i -= curLineItemCount)
                    {
                        pos2 -= itemList[i].Size.y + SpacingY;
                        if (pos2 <= pos)
                        {
                            pos = pos2;
                            return i;
                        }
                    }

                    pos = 0;
                    return 0;
                }
                else
                {
                    float testGap = SpacingY > 0 ? SpacingY : 0;
                    for (int i = firstIndex; i < itemCount; i += curLineItemCount)
                    {
                        float pos3 = pos2 + itemList[i].Size.y;
                        if (pos3 + testGap >= pos)
                        {
                            pos = pos2;
                            return i;
                        }
                        pos2 = pos3 + SpacingY;
                    }

                    pos = pos2;
                    return itemCount - curLineItemCount;
                }
            }
            else
            {
                float pos2 = 0;
                float testGap = SpacingY > 0 ? SpacingY : 0;
                for (int i = 0; i < itemCount; i += curLineItemCount)
                {
                    float pos3 = pos2 + itemList[i].Size.y;
                    if (pos3 + testGap > pos)
                    {
                        pos = pos2;
                        return i;
                    }
                    pos2 = pos3 + SpacingY;
                }

                pos = pos2;
                return itemCount - curLineItemCount;
            }
        }

        private int GetIndexOnPos2(ref float pos, bool forceUpdate)
        {
            if (itemCount < curLineItemCount)
            {
                pos = 0;
                return 0;
            }

            if (ActiveChildren > 0 && !forceUpdate)
            {
                float pos2 = this.itemList[firstIndex].Item.anchoredPosition.x;
                //往右移动时pos为负数
                if (pos2 + (SpacingX > 0 ? 0 : -SpacingX) > -pos)
                {
                    for (int i = firstIndex - curLineItemCount; i >= 0; i -= curLineItemCount)
                    {
                        pos2 -= itemList[i].Size.x + SpacingX;
                        if (pos2 <= -pos)
                        {
                            pos = pos2;
                            return i;
                        }
                    }

                    pos = 0;
                    return 0;
                }
                else
                {
                    float testGap = SpacingX > 0 ? SpacingX : 0;
                    for (int i = firstIndex; i < itemCount; i += curLineItemCount)
                    {
                        float pos3 = pos2 + itemList[i].Size.x;
                        if (pos3 + testGap > -pos)
                        {
                            pos = pos2;
                            return i;
                        }
                        pos2 = pos3 + SpacingX;
                    }

                    pos = pos2;
                    return itemCount - curLineItemCount;
                }
            }
            else
            {
                float pos2 = 0;
                float testGap = SpacingX > 0 ? SpacingX : 0;
                for (int i = 0; i < itemCount; i += curLineItemCount)
                {
                    float pos3 = pos2 + itemList[i].Size.x;
                    if (pos3 + testGap > pos)
                    {
                        pos = pos2;
                        return i;
                    }
                    pos2 = pos3 + SpacingX;
                }

                pos = pos2;
                return itemCount - curLineItemCount;
            }
        }

        /// <summary>
        /// 设置列表Item数量
        /// </summary>
        public int NumItems
        {
            get
            {
                return numItems;
            }
            set
            {
                numItems = value;
                if (loop)
                    itemCount = numItems * 6;   //注意:循环列表一个方向至少偶数个等数绘制，当content宽高严重大于item宽高时需要更多个
                else
                    itemCount = numItems;

                int oldCount = itemList.Count;
                if (itemCount > oldCount)
                {
                    for (int i = oldCount; i < itemCount; i++)
                    {
                        ItemInfo ii = new ItemInfo();
                        if (DefaultItem != null)
                        {
                            if (ItemProvider != null)
                            {
                                var render = ItemProvider(i % numItems);
                                ii.Size = render.sizeDelta;
                            }
                            else
                                ii.Size = itemSize;
                        }
                        else
                            ii.Size = new Vector2(100, 100);
                        itemList.Add(ii);
                    }
                }
                RefreshContentSize();
                HandleAlign();
                OnScroll(true);
            }
        }

        public bool Loop
        {
            get { return loop; }
            set
            {
                if (curLineItemCount > 1)
                {
                    Debug.LogWarning("LoopList not support gridlayout!");
                }
                loop = value;
            }
        }

        /// <summary>
        /// 刷新Content尺寸
        /// </summary>
        public void RefreshContentSize()
        {
            var viewRect = this.scrollRect.GetComponent<RectTransform>().rect;
            float viewWidth = viewRect.width;
            float viewHeight = viewRect.height;

            if (scrollDirection == ScrollDirection.Horizontal)
            {
                curLineItemCount = Mathf.FloorToInt((viewHeight + SpacingY) / (itemSize.y + SpacingY));
            }
            else if (scrollDirection == ScrollDirection.Vertical)
            {
                curLineItemCount = Mathf.FloorToInt((viewWidth + SpacingX) / (itemSize.x + SpacingX));
            }
            if (curLineItemCount <= 0 || loop)
                curLineItemCount = 1;

            float ch = 0, cw = 0;
            if (itemCount > 0)
            {
                int len = Mathf.CeilToInt((float)itemCount / curLineItemCount) * curLineItemCount;
                int len2 = Math.Min(curLineItemCount, itemCount);
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    for (int i = 0; i < len; i += curLineItemCount)
                        cw += itemList[i].Size.x + SpacingX;
                    if (cw > 0)
                        cw -= SpacingX;

                    ch = viewHeight;
                }
                else if (scrollDirection == ScrollDirection.Vertical)
                {
                    for (int i = 0; i < len; i += curLineItemCount)
                        ch += itemList[i].Size.y + SpacingY;
                    if (ch > 0)
                        ch -= SpacingY;

                    cw = viewWidth;
                }
            }

            scrollRect.content.sizeDelta = new Vector2(cw, ch);
        }

        private void HandleAlign()
        {
            Vector2 newOffset = Vector2.zero;
            var contentSize = scrollRect.content.sizeDelta;
            var contentHeight = contentSize.y;
            var contentWidth = contentSize.x;
            var viewSize = scrollRect.GetComponent<RectTransform>().sizeDelta;
            var viewHeight = viewSize.y;
            var viewWidth = viewSize.x;
            if (contentHeight < viewHeight && scrollDirection == ScrollDirection.Horizontal)
            {
                if (VerticalAlign == ListVerticalAlign.Middle)
                    newOffset.y = (int)((viewHeight - contentHeight) / 2);
                else if (VerticalAlign == ListVerticalAlign.Bottom)
                    newOffset.y = viewHeight - contentHeight;
            }

            if (contentWidth < viewWidth && scrollDirection == ScrollDirection.Vertical)
            {
                if (HorizontalAlign == ListHorizontalAlign.Center)
                    newOffset.x = (int)((viewWidth - contentWidth) / 2);
                else if (HorizontalAlign == ListHorizontalAlign.Right)
                    newOffset.x = viewWidth - contentWidth;
            }

            scrollRect.content.anchoredPosition = newOffset;//
        }

        public RectTransform AddItemFromPool(RectTransform item = null)
        {
            if (item == null) item = DefaultItem;
            RectTransform obj = itemPool.Get(item);
            obj.transform.SetParent(scrollRect.content, false); return obj;
        }

        public void RemoveChildToPool(RectTransform go)
        {
            itemPool.Release(go);
        }

        public void ScrollToIndex(int index, bool playAnimation = false)
        {
            if (itemCount == 0) return;

            if (index >= itemList.Count)
                throw new Exception("Invalid child index: " + index + ">" + itemList.Count);
            if (loop)
            {
                index = Mathf.FloorToInt((float)firstIndex / numItems) * numItems + index;
            }

            Rect rect = new Rect();
            ItemInfo ii = itemList[index];
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                float pos = 0;
                for (int i = curLineItemCount - 1; i < index; i += curLineItemCount)
                    pos -= itemList[i].Size.x + SpacingX;
                rect = new Rect(pos, 0, ii.Size.x, itemSize.y);
            }
            else if (scrollDirection == ScrollDirection.Vertical)
            {
                float pos = 0;
                for (int i = curLineItemCount - 1; i < index; i += curLineItemCount)
                    pos += itemList[i].Size.y + SpacingY;
                rect = new Rect(0, pos, itemSize.x, ii.Size.y);
            }

            if (playAnimation)
            {
            }
            else
            {
                scrollRect.content.anchoredPosition = new Vector2(rect.x, rect.y);
            }
        }

        public int ActiveChildren
        {
            get
            {
                var active = 0;
                for (int i = 0; i < scrollRect.content.childCount; i++)
                {
                    if (scrollRect.content.GetChild(i).gameObject.activeInHierarchy == true)
                    {
                        active++;
                    }
                }
                return active;
            }
        }

        private void ResetAnchor(RectTransform content)
        {
            content.anchorMin = new Vector2(0, 1f);
            content.anchorMax = new Vector2(0, 1f);
            content.pivot = new Vector2(0, 1f);
            //content.offsetMax = Vector2.zero;
            //content.offsetMin = Vector2.zero;
            content.localScale = Vector3.one;
        }
    }
}