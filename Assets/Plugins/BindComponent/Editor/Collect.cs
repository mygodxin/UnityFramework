using System.Collections.Generic;
using UnityEngine;

//收集组件
public class Collect : ICollect
{
    private const char ComponentSeparator = '_';

    public Dictionary<string, Component> CollectComponentFields(Transform target, string fieldNamePrefix, bool fieldNameByType, Dictionary<string, string> componentMapDict)
    {
        Dictionary<string, Component> fieldComponentDict = new Dictionary<string, Component>();
        var children = target.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            CollectComponentField(child, fieldComponentDict, fieldNamePrefix, fieldNameByType, componentMapDict);
        }
        return fieldComponentDict;
    }

    private void CollectComponentField(Transform target, Dictionary<string, Component> fieldComponentDict, string fieldNamePrefix, bool fieldNameByType, Dictionary<string, string> componentMapDict)
    {
        string[] splits = target.name.Split(ComponentSeparator);
        if (splits.Length <= 1)
        {
            return;
        }

        int nameIndex = splits.Length;
        for (int i = 1; i < nameIndex; i++)
        {
            string typeKey = splits[i];
            string typeName;
            if (!componentMapDict.TryGetValue(typeKey, out typeName))
            {
                Debug.LogErrorFormat("Component type key '{0}' has no mapping component type.", typeKey);
                continue;
            }

            Component component = target.GetComponent(typeName);
            if (component == null)
            {
                Debug.LogErrorFormat("Transform '{0}' has no component '{1}'.", target.name, typeName);
                continue;
            }

            string name = splits[0];
            name = LowerFirst(name);//string.IsNullOrEmpty(fieldNamePrefix) ? LowerFirst(name) : UpperFirst(name);
            string fieldName = string.Format("{0}{1}{2}", fieldNamePrefix, name, fieldNameByType ? typeName : typeKey);
            if (fieldComponentDict.ContainsKey(fieldName))
            {
                Debug.LogErrorFormat("Already exits the same field '{0}'. Please modify the name of transform '{1}'.", fieldName, target.name);
                continue;
            }

            fieldComponentDict.Add(fieldName, component);
        }
    }

    private string UpperFirst(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        if (str.Length <= 1)
        {
            return str.ToUpper();
        }
        return str.Substring(0, 1).ToUpper() + str.Substring(1);
    }

    private string LowerFirst(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        if (str.Length <= 1)
        {
            return str.ToLower();
        }
        return str.Substring(0, 1).ToLower() + str.Substring(1);
    }
}
