using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


[CreateAssetMenu(fileName = "CollectSetting", menuName = "BindComponent/Collect Setting", order = 1)]
public class CollectSetting : ScriptableObject
{
    [Serializable]
    public class ComponentTypeMap
    {
        public string typeKey;
        public string typeName;

        public ComponentTypeMap(string typeKey, string typeName)
        {
            this.typeKey = typeKey;
            this.typeName = typeName;
        }
    }

    [Space(4)]
    [Header("Collect")]
    [Tooltip("Collect")]
    public string _collectorTypeName = "Collect";
    [Tooltip("组件类型映射列表。")]
    public List<ComponentTypeMap> _componentMaps = new List<ComponentTypeMap>
        {
            // Transform
            new ComponentTypeMap("Tran", "Transform"),
            new ComponentTypeMap("Rect", "RectTransform"),

            // Animation
            new ComponentTypeMap("Anim", "Animation"),
            new ComponentTypeMap("Animator", "Animator"),

            // Graphic
            new ComponentTypeMap("Text", "Text"),
            new ComponentTypeMap("Image", "Image"),
            new ComponentTypeMap("RawImage", "RawImage"),

            // Controls
            new ComponentTypeMap("Button", "Button"),
            new ComponentTypeMap("Toggle", "Toggle"),
            new ComponentTypeMap("TGroup", "ToggleGroup"),
            new ComponentTypeMap("Slider", "Slider"),
            new ComponentTypeMap("Scrollbar", "Scrollbar"),
            new ComponentTypeMap("Dropdown", "Dropdown"),
            new ComponentTypeMap("InputField", "InputField"),

            // Container
            new ComponentTypeMap("Canvas", "Canvas"),
            new ComponentTypeMap("ScrollView", "ScrollRect"),
            new ComponentTypeMap("CGroup", "CanvasGroup"),
            new ComponentTypeMap("GLGroup", "GridLayoutGroup"),
            new ComponentTypeMap("VLGroup", "VerticalLayoutGroup"),
            new ComponentTypeMap("HLGroup", "HorizontalLayoutGroup"),

            // Mask
            new ComponentTypeMap("Mask", "Mask"),
            new ComponentTypeMap("RectMask", "RectMask2D"),
        };
    [Tooltip("默认字段前缀。")]
    public string m_DefaultFieldNamePrefix = "_";
    [Tooltip("字段名是否使用组件类型 默认值。")]
    public bool m_DefaultFieldNameByType = false;
    // 组件类型映射字典。
    private Dictionary<string, string> _componentMapDict;

    [Space(4)]
    [Header("Generate")]
    [Tooltip("Generate")]
    public string _generatorTypeName = "Generate";
    [Tooltip("默认命名空间")]
    public string _defaultNameSpace = "UnityFramework";
    [Tooltip("默认代码保存地址")]
    public string _defaultCodeSavePath = "Assets/UICode";
    [Tooltip("代码模板")]
    public TextAsset _componentsCodeTemplate;
    [Tooltip("代码模板")]
    public TextAsset _behaviourCodeTemplate;
    [Space(4)]
    [Header("Extension")]
    [Tooltip("代码模板")]
    public TextAsset _collectionExtensionCodeTemplate;

    // 命名规范正则表达式
    public Regex _defaultNameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
    public Regex _fieldNameRegex = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$");

    /// <summary>
    /// 组件类型映射字典。
    /// </summary>
    public Dictionary<string, string> componentMapDict
    {
        get
        {
            if (_componentMapDict == null)
            {
                _componentMapDict = new Dictionary<string, string>();
                for (int i = _componentMaps.Count - 1; i >= 0; i--)
                {
                    ComponentTypeMap map = _componentMaps[i];
                    if (_componentMapDict.ContainsKey(map.typeKey))
                    {
                        _componentMaps.RemoveAt(i);
                        continue;
                    }
                    _componentMapDict.Add(map.typeKey, map.typeName);
                }
            }

            return _componentMapDict;
        }
    }
}
