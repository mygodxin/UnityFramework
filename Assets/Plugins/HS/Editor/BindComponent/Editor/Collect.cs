using System.Collections.Generic;
using UnityEngine;

//收集组件
public class Collect
{
    private const char ComponentSeparator = '_';

    public Dictionary<string, Component> CollectComponentFields(Transform target, CollectSetting setting)
    {
        string fieldNamePrefix = setting.FieldNamePrefix;
        bool fieldNameByType = setting.FieldNameUseType;
        Dictionary<string, string> componentMapDict = setting.ComponentMapDict;
        string excludeName = setting.ExcludeName;

        Dictionary<string, Component> fieldComponentDict = new Dictionary<string, Component>();
        var children = target.GetComponentsInChildren<Transform>(true);

        //剔除排除项的组件
        var excludes = new List<Transform>();
        foreach (var child in children)
        {
            if (target.name != child.name && child.name.IndexOf(excludeName) >= 0)
            {
                excludes.Add(child);
            }
        }

        foreach (var child in children)
        {
            if (child.name == target.name)
            {
                continue;
            }
            //排除
            var exclude = false;
            foreach (var c in excludes)
            {
                if (child.IsChildOf(c) && c.name != child.name)
                {
                    exclude = true;
                    break;
                }
            }
            if (exclude)
                continue;
            //分割名字
            string[] splits = child.name.Split(ComponentSeparator);
            if (splits.Length <= 1)
            {
                //Debug.LogErrorFormat("Child name split fail.Please check child name.", child.name);
                continue;
            }

            int nameIndex = splits.Length;
            for (int i = 1; i < nameIndex; i++)
            {
                string typeKey = splits[i];
                string typeName;
                if (!componentMapDict.TryGetValue(typeKey, out typeName))
                {
                    Debug.LogErrorFormat("Component Type key '{0}' has no mapping component Type.", typeKey);
                    continue;
                }

                Component component = child.GetComponent(typeName);
                if (component == null)
                {
                    Debug.LogErrorFormat("Transform '{0}' has no component '{1}'.", child.name, typeName);
                    continue;
                }

                string name = splits[0];
                name = UpperFirst(name);//string.IsNullOrEmpty(FieldNamePrefix) ? LowerFirst(name) : UpperFirst(name);
                string fieldName = string.Format("{0}{1}{2}", fieldNamePrefix, name, fieldNameByType ? typeName : typeKey);
                if (fieldComponentDict.ContainsKey(fieldName))
                {
                    Debug.LogErrorFormat("Already exits the same field '{0}'. Please modify the name of transform '{1}'.", fieldName, child.name);
                    continue;
                }

                fieldComponentDict.Add(fieldName, component);
            }
        }
        return fieldComponentDict;
    }

    /// <summary>
    /// 首字母大写
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 首字母小写
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
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
