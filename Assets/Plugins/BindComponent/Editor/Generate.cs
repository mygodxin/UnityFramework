using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

//生成代码
public class Generate : IGenerate
{
    public void GenerateComponentsCode(string filePath, string codeTemplate, string nameSpace, string className, Dictionary<string, string> fieldTypeDict)
    {
        if (string.IsNullOrEmpty(codeTemplate))
        {
            Debug.LogError("Component collection code template file is invalid.");
            return;
        }

        StringBuilder stringBuilder = new StringBuilder(codeTemplate);
        stringBuilder.Replace("__CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.ff"));
        stringBuilder.Replace("__NAME_SPACE__", nameSpace);
        stringBuilder.Replace("__CLASS_NAME__", className);
        string field, getField;
        GenerateComponentField(fieldTypeDict, out field, out getField);
        stringBuilder.Replace("__FIELD__", field);
        stringBuilder.Replace("__GET_FIELD__", getField);

        SaveFile(filePath, stringBuilder.ToString());
    }

    public void GenerateBehaviourCode(string filePath, string codeTemplate, string nameSpace, string className, string path)
    {
        if (string.IsNullOrEmpty(codeTemplate))
        {
            Debug.LogError("Component behaviour code template file is invalid.");
            return;
        }

        StringBuilder stringBuilder = new StringBuilder(codeTemplate);

        stringBuilder.Replace("__NAME_SPACE__", nameSpace);
        stringBuilder.Replace("__CLASS_NAME__", className);
        stringBuilder.Replace("__PATH__", path);

        SaveFile(filePath, stringBuilder.ToString());
    }

    private void GenerateComponentField(Dictionary<string, string> fieldTypeDict, out string field, out string getField)
    {
        StringBuilder fieldBuilder = new StringBuilder();
        StringBuilder getFieldBuilder = new StringBuilder();
        int index = 0;
        foreach (var pair in fieldTypeDict)
        {
            fieldBuilder.AppendLine(string.Format("        private {0} {1};", pair.Value, pair.Key));
            getFieldBuilder.AppendLine(string.Format("            {0} = collection.GetComponent<{1}>({2});", pair.Key, pair.Value, index++));
        }

        // 去除最后的换行符
        field = fieldBuilder.ToString().TrimEnd();
        getField = getFieldBuilder.ToString().TrimEnd();
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
