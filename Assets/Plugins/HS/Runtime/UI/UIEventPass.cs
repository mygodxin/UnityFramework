using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventPass : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    //开关  手动开启关闭功能
    public bool IsPassEvent = false;
    //点击事件
    public void OnPointerClick(PointerEventData eventData)
    {
        Psss(eventData, ExecuteEvents.pointerClickHandler);
    }
    //按下事件
    public void OnPointerDown(PointerEventData eventData)
    {
        //Psss(eventData, ExecuteEvents.pointerDownHandler);
    }
    //弹起事件
    public void OnPointerUp(PointerEventData eventData)
    {
        //Psss(eventData, ExecuteEvents.pointerUpHandler);
    }

    private bool _hasPassedEvent = false;
    public void Psss<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        if (_hasPassedEvent) return;
        _hasPassedEvent = true;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;

        //遍历 RayCastResult   
        for (int i = 0; i < results.Count; i++)
        {
            //results[i].gameObject.name.DLog();
            //剔除穿透脚本所在对象
            if (current != results[i].gameObject)
            {
                //执行多层点击穿透
                // ExecuteEvents.Execute(results[i].gameObject, data, function);

                //只执行单层层穿透  点击事件传递成功break
                if (ExecuteEvents.Execute(results[i].gameObject, data, function))
                {
                    break;
                }
            }
        }
        results.Clear();
        _hasPassedEvent = false;
    }
}