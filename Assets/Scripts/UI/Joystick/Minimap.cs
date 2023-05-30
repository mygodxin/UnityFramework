using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 小地图
/// </summary>
public class Minimap : MonoBehaviour
{
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
    }
    void Update()
    {
    }
}