using HybridCLR.Editor.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HS.Editor
{
    public class AutoBuildTask : EditorWindow
    {
        [MenuItem("Tools/AutoBuildTask")]
        public static void OpenWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<AutoBuildTask>();
            wnd.titleContent = new GUIContent("AutoBuildTask");
        }

        public ObjectField _hotfixDll;

        public void OnEnable()
        {
            var root = this.rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Build/BuildTaskWindow.uxml");
            visualTree.CloneTree(root);

            //1、调用HybridCLR的构建Dll
            CompileDllCommand.CompileDllActiveBuildTarget();

            //2、拷贝Dll到热更新目录
            CopyHotFixDll2Assets.CopeByActive();

            //3、调用Addressable构建

        }
    }
}