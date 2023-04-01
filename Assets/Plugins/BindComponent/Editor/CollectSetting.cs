using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectSetting", menuName = "BindComponent/Collect Setting", order = 1)]
public class CollectSetting : ScriptableObject
{
    [Serializable]
    public class ComponentType
    {
        public string typeKey;
        public string typeName;
        public ComponentType(string typeKey, string typeName)
        {
            this.typeKey = typeKey;
            this.typeName = typeName;
        }
    }

    [Space(4)]
    [Header("Collect")]
    [Tooltip("收集代码名称")]
    public string _collectorCodeName = "Collect";
    [Tooltip("组件类型映射列表")]
    public List<ComponentType> _componentList = new List<ComponentType>
        {
            new ComponentType("Tran", "Transform"),
            new ComponentType("Rect", "RectTransform"),

            new ComponentType("Canvas", "Canvas"),
            new ComponentType("CGroup", "CanvasGroup"),

            new ComponentType("Anim", "Animation"),
            new ComponentType("Animator", "Animator"),

            new ComponentType("Text", "Text"),
            new ComponentType("Image", "Image"),
            new ComponentType("RawImage", "RawImage"),
            new ComponentType("Mask", "Mask"),
            new ComponentType("RectMask", "RectMask2D"),

            new ComponentType("Button", "Button"),
            new ComponentType("Toggle", "Toggle"),
            new ComponentType("TGroup", "ToggleGroup"),
            new ComponentType("Slider", "Slider"),
            new ComponentType("Scrollbar", "Scrollbar"),
            new ComponentType("Dropdown", "Dropdown"),
            new ComponentType("InputField", "InputField"),
            new ComponentType("ScrollRect", "ScrollRect"),

            new ComponentType("HLGroup", "HorizontalLayoutGroup"),
            new ComponentType("VLGroup", "VerticalLayoutGroup"),
            new ComponentType("GLGroup", "GridLayoutGroup"),

            new ComponentType("TMP","TMP_Text")

        };
    [Tooltip("默认字段前缀")]
    public string _fieldNamePrefix = "_";
    [Tooltip("字段名是否使用组件类型 默认值")]
    public bool _fieldNameUseType = false;
    private Dictionary<string, string> _componentDic;

    [Space(4)]
    [Header("Generate")]
    [Tooltip("Generate")]
    public string _generatorCodeName = "Generate";
    [Tooltip("默认命名空间")]
    public string _namespace = "UFO";
    [Tooltip("默认代码保存地址")]
    public string _codeSavePath = "Assets/Scripts/View/Temp";
    [Tooltip("组件代码模板")]
    public TextAsset _componentCodeTemp;
    [Tooltip("MonoBehaviour代码模板")]
    public TextAsset _behaviourCodeTemp;
    [Space(4)]
    [Header("Extension")]
    [Tooltip("收集代码模板")]
    public TextAsset _collectionExtensionCodeTemp;

    public Regex _defaultNameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
    public Regex _fieldNameRegex = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$");

    public Dictionary<string, string> componentMapDict
    {
        get
        {
            if (_componentDic == null)
            {
                _componentDic = new Dictionary<string, string>();
                for (int i = _componentList.Count - 1; i >= 0; i--)
                {
                    ComponentType map = _componentList[i];
                    if (_componentDic.ContainsKey(map.typeKey))
                    {
                        _componentList.RemoveAt(i);
                        continue;
                    }
                    _componentDic.Add(map.typeKey, map.typeName);
                }
            }

            return _componentDic;
        }
    }
}
