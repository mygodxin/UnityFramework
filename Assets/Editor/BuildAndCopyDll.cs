using HybridCLR.Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAndCopyDll
{
    public static string CopyAssetsDir => Application.dataPath + "/HotfixPackage/HotfixDll";

    [MenuItem("HybridCLR/CopyABAOTHotUpdateDlls")]
    public static void CopyABAOTHotUpdateDlls()
    {
        MakeFolder(CopyAssetsDir);
        CopyAOTAssembliesToAssetsPath();
        CopyHotUpdateAssembliesToAssetsPath();
        AssetDatabase.Refresh();
    }

    private static void CopyAOTAssembliesToAssetsPath()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);

        foreach (var dll in SettingsUtil.AOTAssemblyNames)
        {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
            if (!File.Exists(srcDllPath))
            {
                Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                continue;
            }
            string dllBytesPath = $"{CopyAssetsDir}/{dll}.dll.bytes";
            File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
        }
    }

    public static void CopyHotUpdateAssembliesToAssetsPath()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;

        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
        {
            string dllPath = $"{hotfixDllSrcDir}/{dll}";
            string dllBytesPath = $"{CopyAssetsDir}/{dll}.bytes";
            File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
        }
    }

    public static void MakeFolder(String folder)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folder);
        if (directoryInfo.Exists == false)
        {
            directoryInfo.Create();
        }
    }
}