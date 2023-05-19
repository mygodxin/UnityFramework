using HS;
using UnityEngine;

/// <summary>
/// 启动场景
/// </summary>
public class Launch : MonoBehaviour
{
    void Start()
    {
        //启动pureMVC

        //初始化配置
        ConfigManager.Inst.Init();

        //启动开始场景
        UIManager.Inst.ShowScene<LoginScene>("打开loginScene");
    }
}
