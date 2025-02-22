using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UxmlComponentExporter : Editor
{
    // 指定导出的C#文件路径
    public static readonly string ExportPath = "Assets/ZTest/GeneratedComponents.cs"; // 替换成你想要导出的C#文件路径

    // 定义右键菜单回调方法
    [MenuItem("Assets/Export UXML Components")]
    private static void ExportUxmlComponents()
    {
        // 获取选中的UXML文件路径
        string uxmlPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        // 确保选中的是UXML文件
        if (!uxmlPath.EndsWith(".uxml"))
        {
            Debug.LogWarning("请选择一个UXML文件来触发菜单回调。");
            return;
        }
        // 加载uxml文件
        VisualTreeAsset uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

        if (uxmlAsset != null)
        {
            // 生成C#文件的内容
            string csharpCode = GenerateCSharpCode(uxmlAsset);

            // 将生成的C#代码保存到文件中
            System.IO.File.WriteAllText(ExportPath, csharpCode);

            Debug.Log("组件的定义已导出至：" + ExportPath);
        }
        else
        {
            Debug.LogError("未找到指定的uxml文件：" + uxmlPath);
        }
    }

    // 生成C#文件的代码
    private static string GenerateCSharpCode(VisualTreeAsset uxmlAsset)
    {
        var root = uxmlAsset.CloneTree();
        // 获取uxml文件中的所有组件
        var components = new List<VisualElement>(root.Children());
        // C#代码的头部
        string code = "using UnityEngine;\n";
        code += "using UnityEngine.UIElements;\n\n";
        code += "public class GeneratedComponents\n";
        code += "{\n";

        code += "public class AddTestScript";

        // 为每个组件生成代码
        foreach (var component in components)
        {
            string componentName = component.name;
            string componentType = component.GetType().ToString();

            code += $"\tpublic {componentType} {componentName};\n";

            var comp = root.Q(componentName);
        }

        // C#代码的尾部
        code += "}\n";

        return code;
    }
}
