using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BindComponent")]
[DisallowMultipleComponent]
public sealed class BindComponent : MonoBehaviour
{
#if UNITY_EDITOR
    // 是否已设置过默认值
#pragma warning disable 0414
    [SerializeField] private bool _setup;

    // Collect settings
    [SerializeField] 
    private string _collectorTypeName;
    [SerializeField] 
    private string _fieldNamePrefix;
    [SerializeField] 
    private bool _fieldNameByType;
    [SerializeField] 
    private List<string> _fieldNames;
    [SerializeField] 
    private List<Component> _fieldComponents;

    // Generate settings
    [SerializeField] 
    private string _generatorTypeName;
    [SerializeField] 
    private string _nameSpace;
    [SerializeField] 
    private string _className;
    [SerializeField] 
    private string _codeSavePath;

    private void Reset()
    {
        _setup = false;
    }
#endif

    [SerializeField] private List<Component> _components;

    public T GetComponent<T>(int index) where T : Component
    {
        if (index < 0 || index >= _components.Count)
        {
            Debug.LogError("Get component failed with invalid index.");
            return null;
        }

        T component = _components[index] as T;
        if (component == null)
        {
            Debug.LogErrorFormat("Get component failed with invalid type, index = {0}.", index);
            return null;
        }

        return component;
    }
}
