using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

//依赖分析
public class DependAnalysis : EditorWindow
{
    private static List<Object> selectedObjects;
    private static bool[] foldoutArr;
    private static Object[][] reverseDepends;
    private static Vector2 scrollPos;
    private static readonly string[] WithoutExtensions = new string[] { ".prefab", ".unity", ".mat", ".asset", ".controller" };

    [MenuItem("Assets/DependAnalysis")]
    private static void FindReferences()
    {

        selectedObjects = GetSelectedObjects();
        var len = selectedObjects.Count;
        reverseDepends = new Object[len][];
        foldoutArr = new bool[len];
        EditorStyles.foldout.richText = true;
        for (int i = 0; i < len; i++)
        {
            reverseDepends[i] = GetReverseDepend(selectedObjects[i]);
        }
        DependAnalysis window = GetWindow<DependAnalysis>("DependAnalysis");
        window.Show();
    }

    private static List<Object> GetObjectsInFolder(string folderPath)
    {
        string[] assetGUIDs = AssetDatabase.FindAssets("t:Object", new string[] { folderPath });
        List<Object> objectsInFolder = new List<Object>();

        for (int i = 0; i < assetGUIDs.Length; i++)
        {

            string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                //objectsInFolder.AddRange(GetObjectsInFolder(assetPath));
            }
            else
                objectsInFolder.Add(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
        }

        return objectsInFolder;
    }

    private static List<Object> GetSelectedObjects()
    {
        List<Object> selectedObjects = new List<Object>();

        var objects = Selection.GetFiltered<Object>(SelectionMode.Assets);
        foreach (Object obj in objects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                selectedObjects.AddRange(GetObjectsInFolder(assetPath));
            }
            else
            {
                selectedObjects.Add(obj);
            }
        }

        return selectedObjects;
    }

    private void OnGUI()
    {
        if (selectedObjects == null) return;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        Object[] objArr;
        int count;
        string objName;
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            objArr = reverseDepends[i];
            count = objArr == null ? 0 : objArr.Length;
            var assetPath = AssetDatabase.GetAssetPath(selectedObjects[i]);
            objName = Path.GetFileName(assetPath);
            string info = count == 0 ? $"<color=red>{objName}[{count}]</color>" : $"{objName}[{count}]";

            foldoutArr[i] = EditorGUILayout.Foldout(foldoutArr[i], info, true);
            if (foldoutArr[i])
            {
                if (GUILayout.Button("Highlight in project"))
                {
                    HighlightDependency(assetPath);
                }
                if (count > 0)
                {
                    foreach (var obj in objArr)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.ObjectField(obj, typeof(Object), true);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void HighlightDependency(string dependencyPath)
    {
        Object dependencyObject = AssetDatabase.LoadMainAssetAtPath(dependencyPath);
        if (dependencyObject != null)
        {
            Selection.activeObject = dependencyObject;
            EditorGUIUtility.PingObject(dependencyObject);
        }
    }

    private static Object[] GetReverseDepend(Object target)
    {
        if (target == null) return null;
        string path = AssetDatabase.GetAssetPath(target);
        if (string.IsNullOrEmpty(path)) return null;
        string guid = AssetDatabase.AssetPathToGUID(path);
        string[] files = Directory.GetFiles(Application.dataPath, "*",
            SearchOption.AllDirectories).Where(s => WithoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        List<Object> objects = new List<Object>();
        foreach (var file in files)
        {
            string assetPath = file.Replace(Application.dataPath, "");
            assetPath = "Assets" + assetPath;
            string readText = File.ReadAllText(file);

            if (!readText.StartsWith("%YAML"))
            {
                var depends = AssetDatabase.GetDependencies(assetPath, false);
                if (depends != null)
                {
                    foreach (var dep in depends)
                    {
                        if (dep == path)
                        {
                            objects.Add(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
                            break;
                        }
                    }
                }
            }
            else if (Regex.IsMatch(readText, guid)) objects.Add(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
        }
        return objects.ToArray();
    }

    private void OnDestroy()
    {
        reverseDepends = null;
        foldoutArr = null;
    }
}
