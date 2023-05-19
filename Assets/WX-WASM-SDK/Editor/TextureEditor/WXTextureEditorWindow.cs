using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using LitJson;
using System;

namespace WeChatWASM
{


    public class WXReplaceTextureData
    {
        public string path;
        public int width;
        public int height;
        public string astc;
        public string limittype;
    }

    public class WXBundlePicDepsData
    {
        public string bundlePath;
        public List<WXReplaceTextureData> pics;
        public bool isCached;
    }

    public class WXFileCachedData
    {
        public string filePath;
        public string md5;
    }


    public class JSTextureTaskConf
    {
        public string dst;
        public string dataPath;
        public bool useDXT5;
        public List<WXReplaceTextureData> textureList;
    }

    public class JSTextureData
    {
        public string p;
        public int w;
        public int h;
    }

    public class WXTextureFileCacheScriptObject
    {
        public int Version;
        public DateTime UpdateTime;
        public int CostTimeInSeconds;
        public List<WXFileCachedData> cachedDatas = new List<WXFileCachedData>();
    }

    public class WXTextureReplacerScriptObject
    {
        public int Version;
        public DateTime UpdateTime;
        public List<WXBundlePicDepsData> bundlePicDeps = new List<WXBundlePicDepsData>();
    }

    public class WXTextureEditorWindow : EditorWindow
    {
        public static WXEditorScriptObject miniGameConf;

        [MenuItem("微信小游戏 / 包体瘦身--压缩纹理")]
        public static void Open()
        {
            miniGameConf = UnityUtil.GetEditorConf();
            var win = GetWindow(typeof(WXTextureEditorWindow), false, "包体瘦身--压缩纹理", true);//创建窗口
            win.minSize = new Vector2(600, 450);
            win.maxSize = new Vector2(600, 450);
            win.Show();
        }

        public static void Log(string type, string msg)
        {

            if (type == "Error")
            {
                UnityEngine.Debug.LogError(msg);
            }
            else if (type == "Log")
            {
                UnityEngine.Debug.Log(msg);
            }
            else if (type == "Warn")
            {
                UnityEngine.Debug.LogWarning(msg);
            }

        }

        private static WXTextureReplacerScriptObject GetTextureEditorCacheConf()
        {
            var BundlePicsFilePath = Path.Combine(GetDestDir(), "BundlePicsFile.json");
            string BundlePicsFileJson = "";
            if (File.Exists(BundlePicsFilePath))
            {
                using (FileStream fileStream = new FileStream(BundlePicsFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        BundlePicsFileJson = reader.ReadToEnd();
                    }
                }
            }
            WXTextureReplacerScriptObject wXTextureReplacerScriptObject = JsonMapper.ToObject<WXTextureReplacerScriptObject>(BundlePicsFileJson);
            Dictionary<string, List<WXReplaceTextureData>> cacheMap = new Dictionary<string, List<WXReplaceTextureData>>();
            if (wXTextureReplacerScriptObject == null)
                wXTextureReplacerScriptObject = new WXTextureReplacerScriptObject();
            if (wXTextureReplacerScriptObject.bundlePicDeps == null)
                wXTextureReplacerScriptObject.bundlePicDeps = new List<WXBundlePicDepsData>();
            return wXTextureReplacerScriptObject;
        }

        private void showToast(string content, bool err = false)
        {
            if (err)
            {
                UnityEngine.Debug.LogError(content);
            }
            else
            {
                UnityEngine.Debug.LogFormat(content);
            }
            ShowNotification(new GUIContent(content));
        }

        public static string GetDestDir()
        {
            var dstDir = miniGameConf.ProjectConf.DST + "/webgl-min";
            if (!string.IsNullOrEmpty(miniGameConf.CompressTexture.dstMinDir))
            {
                dstDir = miniGameConf.CompressTexture.dstMinDir;
            }
            return dstDir;
        }

        public static void ReplaceBundle()
        {
            Debug.Log("Start! 【" + System.DateTime.Now.ToString("T") + "】");
            WXAssetsTextTools.CompressText((result, msg) =>
            {
                if (result)
                {
                    Debug.Log("End! 【" + System.DateTime.Now.ToString("T") + "】");
                    Debug.Log("微信压缩纹理转换完成！");
                }
                else
                {
                    Debug.LogError(msg);
                }
            }, miniGameConf.CompressTexture.bundleDir, null, miniGameConf.CompressTexture.debugMode, miniGameConf.CompressTexture.force);
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(miniGameConf);
        }

