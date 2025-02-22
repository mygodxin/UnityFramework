using HS;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 压缩TextMeshPro字体
/// </summary>
public class CompressFont : Editor
{
    [MenuItem("Tools/CompressFont")]
    public static void Compress()
    {
        ExtractTexture("Assets/TextMesh Pro/Resources/Fonts & Materials/Tensentype/Tensentype SDF.asset");
    }

    public static void ExtractTexture(string fontPath)
    {
        string texturePath = fontPath.Replace(".asset", ".png");
        TMP_FontAsset targeFontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontPath.Replace(Application.dataPath, "Assets"));
        Texture2D texture2D = new Texture2D(targeFontAsset.atlasTexture.width, targeFontAsset.atlasTexture.height, TextureFormat.Alpha8, false);
        Graphics.CopyTexture(targeFontAsset.atlasTexture, texture2D);
        byte[] dataBytes = texture2D.EncodeToPNG();
        FileStream fs = File.Open(texturePath, FileMode.OpenOrCreate);
        fs.Write(dataBytes, 0, dataBytes.Length);
        fs.Flush();
        fs.Close();
        AssetDatabase.Refresh();
        Texture2D atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath.Replace(Application.dataPath, "Assets"));

        //替换所有用到的该字体的材质
        var matPresets = FindMaterialReferences(targeFontAsset);
        targeFontAsset.material.SetTexture(ShaderUtilities.ID_MainTex, atlas);
        foreach (var mat in matPresets)
        {
            mat.SetTexture(ShaderUtilities.ID_MainTex, atlas);
        }

        AssetDatabase.RemoveObjectFromAsset(targeFontAsset.atlasTexture);
        targeFontAsset.atlasTextures[0] = atlas;
        targeFontAsset.material.mainTexture = atlas;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static Material[] FindMaterialReferences(TMP_FontAsset fontAsset)
    {
        List<Material> refs = new List<Material>();
        Material mat = fontAsset.material;
        refs.Add(mat);

        // Get materials matching the search pattern.
        string searchPattern = "t:Material" + " " + fontAsset.name.Split(new char[] { ' ' })[0];
        string[] materialAssetGUIDs = AssetDatabase.FindAssets(searchPattern);

        for (int i = 0; i < materialAssetGUIDs.Length; i++)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(materialAssetGUIDs[i]);
            Material targetMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (targetMaterial.HasProperty(ShaderUtilities.ID_MainTex) && targetMaterial.GetTexture(ShaderUtilities.ID_MainTex) != null && mat.GetTexture(ShaderUtilities.ID_MainTex) != null && targetMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == mat.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
            {
                if (!refs.Contains(targetMaterial))
                    refs.Add(targetMaterial);
            }
            else
            {
                // TODO: Find a more efficient method to unload resources.
                //Resources.UnloadAsset(targetMaterial.GetTexture(ShaderUtilities.ID_MainTex));
            }
        }

        return refs.ToArray();
    }
}