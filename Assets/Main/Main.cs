
using HybridCLR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

/// <summary>
/// 游戏主入口
/// </summary>
public class Main : MonoBehaviour
{
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

    private async void Awake()
    {
        //资源更新
        await ResUpdate.Start(PlayMode);

        //加载DLL
        await LoadDll.Start();

        //更新完成跳转至启动场景
        YooAssets.LoadSceneAsync("HotfixPackage/Launch.scene");
    }
}
