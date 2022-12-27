using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 窗口
/// </summary>
public class Window : GComponent
{
    public GameObject contentPanel;
    public string path = "";
    private bool inited = false;
    public void Show()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        handle.Completed += onLoadComplete;
    }
    void onLoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        contentPanel = Object.Instantiate(handle.Result);
    }
    public void Hide()
    {

    }
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
