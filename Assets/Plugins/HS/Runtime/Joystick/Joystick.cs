using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HS
{
    public enum Direction
    {
        Both,
        Horizontal,
        Vertical
    }
    /// <summary>
    /// 虚拟摇杆
    /// </summary>
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool IsDraging
        {
            get
            {
                return this.fingerId != int.MinValue;
            }
        }
        private int fingerId = int.MinValue;
        private Vector2 pointerDownPosition;
        private Vector2 backgroundOriginLocalPostion;
        //为true background位置跟随pointerdown, 为false background位置固定
        [SerializeField] private bool dynamic;
        [SerializeField] private Transform background;
        [SerializeField] private Transform dot;
        [SerializeField] private Transform arrow;
        [SerializeField] private Direction directionAxisd;
        [SerializeField] private bool showDirectionArrow;
        //摇杆移动最大半径
        [SerializeField] private float maxRadius = 38;

        public Action<Vector2> PointerDownHandle;
        public Action<Vector2> PointerUpHandle;
        public Action<Vector2> PointerMoveHandle;
        private void Start()
        {
            this.backgroundOriginLocalPostion = this.background.localPosition;
        }
        private void Update()
        {
            if (this.PointerMoveHandle != null)
                this.PointerMoveHandle.Invoke(this.dot.localPosition / this.maxRadius);
        }
        private void OnDisable()
        {
            this.RestJoystick();
        }
        private void OnValidate()
        {
            this.ConfigJoystick();
        }
        private void ConfigJoystick()
        {
            if (!dynamic) backgroundOriginLocalPostion = background.localPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //适配 Touch：只响应一个Touch；适配鼠标：只响应左键
            if (eventData.pointerId < -1 || this.IsDraging) return;
            fingerId = eventData.pointerId;
            pointerDownPosition = eventData.position;
            if (dynamic)
            {
                //pointerDownPosition[2] = eventData.pressEventCamera?.WorldToScreenPoint(background.position).z ?? background.position.z;
                background.position = eventData.pressEventCamera?.ScreenToWorldPoint(pointerDownPosition) ?? pointerDownPosition;
            }
            if (this.PointerDownHandle != null)
                this.PointerDownHandle.Invoke(eventData.position);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            //正确的手指抬起时才会重置摇杆；
            if (fingerId != eventData.pointerId) return;
            RestJoystick();

            if (this.PointerUpHandle != null)
                this.PointerUpHandle.Invoke(eventData.position);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (fingerId != eventData.pointerId) return;
            //得到background指向dot的向量
            Vector2 direction = eventData.position - pointerDownPosition;
            //获取并锁定向量的长度以控制Handle半径
            float radius = Mathf.Clamp(Vector3.Magnitude(direction), 0, maxRadius);
            Vector2 localPosition = new Vector2()
            {
                //确认是否激活水平轴向
                x = (directionAxisd == Direction.Both || directionAxisd == Direction.Horizontal) ? (direction.normalized * radius).x : 0,
                //确认是否激活垂直轴向
                y = (directionAxisd == Direction.Both || directionAxisd == Direction.Vertical) ? (direction.normalized * radius).y : 0
            };
            dot.localPosition = localPosition;
            if (showDirectionArrow)
            {
                if (!arrow.gameObject.activeInHierarchy) arrow.gameObject.SetActive(true);
                arrow.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, localPosition));
            }
        }
        private void RestJoystick()
        {
            background.localPosition = backgroundOriginLocalPostion;
            dot.localPosition = Vector3.zero;
            arrow.gameObject.SetActive(false);
            fingerId = int.MinValue;
        }
    }
}