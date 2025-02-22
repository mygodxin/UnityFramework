using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;

public class EditorUtilityExtension
{
    public static string OpenRelativeFilePanel(string title, string relativeFilePath, string fileExt)
    {
        var rootPath = Directory.GetParent(Application.dataPath).FullName;
        var curFullPath = !string.IsNullOrWhiteSpace(relativeFilePath) ? Path.Combine(rootPath, relativeFilePath) : rootPath;
        var selectPath = EditorUtility.OpenFilePanel(title, Path.GetDirectoryName(curFullPath), fileExt);

        return string.IsNullOrWhiteSpace(selectPath) ? selectPath : Path.GetRelativePath(rootPath, selectPath);
    }
    /// <summary>
    /// 选择相对工程路径文件夹
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="relativePath">默认打开的路径(相对路径)</param>
    /// <returns></returns>
    public static string OpenRelativeFolderPanel(string title, string relativePath)
    {
        var rootPath = Directory.GetParent(Application.dataPath).FullName;
        var curFullPath = !string.IsNullOrWhiteSpace(relativePath) ? Path.Combine(rootPath, relativePath) : rootPath;
        var selectPath = EditorUtility.OpenFolderPanel(title, curFullPath, null);

        return string.IsNullOrWhiteSpace(selectPath) ? selectPath : Path.GetRelativePath(rootPath, selectPath);
    }
}
