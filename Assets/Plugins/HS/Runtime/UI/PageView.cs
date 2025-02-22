using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS
{
    public class PageView : MonoBehaviour
    {
        //是否循环
        [Tooltip("是否可循环")] public bool isCirculation = true;

        [Tooltip("所有Item的父节点")] public GameObject ContentGo;

        [Tooltip("所有Item是否宽度一致")] public bool isItemUniform = true;

        [Tooltip("移动速度")] public float _speed = 500;

        [Tooltip("初始显示哪个Item，用来判断是否到了最边上，不能再移动")]
        public int initShowIndex = 1;

        [Tooltip("是否可以点击Item移动，相对显示位置Index移动")]
        public bool isClickItemMove = true;

        [Tooltip("设置每次移动的间隔，如果不设置，按第一个和最后一个的位置来算平均值，锚点需要一致")]
        public float _interval = 0;

        public int limit = 50;

        //当前显示的Index，初始为0
        private int showIndex = 0;

        //当showIndex为0时，content的位置
        private float contentInitPositionX = 0;

        //所有Item列表
        private List<RectTransform> _transformList = new List<RectTransform>();

        private List<float> _positionList = new List<float>();

        //第一个Item
        private RectTransform _initRect;

        //最后一个Item
        private RectTransform _endRect;

        //当前点击事件Id
        //按下时鼠标坐标X值
        private float _initMouserX;

        private Transform _contentTransform;

        /// <summary>
        /// scroll的最小值X和最大值X
        /// </summary>
        private Vector2 scrollInitEndX;

        //改变ScollView位置
        private Vector2 _changeScorllViewVector2 = new Vector2();

        //循环时改变Item位置
        private Vector2 _changeItemVector2 = new Vector2();

        //需要移动的目标位置
        private float endPositon = 0;

        // Start is called before the first frame update
        void Awake()
        {
            Init();
            RecordContentPosition();
            InitPanleView();
            if (isClickItemMove)
            {
                SetItemClick();
            }
        }

        private void Init()
        {
            _positionList.Clear();


            _contentTransform = ContentGo.transform;
            var length = _contentTransform.childCount;
            if (length <= 0)
            {
                Debug.Log("Scroll子节点数量为空");
                return;
            }

            for (int i = 0; i < length; i++)
            {
                RectTransform tr = _contentTransform.GetChild(i).GetComponent<RectTransform>();
                if (i == 0) _initRect = tr;
                else if (i == length - 1) _endRect = tr;
                _transformList.Add(tr);
                _positionList.Add(tr.localPosition.x);
            }

            if (isItemUniform)
            {
                if (_interval == 0) _interval = (_positionList[_positionList.Count - 1] - _positionList[0]) / (_positionList.Count - 1);
            }
        }

        //记录Content初始位置
        private void RecordContentPosition()
        {
            showIndex = 0;
            endPositon = _contentTransform.localPosition.x;
            contentInitPositionX = _contentTransform.localPosition.x;
        }

        /// <summary>
        /// 初始化可视化区域
        /// </summary>
        private void InitPanleView()
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            scrollInitEndX = GetInitEndXByRect(rectTransform);
        }

        /// <summary>
        /// 设置Item点击事件
        /// </summary>
        private void SetItemClick()
        {
            var length = _contentTransform.childCount;
            if (length <= 0)
            {
                return;
            }

            for (int i = 0; i < length; i++)
            {
                Button btn = _contentTransform.GetChild(i).GetComponent<Button>();
                if (!btn)
                {
                    btn = _contentTransform.GetChild(i).gameObject.AddComponent<Button>();
                }

                btn.onClick.AddListener(ClickItem);
            }
        }

        /// <summary>
        /// Item的点击事件
        /// </summary>
        private void ClickItem()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                var clickObj = EventSystem.current.currentSelectedGameObject;
                var clickIndex = -1;
                var length = _transformList.Count;
                for (int i = _transformList.Count - 1; i >= 0; i--)
                {
                    if (clickObj == _transformList[i].gameObject)
                    {
                        clickIndex = i;
                    }
                }

                if (clickIndex != -1)
                {
                    Debug.Log("点击了clickIndex：" + clickIndex);

                    if (!this.isCirculation)
                    {

                        var realIndex = GetNowClickIndex();
                        //如果不循环才可以将点击Index和现在显示的index对比，然后+showIndex，得到该移动的位置
                        var toIndex = showIndex + (clickIndex - realIndex);
                        ToMoveByIndex(toIndex);
                    }
                    else
                    {
                        //如果循环只能左右移动一格
                        if (clickIndex > initShowIndex) ToLeft();
                        else if (clickIndex < initShowIndex) ToRight();
                    }

                }
            }
        }

        //往左移动，判断第一个Item右侧是否超过显示框最左边，超过则将Item移动到末尾
        private void MoveLeft()
        {
            var itemRect = _transformList.First();
            var rectInitEndX = GetInitEndXByRect(itemRect);
            var rectTransform = gameObject.GetComponent<RectTransform>();
            scrollInitEndX = GetInitEndXByRect(rectTransform);
            if (rectInitEndX.y <= scrollInitEndX.x)
            {
                Debug.Log("该移动了");
                var endRect = _endRect;
                _changeItemVector2.x = endRect.localPosition.x + _interval;
                _changeItemVector2.y = itemRect.localPosition.y;
                itemRect.localPosition = _changeItemVector2;
                if (isCirculation)
                {
                    _endRect = itemRect;
                    _transformList.RemoveAt(0);
                    _transformList.Add(itemRect);
                    _initRect = _transformList[0];
                }
            }
        }

        //往右移动，判断最后一个Item左侧是否超过显示框最右边，超过则将Item移动到首位
        private void MoveRight()
        {
            var itemRect = _transformList.Last();
            var rectInitEndX = GetInitEndXByRect(itemRect);
            if (rectInitEndX.x >= scrollInitEndX.y)
            {
                var initRect = _initRect;
                _changeItemVector2.x = initRect.localPosition.x - _interval;
                _changeItemVector2.y = itemRect.localPosition.y;
                itemRect.localPosition = _changeItemVector2;
                if (isCirculation)
                {
                    //将最后一个放到第一个来
                    _initRect = itemRect;
                    InsertItemList(itemRect);
                    _endRect = _transformList[_transformList.Count - 1];
                }
            }
        }

        #region 滑动处理

        //是否点击
        private bool isTouch = false;

        //当前点击（防止多点触碰）
        private int pointerId = 0;

        //是否已经拖拽，需要开始移动
        private bool isDrag = false;

        //移动方向
        private int dir = 0;

        /// <summary>
        /// 开始滑动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isTouch) return;
            if (pointerId == 0)
            {
                pointerId = eventData.pointerId;
                isTouch = true;
                _initMouserX = eventData.position.x;
            }
        }

        /// <summary>
        /// 滑动结束
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerId == pointerId)
            {
                isTouch = false;
                pointerId = 0;
                var endMouserX = eventData.position.x;
                if (endMouserX > _initMouserX)
                {
                    if (endMouserX - _initMouserX >= this.limit)
                    {
                        Debug.Log("大于了50,向右移动");
                        ToRight();
                    }
                }
                else
                {
                    if (_initMouserX - endMouserX >= this.limit)
                    {
                        print("大于了50,向左移动");
                        ToLeft();
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isDrag)
            {
                if (isItemUniform)
                {
                    var toX = _contentTransform.localPosition.x + Time.deltaTime * _speed * dir;
                    _changeScorllViewVector2.x = toX;
                    _changeScorllViewVector2.y = _contentTransform.localPosition.y;
                    _contentTransform.localPosition = _changeScorllViewVector2;
                    //如果循环，需要判断
                    if (isCirculation)
                    {
                        if (dir == 1)
                        {
                            this.MoveRight();
                        }

                        if (dir == -1)
                        {
                            this.MoveLeft();
                        }
                    }

                    //超过判断
                    if ((dir == 1 && _contentTransform.localPosition.x >= endPositon) ||
                        (dir == -1 && _contentTransform.localPosition.x <= endPositon))
                    {
                        isDrag = false;
                        dir = 0;
                        _changeScorllViewVector2.x = endPositon;
                        _contentTransform.localPosition = _changeScorllViewVector2;
                    }
                }
            }
        }

        #endregion

        #region 逻辑

        /// <summary>
        /// 根据RectTransform得到最小值X和最大值X
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        private Vector2 GetInitEndXByRect(RectTransform rectTransform)
        {
            var v2 = new Vector2();
            var x = rectTransform.position.x;
            var width = (float)(rectTransform.rect.width * rectTransform.lossyScale.x);
            v2.x = x - rectTransform.pivot.x * width;
            v2.y = v2.x + width;

            return v2;
        }

        /// <summary>
        /// 插入一个Item，放到_transformList首位
        /// </summary>
        /// <param name="itemRect"></param>
        private void InsertItemList(RectTransform itemRect)
        {
            var length = _transformList.Count;
            RectTransform temp = _transformList[length - 1];
            for (int i = length - 1; i >= 0; i--)
            {
                if (i >= 1)
                {
                    _transformList[i] = _transformList[i - 1];
                }
                else _transformList[i] = temp;
            }
        }
        /// <summary>
        /// 得到当前的Index（0-content.length范围）
        /// </summary>
        /// <returns></returns>
        private int GetNowClickIndex()
        {
            var realIndex = showIndex + initShowIndex;
            Debug.Log("当前真实的realIndex:" + realIndex);
            int a = 0;
            var length = _transformList.Count;
            while (realIndex < 0)
            {
                realIndex += length;
            }
            while (realIndex > length - 1)
            {
                realIndex -= length;
            }

            return realIndex;
        }

        #endregion
        /// <summary>
        /// 移动几个位置
        /// </summary>
        /// <param name="num"></param>
        public void ToLeft(int num = 1)
        {
            //初始位置减去已经显示的Index，判断是否超出边界
            if (!isCirculation && initShowIndex - showIndex >= _transformList.Count - 1) return;
            isDrag = true;
            dir = -1;
            showIndex += num;

            endPositon = contentInitPositionX - showIndex * _interval;
        }
        /// <summary>
        /// 移动几个位置
        /// </summary>
        /// <param name="num"></param>
        public void ToRight(int num = 1)
        {
            //初始位置减去已经显示的Index，判断是否超出边界
            if (!isCirculation && initShowIndex - showIndex <= 0) return;
            isDrag = true;
            dir = 1;
            showIndex -= num;

            endPositon = contentInitPositionX - showIndex * _interval;
        }

        public void ToMoveByIndex(int toIndex)
        {
            isDrag = true;
            dir = toIndex > showIndex ? -1 : 1;
            showIndex = toIndex;
            endPositon = contentInitPositionX - showIndex * _interval;
        }
    }
}
