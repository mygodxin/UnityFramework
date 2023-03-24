using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public bool isDraging {
        get
        {
            return this._fingerId != int.MinValue;
        }
    }
    private int _fingerId = int.MinValue;
    private Vector2 _pointerDownPosition;
    private Vector2 _backgroundOriginLocalPostion;
    //为true background位置跟随pointerdown, 为false background位置固定
    public bool dynamic;
    public Transform background;
    public Transform dot;
    public Transform arrow;
    public Direction directionAxisd;
    public bool showDirectionArrow;
    //摇杆移动最大半径
    public float maxRadius = 38;

    public Action<Vector2> onPointerDown;
    public Action<Vector2> onPointerUp;
    public Action<Vector2> onPointerMove;
    void Start()
    {
        this._backgroundOriginLocalPostion = this.background.localPosition;
    }
    void Update()
    {
        if (this.onPointerMove != null)
            this.onPointerMove.Invoke(this.dot.localPosition / this.maxRadius);
    }
    void OnDisable()
    {
        this.RestJoystick();
    }
    void OnValidate()
    {
        this.ConfigJoystick();
    }
    private void ConfigJoystick()
    {
        if (!dynamic) _backgroundOriginLocalPostion = background.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //适配 Touch：只响应一个Touch；适配鼠标：只响应左键
        if (eventData.pointerId < -1 || this.isDraging) return;
        _fingerId = eventData.pointerId;
        _pointerDownPosition = eventData.position;
        if (dynamic)
        {
            //_pointerDownPosition[2] = eventData.pressEventCamera?.WorldToScreenPoint(background.position).z ?? background.position.z;
            background.position = eventData.pressEventCamera?.ScreenToWorldPoint(_pointerDownPosition) ?? _pointerDownPosition;
        }
        if (this.onPointerDown != null)
            this.onPointerDown.Invoke(eventData.position);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //正确的手指抬起时才会重置摇杆；
        if (_fingerId != eventData.pointerId) return;
        RestJoystick();

        if (this.onPointerUp != null)
            this.onPointerUp.Invoke(eventData.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_fingerId != eventData.pointerId) return;
        //得到background指向dot的向量
        Vector2 direction = eventData.position - _pointerDownPosition;
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
        background.localPosition = _backgroundOriginLocalPostion;
        dot.localPosition = Vector3.zero;
        arrow.gameObject.SetActive(false);
        _fingerId = int.MinValue;
    }
}