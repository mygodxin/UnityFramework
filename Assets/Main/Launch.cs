
using HybridCLR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    public Text text;
    void Start()
    {
        LoadDllAsset();
    }

    void StartGame()
    {
        byte[] assemblyData = GetAssetData("Assembly-CSharp.dll");
        Assembly.Load(assemblyData);
        //AssetBundle prefabAb = AssetBundle.LoadFromMemory(GetAssetData("defaultlocalgroup_assets_all_d1990ef8fedf9fe10470645fc4b5d879.bundle"));
        //GameObject testPrefab = Instantiate(prefabAb.LoadAsset<GameObject>("HotUpdatePrefab.prefab"));

        Addressables.LoadSceneAsync("Assets/Scenes/LoginScene.unity");
       //Addressables.InstantiateAsync("Assets/Prefab/HotUpdate.prefab");
        Debug.Log("添加到场景中");
        this.text.text = "加载完成";
    }

    private static readonly string AssemblyFile = "Assets/Hotfix/";
    public static List<string> HOTAssemblyNames { get; } = new List<string>()
    {
        "Assembly-CSharp.dll"
    };
    public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
    };
    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

    public static byte[] GetAssetData(string dllName)
    {
        return s_assetDatas[dllName];
    }

    private  void LoadDllAsset()
    {
        var assets = HOTAssemblyNames.Concat(AOTMetaAssemblyNames);
        foreach (var asset in assets)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(AssemblyFile + asset + ".bytes");
            handle.Completed += DownloadComplete;
        }
    }

    private void DownloadComplete(AsyncOperationHandle<TextAsset> obj)
    {
        byte[] assetData = obj.Result.bytes;
        Debug.Log($"dll:{obj.Result.name}  size:{assetData.Length}");
        s_assetDatas[obj.Result.name] = assetData;

        if (s_assetDatas.Count == 4)
        {
            LoadMetadataForAOTAssemblies();

            StartGame();
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
