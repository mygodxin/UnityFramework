using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(BindComponent))]
public class CollectInspector : Editor
{
    private const string NoneOptionName = "<None>";
    private readonly string[] _fieldNameRule = { "TypeKey", "TypeName" };

    private int _exitsSettingsCount;
    private CollectSetting _settings;

    private SerializedProperty _collectCodeName;
    private SerializedProperty _fieldNamePrefix;
    private SerializedProperty _fieldNameUseType;
    private SerializedProperty _fieldNames;
    private SerializedProperty _fieldComponents;
    private string[] _collectTypeNames;
    private int _collectTypeNameIndex;
    private ICollect _collector;
    private int _fieldNameRuleIndex;
    private bool _showComponentField;

    private SerializedProperty _generateTypeName;
    private SerializedProperty _nameSpace;
    private SerializedProperty _className;
    private SerializedProperty _codeSavePath;
    private string[] _generateTypeNames;
    private int _generateTypeNameIndex;
    private IGenerate _generate;

    private SerializedProperty _setup;

    private SerializedProperty _components;

    private ICollect collect
    {
        get
        {
            if (_collector == null)
            {
                string collectorTypeName = _collectCodeName.stringValue;
                if (string.IsNullOrEmpty(collectorTypeName))
                {
                    Debug.LogError("Collector is invalid.");
                    return null;
                }

                Type collectorType = TypeUtil.GetEditorType(collectorTypeName);
                if (collectorType == null)
                {
                    Debug.LogErrorFormat("Can not get collector type '{0}'.", collectorTypeName);
                    return null;
                }

                _collector = (Collect)Activator.CreateInstance(collectorType);
                if (_collector == null)
                {
                    Debug.LogErrorFormat("Can not create collector instance '{0}'.", collectorTypeName);
                    return null;
                }
            }

            return _collector;
        }
    }

    private IGenerate generate
    {
        get
        {
            if (_generate == null)
            {
                string generatorTypeName = _generateTypeName.stringValue;
                if (string.IsNullOrEmpty(generatorTypeName))
                {
                    Debug.LogError("Generator is invalid.");
                    return null;
                }

                Type generatorType = TypeUtil.GetEditorType(generatorTypeName);
                if (generatorType == null)
                {
                    Debug.LogErrorFormat("Can not get generator type '{0}'.", generatorTypeName);
                    return null;
                }

                _generate = (IGenerate)Activator.CreateInstance(generatorType);
                if (_generate == null)
                {
                    Debug.LogErrorFormat("Can not create generator instance '{0}'.", generatorTypeName);
                    return null;
                }
            }

            return _generate;
        }
    }

