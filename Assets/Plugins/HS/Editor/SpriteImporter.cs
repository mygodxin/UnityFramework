using UnityEngine;
using UnityEditor;
using UnityEditor.U2D.Sprites;

/// <summary>
/// 导入图片自动设置为Sprite2D的属性
/// </summary>
class SpritePreprocessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
            return;
        // 检查.meta文件是否存在
        if (!assetImporter.importSettingsMissing)
        {
            return;
        }
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
    }
}