using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

//自动绑定组件，配置在CollectSetting文件中
public class BindComponent : Editor
{
    private static string codeSavePath = "";
    [MenuItem("GameObject/Bind Component Default Path _F1")]
    private static void BindComponentDefault(MenuCommand menuCommand)
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            var uibind = new UIBind();
            uibind.Bind(selectedObject);
        }
    }
    [MenuItem("GameObject/Bind Component Custom Path &F1")]
    private static void BindComponentCustomPath(MenuCommand menuCommand)
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            var uibind = new UIBind();
            string path = EditorUtility.SaveFolderPanel("Select Save Path", codeSavePath, string.Empty);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Select folder Path is invalid.");
            }
            else
            {
                int index = path.IndexOf("Assets", StringComparison.Ordinal);
                if (index < 0)
                {
                    Debug.LogWarning("Suggest to save to any folder of 'Assets'.");
                }
                codeSavePath = path.Substring(index);
                uibind.Bind(selectedObject);
            }
        }
    }
}


