using HybridCLR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 加载dll
/// </summary>
public class LoadDll
{
    private static readonly string AssemblyFile = "Assets/HotfixPackage/HotfixDll/";
    //必须按照依赖顺序加载
    public static List<string> HOTAssemblyNames { get; } = new List<string>()
    {
        "HS.dll",
        "Assembly-CSharp.dll",
    };
    //没有加载顺序的要求，但加载后会占用6倍dll大小的内存
    public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
    {
        "mscorlib.dll",
        "System.Core.dll",
        "System.dll",
        "Newtonsoft.Json.dll",
        "UnityEngine.CoreModule.dll",
        "UnityWebSocket.Runtime.dll",
        "DOTween.dll",
        "YooAsset.dll",
        "spine-unity.dll",
    };
    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

    public static byte[] GetAssetData(string dllName)
    {
        return s_assetDatas[dllName];
    }

    public static async Task Start()
    {
        var assets = HOTAssemblyNames.Concat(AOTMetaAssemblyNames);
        foreach (var asset in assets)
        {
            var handle = YooAssets.LoadAssetAsync<TextAsset>(AssemblyFile + asset + ".bytes");
            await handle.Task;
            var textAsset = handle.AssetObject as TextAsset;
            handle.Release();
            byte[] assetData = textAsset.bytes;
            Debug.Log($"dll:{textAsset.name}  Size:{assetData.Length}");
            s_assetDatas[textAsset.name] = assetData;

            var len = AOTMetaAssemblyNames.Count + HOTAssemblyNames.Count;
            if (s_assetDatas.Count == len)
            {
                LoadMetadataForAOTAssemblies();
                LoadHotfixAssemblies();
            }
        }
    }

    private static void LoadHotfixAssemblies()
    {
        var assets = HOTAssemblyNames;
        foreach (var asset in assets)
        {
            byte[] assemblyData = GetAssetData(asset);
            Assembly.Load(assemblyData);
        }
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyNames)
        {
            byte[] dllBytes = GetAssetData(aotDllName);
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
