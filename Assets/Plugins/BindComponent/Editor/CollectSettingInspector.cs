using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CollectSetting))]
public class CollectSettingInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Extension Code"))
        {
            GenerateExtensionCode();
        }
    }

    private void GenerateExtensionCode()
    {
        CollectSetting settings = (CollectSetting)target;

        if (string.IsNullOrEmpty(settings._codeSavePath))
        {
            Debug.LogError("Code save path is invalid.");
            return;
        }

        if (!Directory.Exists(settings._codeSavePath))
        {
            Directory.CreateDirectory(settings._codeSavePath);
        }

        string nameSpace = settings._namespace;
        if (string.IsNullOrEmpty(nameSpace) || !settings._defaultNameRegex.IsMatch(nameSpace))
        {
            Debug.LogErrorFormat("NameSpace '{0}' is invalid.", nameSpace);
            return;
        }

        if (settings._collectionExtensionCodeTemp == null)
        {
            Debug.LogError("CollectionExtensionCodeTemplate is null, Please check 'CollectSetting' asset.");
            return;
        }

        string codeFileName = string.Format("{0}/CollectExtension.cs", settings._codeSavePath);
        if (!CheckGenerateFile(codeFileName))
        {
            return;
        }

        GenerateExtensionCode(codeFileName, settings._collectionExtensionCodeTemp.text, settings._namespace, settings.componentMapDict.Values.ToList());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private bool CheckGenerateFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return EditorUtility.DisplayDialog("File Exits", " File already exits, continue regenerate ?", "Continue", "Cancel");
        }
        return true;
    }

    public void GenerateExtensionCode(string filePath, string codeTemplate, string nameSpace, List<string> componentTypes)
    {
        if (string.IsNullOrEmpty(codeTemplate))
        {
            Debug.LogError("Component behaviour code template file is invalid.");
            return;
        }

        StringBuilder stringBuilder = new StringBuilder(codeTemplate);

        stringBuilder.Replace("__CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.ff"));
        stringBuilder.Replace("__NAME_SPACE__", nameSpace);
        string function;
        GenerateGetComponentFunction(componentTypes, out function);
        stringBuilder.Replace("__GET_COMPONENT__", function);

        SaveFile(filePath, stringBuilder.ToString());
    }

    private void GenerateGetComponentFunction(List<string> componentTypes, out string function)
    {
        StringBuilder functionBuilder = new StringBuilder();
        foreach (string component in componentTypes)
        {
            functionBuilder.AppendLine(string.Format("        public static {0} Get{0}(this BindComponent comp, int index)", component));
            functionBuilder.AppendLine("        {");
            functionBuilder.AppendLine(string.Format("            return comp.GetComponent<{0}>(index);", component));
            functionBuilder.AppendLine("        }");
            functionBuilder.AppendLine();
        }
        function = functionBuilder.ToString().TrimEnd();
    }

    private void SaveFile(string filePath, string content)
    {
        using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(content);
            }
        }
    }
}