        private void OnEnable()
        {
            miniGameConf = UnityUtil.GetEditorConf();
        }

        private void OnGUI()
        {

            var labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.fontSize = 14;

            labelStyle.margin.left = 20;
            labelStyle.margin.top = 10;
            labelStyle.margin.bottom = 10;

            GUILayout.Label("基本设置", labelStyle);

            var inputStyle = new GUIStyle(EditorStyles.textField);
            inputStyle.fontSize = 14;
            inputStyle.margin.left = 20;
            inputStyle.margin.bottom = 10;
            inputStyle.margin.right = 20;

            GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.margin.left = 20;
            toggleStyle.margin.right = 20;

            //miniGameConf.CompressTexture.bundleSuffix = EditorGUILayout.TextField(new GUIContent("bunlde文件后缀(?)", "多个不同后缀可用;分割开来"), miniGameConf.CompressTexture.bundleSuffix, inputStyle);


            GUILayout.Label(new GUIContent("bundle资源配置(?)", "可启用忽略、对ASCT格式进行bundle粒度的配置。注：忽略的bundle(被压缩过)将被强制还原为原始bundle"), labelStyle);

            GUIStyle pathButtonStyle0 = new GUIStyle(GUI.skin.button);
            pathButtonStyle0.fontSize = 12;
            pathButtonStyle0.margin.left = 20;

            GUILayout.BeginHorizontal();
            GUIStyle pathButtonStyle1 = new GUIStyle(GUI.skin.button);
            pathButtonStyle1.fontSize = 12;
            pathButtonStyle1.margin.left = 20;
            if (GUILayout.Button(new GUIContent("打开bundle配置面板"), pathButtonStyle1, GUILayout.Height(30), GUILayout.Width(150)))
            {
                var win2 = GetWindow(typeof(WXBundleSettingWindow), false, "Bundle配置面板", true);//创建窗口
                win2.minSize = new Vector2(680, 350);
                win2.Show();
            }
            GUILayout.EndHorizontal();

            GUILayout.Label(new GUIContent("自定义目录(?)", "默认不用选择"), labelStyle);

            var labelStyle3 = new GUIStyle(EditorStyles.boldLabel);
            labelStyle3.fontSize = 12;

            labelStyle3.margin.left = 20;
            labelStyle3.margin.top = 10;
            labelStyle3.margin.bottom = 10;

            GUILayout.Label("bundle路径", labelStyle3);

            var chooseBundlePathButtonClicked = false;
            var openBundleButtonClicked = false;
            var resetBundleButtonClicked = false;

            int pathButtonHeight = 28;
            GUIStyle pathLabelStyle = new GUIStyle(GUI.skin.textField);
            pathLabelStyle.fontSize = 12;
            pathLabelStyle.alignment = TextAnchor.MiddleLeft;
            pathLabelStyle.margin.top = 6;
            pathLabelStyle.margin.bottom = 6;
            pathLabelStyle.margin.left = 20;

            if (string.IsNullOrEmpty(miniGameConf.CompressTexture.bundleDir))
            {
                GUIStyle pathButtonStyle2 = new GUIStyle(GUI.skin.button);
                pathButtonStyle2.fontSize = 12;
                pathButtonStyle2.margin.left = 20;

                chooseBundlePathButtonClicked = GUILayout.Button("选择自定义bundle路径，默认不用选", pathButtonStyle2, GUILayout.Height(30), GUILayout.Width(300));
            }
            else
            {
                GUILayout.BeginHorizontal();
                // 路径框
                GUILayout.Label(miniGameConf.CompressTexture.bundleDir, pathLabelStyle, GUILayout.Height(pathButtonHeight - 6), GUILayout.ExpandWidth(true), GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 126));
                openBundleButtonClicked = GUILayout.Button("打开", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                resetBundleButtonClicked = GUILayout.Button("重选", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();


            if (chooseBundlePathButtonClicked)
            {
                // 弹出选目录窗口
                var dstPath = EditorUtility.SaveFolderPanel("选择你的bundle目录", "", "");

                if (dstPath != "")
                {
                    miniGameConf.CompressTexture.bundleDir = dstPath;
                }

            }

            if (openBundleButtonClicked)
            {
                UnityUtil.ShowInExplorer(miniGameConf.CompressTexture.bundleDir);
            }
            if (resetBundleButtonClicked)
            {
                miniGameConf.CompressTexture.bundleDir = "";
            }



            GUILayout.Label("自定义资源处理后存放路径", labelStyle3);

            var chooseDstPathButtonClicked = false;
            var openDstButtonClicked = false;
            var resetDstButtonClicked = false;


            if (string.IsNullOrEmpty(miniGameConf.CompressTexture.dstMinDir))
            {
                GUIStyle pathButtonStyle2 = new GUIStyle(GUI.skin.button);
                pathButtonStyle2.fontSize = 12;
                pathButtonStyle2.margin.left = 20;

                chooseDstPathButtonClicked = GUILayout.Button("选择自定义资源处理后存放路径，默认不用选", pathButtonStyle2, GUILayout.Height(30), GUILayout.Width(300));
            }
            else
            {
                GUILayout.BeginHorizontal();
                // 路径框
                GUILayout.Label(miniGameConf.CompressTexture.dstMinDir, pathLabelStyle, GUILayout.Height(pathButtonHeight - 6), GUILayout.ExpandWidth(true), GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 126));
                openDstButtonClicked = GUILayout.Button("打开", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                resetDstButtonClicked = GUILayout.Button("重选", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();


            if (chooseDstPathButtonClicked)
            {
                // 弹出选目录窗口
                var dstPath = EditorUtility.SaveFolderPanel("选择你的自定义资源处理后存放路径", "", "");

                if (dstPath != "")
                {
                    miniGameConf.CompressTexture.dstMinDir = dstPath;
                }

            }

            if (openDstButtonClicked)
            {
                UnityUtil.ShowInExplorer(miniGameConf.CompressTexture.dstMinDir);
            }
            if (resetDstButtonClicked)
            {
                miniGameConf.CompressTexture.dstMinDir = "";
            }

            GUILayout.Label(new GUIContent("功能选项(?)", ""), labelStyle);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(18));
            miniGameConf.CompressTexture.debugMode = GUILayout.Toggle(miniGameConf.CompressTexture.debugMode, "调试模式（仅生成ASTC）", GUILayout.Height(22), GUILayout.Width(170));
            miniGameConf.CompressTexture.debugMode = !GUILayout.Toggle(!miniGameConf.CompressTexture.debugMode, "全量模式（ASTC、DXT5、ETC2、PNG-min）", GUILayout.Height(22));
            miniGameConf.CompressTexture.parallelWithBundle = false;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(18));
            miniGameConf.CompressTexture.force = GUILayout.Toggle(miniGameConf.CompressTexture.force, "Force（默认采用增量处理，若资源异常，请强制全部重新生成）", GUILayout.Height(22));
            GUILayout.EndHorizontal();
            GUILayout.Label("操作", labelStyle);
            GUIStyle pathButtonStyle = new GUIStyle(GUI.skin.button);
            pathButtonStyle.fontSize = 12;
            pathButtonStyle.margin.left = 20;
            pathButtonStyle.margin.right = 20;

            EditorGUILayout.BeginHorizontal();


            var replaceTexture = GUILayout.Button(new GUIContent("处理资源(?)", "处理完成后会在导出目录生成webgl-min目录，bundle文件要换成使用webgl-min目录下的bundle文件，xx.webgl.data.unityweb.bin.txt文件也要换成使用webgl-min目录下对应的文件，注意要将导出目录里面Assets目录下的都上传至CDN对应路径，小游戏里才会显示成正常的压缩纹理。注意bundle文件不能开启crc校验，否则会展示异常。"), pathButtonStyle, GUILayout.Height(40), GUILayout.Width(140));


            var goReadMe = GUILayout.Button(new GUIContent("README"), pathButtonStyle, GUILayout.Height(40), GUILayout.Width(80));

            var exportLog = GUILayout.Button(new GUIContent("导出日志"), pathButtonStyle, GUILayout.Height(40), GUILayout.Width(80));

            EditorGUILayout.EndHorizontal();

            if (replaceTexture)
            {
                ReplaceBundle();
            }

            if (goReadMe)
            {
                EditorUtility.ClearProgressBar();
                Application.OpenURL("https://github.com/wechat-miniprogram/minigame-unity-webgl-transform/blob/main/Design/CompressedTexture.md");
            }

            if (exportLog)
            {
                WXAssetsTextTools.exportLastLog();
            }


        }

    }

}