    private void OnEnable()
    {
        string[] paths = AssetDatabase.FindAssets("t:CollectSetting");
        _exitsSettingsCount = paths.Length;
        if (_exitsSettingsCount != 1)
        {
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(paths[0]);
        _settings = AssetDatabase.LoadAssetAtPath<CollectSetting>(path);

        InitSerializeData();
        _showComponentField = true;
    }

    public override void OnInspectorGUI()
    {
        if (_settings == null)
        {
            DrawSettingsGUI();
            return;
        }

        serializedObject.Update();

        DrawCollectGUI();
        DrawGenerateGUI();

        if (_setup.boolValue == false)
        {
            SetupDefaultValue();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void InitSerializeData()
    {
        _setup = serializedObject.FindProperty("_setup");

        _collectCodeName = serializedObject.FindProperty("_collectorTypeName");
        _fieldNamePrefix = serializedObject.FindProperty("_fieldNamePrefix");
        _fieldNameUseType = serializedObject.FindProperty("_fieldNameByType");
        _fieldNames = serializedObject.FindProperty("_fieldNames");
        _fieldComponents = serializedObject.FindProperty("_fieldComponents");

        List<string> collectorTypeNames = new List<string> { NoneOptionName };
        collectorTypeNames.AddRange(TypeUtil.GetEditorTypeNames(typeof(ICollect)));
        _collectTypeNames = collectorTypeNames.ToArray();

        _collectTypeNameIndex = 0;
        if (!string.IsNullOrEmpty(_collectCodeName.stringValue))
        {
            _collectTypeNameIndex = collectorTypeNames.IndexOf(_collectCodeName.stringValue);
            if (_collectTypeNameIndex <= 0)
            {
                _collectTypeNameIndex = 0;
                _collectCodeName.stringValue = null;
                _collector = null;
            }
        }
        collectorTypeNames.Clear();

        _generateTypeName = serializedObject.FindProperty("_generatorTypeName");
        _nameSpace = serializedObject.FindProperty("_nameSpace");
        _className = serializedObject.FindProperty("_className");
        _codeSavePath = serializedObject.FindProperty("_codeSavePath");

        List<string> generatorTypeNames = new List<string> { NoneOptionName };
        generatorTypeNames.AddRange(TypeUtil.GetEditorTypeNames(typeof(IGenerate)));
        _generateTypeNames = generatorTypeNames.ToArray();

        _generateTypeNameIndex = 0;
        if (!string.IsNullOrEmpty(_generateTypeName.stringValue))
        {
            _generateTypeNameIndex = generatorTypeNames.IndexOf(_generateTypeName.stringValue);
            if (_generateTypeNameIndex <= 0)
            {
                _generateTypeNameIndex = 0;
                _generateTypeName.stringValue = null;
                _generate = null;
            }
        }
        generatorTypeNames.Clear();

        _fieldNameRuleIndex = _fieldNameUseType.boolValue ? 1 : 0;

        _components = serializedObject.FindProperty("_components");
    }

    private void SetupDefaultValue()
    {
        _setup.boolValue = true;

        _collectCodeName.stringValue = _settings._collectorCodeName;
        _fieldNamePrefix.stringValue = _settings._fieldNamePrefix;
        _fieldNameUseType.boolValue = _settings._fieldNameUseType;

        _generateTypeName.stringValue = _settings._generatorCodeName;
        _nameSpace.stringValue = _settings._namespace;
        _className.stringValue = serializedObject.targetObject.name;
        _codeSavePath.stringValue = _settings._codeSavePath;

        List<string> temp = new List<string>(_collectTypeNames);
        int collectorIndex = temp.IndexOf(_settings._collectorCodeName);
        _collectTypeNameIndex = Mathf.Max(0, collectorIndex);
        temp = new List<string>(_generateTypeNames);
        int generatorIndex = temp.IndexOf(_settings._generatorCodeName);
        _generateTypeNameIndex = Mathf.Max(0, generatorIndex);
        temp.Clear();

        _fieldNameRuleIndex = _fieldNameUseType.boolValue ? 1 : 0;
    }

    private void DrawSettingsGUI()
    {
        if (_exitsSettingsCount > 1)
        {
            EditorGUILayout.HelpBox("Multiple 'CollectSetting' asset exist. Please delete the redundant assets.", MessageType.Warning);
            return;
        }

        EditorGUILayout.HelpBox("Need a CollectSetting asset, Please create one.", MessageType.Error);
        if (GUILayout.Button("Create"))
        {
            _settings = CreateInstance<CollectSetting>();
            AssetDatabase.CreateAsset(_settings, "Assets/CollectSetting.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            InitSerializeData();
        }
    }

    private void DrawCollectGUI()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            GUILayout.Label("Collect Settings", "BoldLabel");

            int collectorTypeNameIndex = EditorGUILayout.Popup("Collector", _collectTypeNameIndex, _collectTypeNames);
            if (collectorTypeNameIndex != _collectTypeNameIndex)
            {
                _collectTypeNameIndex = collectorTypeNameIndex;
                _collectCodeName.stringValue = _collectTypeNameIndex <= 0 ? null : _collectTypeNames[_collectTypeNameIndex];
                _collector = null;
            }

            EditorGUILayout.BeginHorizontal();
            {
                _fieldNamePrefix.stringValue = EditorGUILayout.TextField("Field Name Rule", _fieldNamePrefix.stringValue);
                int index = EditorGUILayout.Popup(_fieldNameRuleIndex, _fieldNameRule);
                if (_fieldNameRuleIndex != index)
                {
                    _fieldNameRuleIndex = index;
                    _fieldNameUseType.boolValue = _fieldNameRuleIndex == 1;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            _showComponentField = EditorGUILayout.Foldout(_showComponentField, "Components", true);
            EditorGUI.indentLevel--;
            if (_showComponentField)
            {
                int deleteIndex = -1;
                for (int i = 0; i < _fieldNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("[{0}]", i), GUILayout.Width(30));
                        SerializedProperty fieldName = _fieldNames.GetArrayElementAtIndex(i);
                        SerializedProperty component = _fieldComponents.GetArrayElementAtIndex(i);
                        fieldName.stringValue = EditorGUILayout.TextField(fieldName.stringValue);
                        component.objectReferenceValue = EditorGUILayout.ObjectField(component.objectReferenceValue, typeof(Component), true);
                        if (GUILayout.Button("", "OL Minus"))
                        {
                            deleteIndex = i;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(4);
                }
                if (deleteIndex != -1)
                {
                    SerializedArrayDeleteIndex(_fieldNames, deleteIndex);
                    SerializedArrayDeleteIndex(_fieldComponents, deleteIndex);
                }
            }

            if (GUILayout.Button("Collect Components To Update"))
            {
                CollectComponentFieldsToUpdate();
                SyncSerializeComponents();
            }

            if (GUILayout.Button("Collect Components To Add"))
            {
                CollectComponentFieldsToAdd();
                SyncSerializeComponents();
            }

            if (GUILayout.Button("Remove Null Component"))
            {
                RemoveNullComponent();
                SyncSerializeComponents();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawGenerateGUI()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            GUILayout.Label("Generate Settings", "BoldLabel");

            int generatorTypeNameIndex = EditorGUILayout.Popup("Generator", _generateTypeNameIndex, _generateTypeNames);
            if (generatorTypeNameIndex != _generateTypeNameIndex)
            {
                _generateTypeNameIndex = generatorTypeNameIndex;
                _generateTypeName.stringValue = _generateTypeNameIndex <= 0 ? null : _generateTypeNames[_generateTypeNameIndex];
                _generate = null;
            }

            EditorGUILayout.BeginHorizontal();
            {
                _nameSpace.stringValue = EditorGUILayout.TextField("Name Space", _nameSpace.stringValue);
                if (GUILayout.Button("Default", GUILayout.Width(60)))
                {
                    _nameSpace.stringValue = _settings._namespace;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                _className.stringValue = EditorGUILayout.TextField("Class Name", _className.stringValue);
                if (GUILayout.Button("Default", GUILayout.Width(60)))
                {
                    _className.stringValue = serializedObject.targetObject.name;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Code Save Path", _codeSavePath.stringValue);
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    string path = EditorUtility.SaveFolderPanel("Select Save Path", _codeSavePath.stringValue, string.Empty);
                    if (string.IsNullOrEmpty(path))
                    {
                        Debug.LogError("Select folder path is invalid.");
                    }
                    else
                    {
                        int index = path.IndexOf("Assets", StringComparison.Ordinal);
                        if (index < 0)
                        {
                            Debug.LogWarning("Suggest to save to any folder of 'Assets'.");
                        }
                        _codeSavePath.stringValue = path.Substring(index);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate Components Code"))
            {
                GenerateComponentsCode();
            }

            if (GUILayout.Button("Generate Behaviour Code"))
            {
                GenerateBehaviourCode();
            }

            if (GUILayout.Button("Add Behaviour Component"))
            {
                AddBehaviourComponent();
            }
        }
        EditorGUILayout.EndVertical();
    }

    #region Collect

    private void CollectComponentFieldsToUpdate()
    {
        if (collect == null)
        {
            return;
        }

        _fieldNames.ClearArray();
        _fieldComponents.ClearArray();
        Transform transform = ((Component)target).transform;
        Dictionary<string, Component> fieldComponentDict = collect.CollectComponentFields(transform, _fieldNamePrefix.stringValue, _fieldNameUseType.boolValue, _settings.componentMapDict);
        foreach (var pair in fieldComponentDict)
        {
            CollectComponentField(pair.Key, pair.Value);
        }
    }

    private void CollectComponentFieldsToAdd()
    {
        if (collect == null)
        {
            return;
        }

        Transform transform = ((Component)target).transform;
        Dictionary<string, Component> fieldComponentDict = collect.CollectComponentFields(transform, _fieldNamePrefix.stringValue, _fieldNameUseType.boolValue, _settings.componentMapDict);
        foreach (var pair in fieldComponentDict)
        {
            CollectComponentField(pair.Key, pair.Value);
        }

        List<int> adjectiveFieldIndex = new List<int>();
        List<string> cacheFieldNames = new List<string>();
        for (int i = 0; i < _fieldNames.arraySize; i++)
        {
            string fieldName = _fieldNames.GetArrayElementAtIndex(i).stringValue;
            if (cacheFieldNames.Contains(fieldName))
            {
                adjectiveFieldIndex.Add(i);
                continue;
            }
            cacheFieldNames.Add(fieldName);
        }
        cacheFieldNames.Clear();

        for (int i = adjectiveFieldIndex.Count - 1; i >= 0; i--)
        {
            int adjectiveIndex = adjectiveFieldIndex[i];
            SerializedArrayDeleteIndex(_fieldNames, adjectiveIndex);
            SerializedArrayDeleteIndex(_fieldComponents, adjectiveIndex);
        }
        adjectiveFieldIndex.Clear();
    }

    private void CollectComponentField(string fieldName, Component component)
    {
        int index = _fieldNames.arraySize;
        _fieldNames.InsertArrayElementAtIndex(index);
        _fieldNames.GetArrayElementAtIndex(index).stringValue = fieldName;
        _fieldComponents.InsertArrayElementAtIndex(index);
        _fieldComponents.GetArrayElementAtIndex(index).objectReferenceValue = component;
    }

    private void RemoveNullComponent()
    {
        for (int i = _fieldNames.arraySize - 1; i >= 0; i--)
        {
            Component component = (Component)_fieldComponents.GetArrayElementAtIndex(i).objectReferenceValue;
            if (component == null)
            {
                SerializedArrayDeleteIndex(_fieldNames, i);
                SerializedArrayDeleteIndex(_fieldComponents, i);
            }
        }
    }

    private void SerializedArrayDeleteIndex(SerializedProperty serializedProperty, int index)
    {
        int originLength = serializedProperty.arraySize;
        if (originLength <= index)
        {
            return;
        }

        // 序列化的引用类型，第一次删除时会将该引用置空，第二次才会将索引移除。所以在这里检查一下。
        // 序列化 string 类型会被认为是值类型，可以直接移除。
        serializedProperty.DeleteArrayElementAtIndex(index);
        if (originLength == serializedProperty.arraySize)
        {
            serializedProperty.DeleteArrayElementAtIndex(index);
        }
    }

    private void SyncSerializeComponents()
    {
        _components.ClearArray();
        for (int i = 0; i < _fieldComponents.arraySize; i++)
        {
            _components.InsertArrayElementAtIndex(i);
            _components.GetArrayElementAtIndex(i).objectReferenceValue = _fieldComponents.GetArrayElementAtIndex(i).objectReferenceValue;
        }
    }

    #endregion

    #region Generate

    private bool CheckGenerateCondition()
    {
        string savePath = _codeSavePath.stringValue;
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("Code save path is invalid.");
            return false;
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        string nameSpace = _nameSpace.stringValue;
        if (string.IsNullOrEmpty(nameSpace) || !_settings._defaultNameRegex.IsMatch(nameSpace))
        {
            Debug.LogErrorFormat("NameSpace '{0}' is invalid.", nameSpace);
            return false;
        }

        string className = _className.stringValue;
        if (string.IsNullOrEmpty(className) || !_settings._defaultNameRegex.IsMatch(className))
        {
            Debug.LogErrorFormat("Class name '{0}' is invalid.", className);
            return false;
        }

        return true;
    }

    private bool CheckGenerateFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return EditorUtility.DisplayDialog("File Exits", " File already exits, continue regenerate ?", "Continue", "Cancel");
        }
        return true;
    }

    private void GenerateComponentsCode()
    {
        if (generate == null)
        {
            Debug.LogError("Generator is null.");
            return;
        }

        if (!CheckGenerateCondition())
        {
            return;
        }

        if (_settings._componentCodeTemp == null)
        {
            Debug.LogError("ComponentsCodeTemplate is null, Please check 'CollectSetting' asset.");
            return;
        }

        string codeFileName = string.Format("{0}/{1}.Components.cs", _codeSavePath.stringValue, _className.stringValue);
        if (!CheckGenerateFile(codeFileName))
        {
            return;
        }

        RemoveNullComponent();
        SyncSerializeComponents();

        Dictionary<string, string> fieldTypeDict = new Dictionary<string, string>();
        for (int i = 0; i < _fieldNames.arraySize; i++)
        {
            string fieldName = _fieldNames.GetArrayElementAtIndex(i).stringValue;
            if (string.IsNullOrEmpty(fieldName) || !_settings._fieldNameRegex.IsMatch(fieldName))
            {
                Debug.LogErrorFormat("Field name '{0}' is invalid.", fieldName);
                continue;
            }

            string componentTypeName = _fieldComponents.GetArrayElementAtIndex(i).objectReferenceValue.GetType().Name;
            fieldTypeDict.Add(fieldName, componentTypeName);
        }

        generate.GenerateComponentsCode(codeFileName, _settings._componentCodeTemp.text, _nameSpace.stringValue, _className.stringValue, fieldTypeDict);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void GenerateBehaviourCode()
    {
        if (generate == null)
        {
            Debug.LogError("Generator is null.");
            return;
        }

        if (!CheckGenerateCondition())
        {
            return;
        }

        if (_settings._behaviourCodeTemp == null)
        {
            Debug.LogError("BehaviourCodeTemplate is null, Please check 'CollectSetting' asset.");
            return;
        }

        string codeFileName = string.Format("{0}/{1}.cs", _codeSavePath.stringValue, _className.stringValue);
        if (!CheckGenerateFile(codeFileName))
        {
            return;
        }

        generate.GenerateBehaviourCode(codeFileName, _settings._behaviourCodeTemp.text, _nameSpace.stringValue, _className.stringValue);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void AddBehaviourComponent()
    {
        string typeName = string.Format("{0}.{1}", _nameSpace.stringValue, _className.stringValue);

        Type componentType = TypeUtil.GetRuntimeType(typeName);
        if (componentType == null)
        {
            Debug.LogWarningFormat("Can't load type '{0}'. Please check  whether 'TypeUtility.RuntimeAssemblyNames' contains the assembly name of this type? ", typeName);
            return;
        }

        ((Component)target).gameObject.AddComponent(componentType);

        //标记为脏数据触发预制体保存
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        EditorUtility.ClearDirty(target);
    }

    #endregion
}
