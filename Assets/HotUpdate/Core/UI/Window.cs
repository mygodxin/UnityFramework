using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口
/// </summary>
public class Window : GComponent
{
    public GameObject contentPanel;
    public string path = "";
    private bool inited = false;

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

    }
    /// <summary>
    /// 打开
    /// </summary>
    public virtual void OnShow()
    {

    }
    /// <summary>
    /// 关闭
    /// </summary>
    public virtual void OnHide()
    {

    }
}
