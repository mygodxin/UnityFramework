using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;

public class AddressableBuilder : EditorWindow
{
    [MenuItem("Window/Addressable Builder")]
    public static void OpenWindow()
    {
        AddressableBuilder window = GetWindow<AddressableBuilder>();
        window.titleContent = new GUIContent("Addressable Builder");
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Build Addressables"))
        {
            BuildAddressables();
        }
    }

    private void BuildAddressables()
    {
        AddressableAssetSettings.CleanPlayerContent();

        AddressableAssetSettings.BuildPlayerContent();
    }
}
