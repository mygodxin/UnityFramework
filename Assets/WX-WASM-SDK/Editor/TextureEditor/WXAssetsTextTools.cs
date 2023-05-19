using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace WeChatWASM
{

    /// <summary>
    /// 微信团队提供的资源纹理处理工具，可对微信小游戏首包资源、AssetsBundle进行专项的纹理资源处理操作
    /// 详细说明请参阅[微信压缩纹理文档](https://github.com/wechat-miniprogram/minigame-unity-webgl-transform/blob/main/Design/CompressedTexture.md)
    /// </summary>
    public class WXAssetsTextTools
    {

        public static string NODE_PATH = ""; //如用户存在自定义Node.js客户端路径，可在此处进行自定义配置，必须为绝对路径。也可在调用本API前进行赋值 WXAssetsTextTools.NODE_PATH = $ ，默认无需配置

        /// <summary>
        /// 执行微信压缩纹理流程，对应【包体瘦身--压缩纹理】面板中「处理资源」按钮的执行，其中小游戏工程导出目录路径来自【转换小游戏】面板中配置。
        /// </summary>
        /// <param name="complete">流程执行结束后产生的回调Action，回调形参1(bool)为是否执行成功，形参2(string)异常时返回的错误提示信息</param>
        /// <param name="bundleDir">自定义bundle目录</param>
        /// <param name="outBundleDir">自定义bundle资源处理后的存放路径</param>
        /// <param name="debug">调试模式 true:仅生成ASTC false:全量模式(ASTC、DXT5、ETC2、PNG-min)</param>
        /// <param name="force">强制全部重新生成，默认采用增量模式</param>
        /// <param name="colorSpace">颜色空间 默认为 Gamma</param>
        public static void CompressText(Action<bool, string> complete = null, string bundleDir = null, string outBundleDir = null, bool debug = false, bool force = false)
        {
            WXEditorScriptObject miniGameConf = UnityUtil.GetEditorConf();
            string errmsg = "";
            string sourceDir = miniGameConf.ProjectConf.DST;
            if (string.IsNullOrEmpty(sourceDir) || !File.Exists(sourceDir + $"{DS}webgl{DS}index.html"))
            {
                errmsg = "请先前往「转换小游戏」面板完成项目导出！";
                complete?.Invoke(false, errmsg);
                Debug.LogError("[WXAssetsTextTools - CompressText]:" + errmsg);
                return;
            }
            string[] unityNamespaceInfo = ParserUnityNamespaceString(sourceDir, new string[] { "convertPluginVersion", "uintyColorSpace" });
            if (string.IsNullOrEmpty(unityNamespaceInfo[0]) || !CheckPluginVersion(unityNamespaceInfo[0]))
            {
                errmsg = "项目导出版本与当前微信SDK版本不一致，需重新前往「转换小游戏」面板导出项目！";
                complete?.Invoke(false, errmsg);
                return;
            }
            bool webgl2 = ParserUnityWebGL2(sourceDir); 
            string colorSpace = string.IsNullOrEmpty(unityNamespaceInfo[1]) ? "Gamma" : unityNamespaceInfo[1];
            if (!CheckUnityVersion(webgl2))
            {
                errmsg = "当前 Unity 版本不支持，无法使用微信纹理压缩工具。";
                complete?.Invoke(false, errmsg);
                return;
            }
            CheckNodeJSVersion((res) =>
            {
                if (!res)
                {
                    errmsg = "微信纹理压缩工具需安装 Node.js (最低支持 v15.x 推荐使用 v16.x)！ 请前往 https://nodejs.org/en/ 下载安装，并正确配置环境变量。如遇已安装Node仍存在异常可查看常见QA尝试自助解决：https://github.com/wechat-miniprogram/minigame-unity-webgl-transform/blob/main/Design/CompressedTexture.md#qa";
                    complete?.Invoke(false, errmsg);
                }
                else
                {
                    string classDataPath = Path.Combine(Application.dataPath, "WX-WASM-SDK/Editor/TextureEditor/classdata.tpk");
                    string dataDir = GetTextMinDataDir();

                    string forceStr = force ? "true" : "false";
                    string ignoreFile = WXBundleSettingWindow.getIgnoreFilePath();
                    string outDir = $"{sourceDir}{DS}webgl-min";
                    if (!string.IsNullOrEmpty(miniGameConf.CompressTexture.dstMinDir))
                    {
                        outDir = miniGameConf.CompressTexture.dstMinDir;
                    }
                    string options = $"-option=text -do=true -sourceDir=\"{sourceDir}{DS}webgl\" -outDir=\"{outDir}\" -classDataPath=\"{classDataPath}\" -dataDir=\"{dataDir}\" -force={forceStr} -ignoreConfPath=\"{ignoreFile}\" -colorSpace={colorSpace}";
                    if (!string.IsNullOrEmpty(bundleDir))
                    {
                        DirectoryInfo info = new DirectoryInfo(bundleDir);
                        string outBundleDirDef = outDir + DS + info.Name;
                        if (!string.IsNullOrEmpty(outBundleDir))
                        {
                            outBundleDirDef = outBundleDir;
                        }
                        options += $" -assetsBundleDir=\"{bundleDir}\" -assetsBundleOutDir=\"{outBundleDirDef}\"";
                        if (!Directory.Exists(outBundleDirDef))
                        {
                            Directory.CreateDirectory(outBundleDirDef);
                        }
                    }

                    WXAssetTextToolsMsgBridge.ResetRecordFiles();
                    ExceWXAssetTextTools(options, (res1) =>
                    {
                        string typeStr = debug ? "ASTC" : "ALL";
                        string options2 = $"-config=\"{dataDir}{DS}DEALSFILERECORD.json\" -outDir=\"{outDir}{DS}Assets{DS}Textures\" -type={typeStr} -dataDir=\"{dataDir}\" -force={forceStr} -colorSpace={colorSpace}";
                        ExecWXTextureConvertTools(options2, (res2) =>
                        {
                            OnReplaced(debug, outDir);
                            EditorUtility.ClearProgressBar();
                            if(res2 == "All Asset Textures have been converted! (Failed count: 0)")
                            {
                                complete?.Invoke(true, res2);
                            }
                            else
                            {
                                complete?.Invoke(false, res2);
                            }
                        }, (current, total, msg) =>
                        {
                            EditorUtility.DisplayProgressBar($"微信压缩纹理工具处理中「阶段2/2」，{current}/{total}", $"Handling:{msg}", (current * 1.0f / total) * 0.6f + 0.4f);
                        });

                    }, (current, total, msg) =>
                    {
                        EditorUtility.DisplayProgressBar($"微信压缩纹理工具处理中「阶段1/2」，{current}/{total}", $"Handling:{msg}", (current * 1.0f / total) * 0.4f);
                    });
                }
            });
        }

        /// <summary>
        /// 异步获取特定资源目录下的 AssetBundle 资源列表
        /// </summary>
        /// <param name="callback">callback(string[] files) 扫描完成后的文件列表 files ，每个元素对应当前磁盘中一个 AssetBundle 资源的绝对路径</param>
        /// <param name="bundleDir"></param>
        public static void GetAssetBundles(Action<string[]> callback = null, string bundleDir = null)
        {
            WXEditorScriptObject miniGameConf = UnityUtil.GetEditorConf();
            string sourceDir = miniGameConf.ProjectConf.DST;
            if (string.IsNullOrEmpty(sourceDir) || !File.Exists(sourceDir + $"{DS}webgl{DS}index.html"))
            {
                string errmsg = "请检查「转换小游戏」导出的 webgl 目录是否配置！或检查传入的资源目录是否有效。";
                Debug.LogError("[WXAssetsTextTools - GetAssetBundles]:" + errmsg);
                return;
            }

            string dataDir = GetTextMinDataDir();
            string options = $"-option=text -do=assetListInfo -sourceDir=\"{sourceDir}{DS}webgl\" -dataDir=\"{dataDir}\"";
            if (!string.IsNullOrEmpty(bundleDir))
            {
                options += $" -assetsBundleDir=\"{bundleDir}\"";
            }
            ExceWXAssetTextTools(options, (res) =>
            {
                if (res != null)
                {
                    callback(res.Split('\n'));
                }
                else
                {
                    callback(new string[0]);
                }
            });
        }

        /// <summary>
        /// 首包资源瘦身
        /// </summary>
        public static void FirstBundleSlim(string dataFilePath, Action<bool,string> callback = null)
        {
            if (!File.Exists(dataFilePath))
            {
                string errmsg = $"首包资源原始文件[{dataFilePath}]不存在，首包资源瘦身优化失败。";
                Debug.LogError("[WXAssetsTextTools - FirstBundleSlim]:" + errmsg);
                callback?.Invoke(false, errmsg);
                return;
            }

            string dataDir = GetTextMinDataDir();
            string tempOutDir = $"{dataDir}{DS}fbslimtTemp";
            if (!Directory.Exists(tempOutDir))
            {
                Directory.CreateDirectory(tempOutDir);
            }
            string classDataPath = Path.Combine(Application.dataPath, "WX-WASM-SDK/Editor/TextureEditor/classdata.tpk");
            string confPath = Path.Combine(Application.dataPath, "WX-WASM-SDK/Editor/TextureEditor/slim.conf");
            string options = $"-option=fb -dataDir=\"{dataDir}\" -classDataPath=\"{classDataPath}\" -sourcePath=\"{dataFilePath}\" -do=slim -outDir=\"{tempOutDir}\" -slimConfPath=\"{confPath}\"";
            ExceWXAssetTextTools(options, (res) => {
                if (string.IsNullOrEmpty(res))
                {
                    callback?.Invoke(false, "执行失败.");
                    return;
                }
                string[] resArray = res.Split('\n');
                File.Delete(dataFilePath);
                File.Move(resArray[0], dataFilePath);
                double oldSize = sizeFormatToMB(long.Parse(resArray[1]));
                double newSize = sizeFormatToMB(long.Parse(resArray[2]));
                Debug.Log($"首包资源优化完成，本次瘦身 {(oldSize - newSize).ToString("f2")}MB ，原资源包体积 {oldSize}MB ，优化后体积 {newSize}MB");
                callback?.Invoke(true, "Done");
            });
        }

        /// <summary>
        /// 转换为MB并保留两位小数
        /// </summary>
        private static double sizeFormatToMB(long size)
        {
            return (double) ((long) (size / 10000) / 100.00);
        }


        /// <summary>
        /// 执行微信资源纹理工具脚本
        /// options 为配置的执行参数
        /// callback(string result) 为脚本最终的回调结果，一次进程仅可给出1个最终结果
        /// progress(int current,int total,string info) 进度回调，一次执行可能存在多次的进度回调，进度回调最终 current = total 则为最后一次回调应调用 EditorUtility.ClearProgressBar() 结束 Unity 的进度展示
        /// </summary>
        private static void ExceWXAssetTextTools(string options, Action<string> callback, Action<int, int, string> progress = null)
        {

            string execDir = Path.Combine(Application.dataPath, $"WX-WASM-SDK{DS}Editor{DS}TextureEditor{DS}Release");
            string execPath = $"{execDir}{DS}WXTextureTools.exe";
#if UNITY_EDITOR_OSX
            var monoPath = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Unity.app/Contents/MonoBleedingEdge/bin/mono");
            RunCmd(monoPath, $"{execPath} {options}", execDir, (string result) => {
                WXAssetTextToolsMsgBridge.Parse(result, callback, progress);
            });
#else
            RunCmd(execPath, $"{options}", execDir, (string result) => {
                WXAssetTextToolsMsgBridge.Parse(result, callback, progress);
            });
#endif
        }


        private static void ExecWXTextureConvertTools(string options, Action<string> callback, Action<int, int, string> progress = null)
        {
            string execDir = Path.Combine(Application.dataPath, $"WX-WASM-SDK{DS}Editor{DS}TextureEditor{DS}Node");
            string execPath = $"{execDir}{DS}wx-assets-text-convert-tools.js";
            string nodePath = string.IsNullOrEmpty(NODE_PATH) ? UnityUtil.GetNodePath("") : NODE_PATH;
            RunCmd(nodePath, $"{execPath} {options} -execRoot={execDir}", execDir, (string result) => {
                WXAssetTextToolsMsgBridge.Parse(result, callback, progress);
            });
        }

        private static void OnReplaced(bool debug,string outDir)
        {
            WXEditorScriptObject miniGameConf = UnityUtil.GetEditorConf();
            string content = File.ReadAllText(Path.Combine(Application.dataPath, "WX-WASM-SDK", "wechat-default", "unity-sdk", "texture.js"), Encoding.UTF8);
            content = content.Replace("'$UseDXT5$'", debug ? "false" : "true");
            File.WriteAllText(Path.Combine(miniGameConf.ProjectConf.DST, "minigame", "unity-sdk", "texture.js"), content, Encoding.UTF8);
            var textureConfigPath = Path.Combine(miniGameConf.ProjectConf.DST, "minigame", "texture-config.js");
            File.WriteAllText(textureConfigPath, "GameGlobal.USED_TEXTURE_COMPRESSION=true;GameGlobal.TEXTURE_PARALLEL_BUNDLE=false;GameGlobal.TEXTURE_BUNDLES = ''", Encoding.UTF8);

            if (miniGameConf.ProjectConf.assetLoadType == 1)
            {

                DirectoryInfo TheFolder = new DirectoryInfo(miniGameConf.ProjectConf.DST + "/minigame/data-package/");
                var dstDataFiles = TheFolder.GetFiles("*.txt");
                if (dstDataFiles.Length != 1)
                {
                    Debug.LogError("目录minigame/data-package/无法找到data首资源文件, 无法进行首资源包替换");
                    return;
                }
                var dstDataFile = dstDataFiles[0].FullName;
                var sourceDataFile = Path.Combine(outDir, Path.GetFileName(dstDataFile));
                if (!File.Exists(sourceDataFile))
                {
                    Debug.LogError($"sourceDataFile not exist {sourceDataFile}");
                    return;
                }
                File.Delete(dstDataFile);
                File.Copy(sourceDataFile, dstDataFile);

            }
        }

        /// <summary>
        /// 检测本机Node.js版本，大版本应大于 v15.x.x 推荐使用 v16.x.x
        /// </summary>
        private static void CheckNodeJSVersion(Action<bool> callback = null)
        {
            try
            {
                string nodePath = string.IsNullOrEmpty(NODE_PATH) ? UnityUtil.GetNodePath("") : NODE_PATH;
                var process = UnityUtil.CreateCmdProcess(nodePath, "-v", "");
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    Debug.Log(line);
                    bool res = false;
                    if (Regex.Match(line, @"^[v]\d{1,2}[.]\d{1,2}[.]\d{1,2}").Success)
                    {
                        string m = line.Substring(1, line.IndexOf(".") - 1);
                        if (int.Parse(m) >= 15)
                        {
                            res = true;
                        }
                    }
                    callback?.Invoke(res);
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            callback?.Invoke(false);
        }

        private static void RunCmd(string cmd, string args, string workDir, Action<string> callback = null, Action<bool> complete = null)
        {
            Debug.Log($"RunCmd {cmd} {args}");
            try
            {
                var p = UnityUtil.CreateCmdProcess(cmd, args, workDir);
                while (!p.StandardOutput.EndOfStream)
                {
                    string line = p.StandardOutput.ReadLine();
                    callback?.Invoke(line);
                }

                var err = p.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(err))
                {
                    Debug.LogError(err);
                    return;
                }
                p.Close();
                complete?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }

        public static string DS;    //操作系统目录斜线符 Windows \ Unix /
        static WXAssetsTextTools()
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            System.PlatformID platformID = osInfo.Platform;
            if (platformID == System.PlatformID.MacOSX || platformID == System.PlatformID.Unix)
            {
                DS = "/";
            }
            else
            {
                DS = "\\";
            }
        }

        private static string[] SupportUnityVersion = new string[] { "2018.", "2019.", "2020.", "2021.2" };
        private static string[] SupportUnityVersionWebGL2 = new string[] { "2019.4.29", "2020.", "2021.2" };
        /**
            Unity 2021.3.x 及以后 2022 等版本均不支持纹理压缩
            支持版本： 2018、2019、2020、2021 其中2021.3.x 不支持 https://github.com/wechat-miniprogram/minigame-unity-webgl-transform#%E5%AE%89%E8%A3%85%E4%B8%8E%E4%BD%BF%E7%94%A8
        */
        private static bool CheckUnityVersion(bool webgl2 = false)
        {
            string unityVersion = Application.unityVersion;
            bool success = false;
            if (webgl2)
            {
                for (int i = 0; i < SupportUnityVersionWebGL2.Length; i++)
                {
                    if (unityVersion.IndexOf(SupportUnityVersionWebGL2[i]) != -1)
                    {
                        success = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < SupportUnityVersion.Length; i++)
                {
                    if (unityVersion.IndexOf(SupportUnityVersion[i]) != -1)
                    {
                        success = true;
                        break;
                    }
                }
            }
            if (!success)
            {
                if (unityVersion.IndexOf("2021.3") != -1)
                {
                    Debug.LogError("纹理压缩工具暂不支持Unity 2021.3.x 版本，请使用 2021.2.18(含)之前版本");
                    return false;
                }
                if (webgl2)
                {
                    Debug.LogError("当前Unity版本不支持 WebGL2 纹理压缩，相关说明请阅读 https://github.com/wechat-miniprogram/minigame-unity-webgl-transform/blob/main/Design/CompressedTexture.md#webgl20-%E6%94%AF%E6%8C%81%E8%AF%B4%E6%98%8E");
                    return false;
                }
                else
                {
                    Debug.LogError("当前Unity版本可能不支持纹理压缩，建议使用 2021.2.18(含)之前版本");
                }
            }
            return true;

        }

        /// <summary>
        /// 纹理资源处理工具的数据目录
        /// </summary>
        public static string GetTextMinDataDir()
        {
            string assetsDir = Application.dataPath;
            string root = assetsDir.Substring(0, assetsDir.LastIndexOf("Assets"));
            string workDir = $"{root}TextToolDatas";
            return workDir;
        }

        /// <summary>
        /// 导出最后一次执行日志
        /// </summary>
        public static void exportLastLog()
        {
            string[] files = WXAssetTextToolsMsgBridge.GetRecordFiles();
            foreach (string file in files)
            {
                Debug.Log(file);
            }
            if(files.Length == 0)
            {
                Debug.LogError("本次运行期间未产生日志文件，请执行一次压缩纹理后再尝试导出.");
                return;
            }
            string dataDir = GetTextMinDataDir();
            string random = Math.Ceiling((1000 + 8999 * new System.Random().NextDouble())).ToString();
            string logFileTempDir = $"{dataDir}{DS}packlog{DS}{random}";
            string logFileTempFile = $"{logFileTempDir}{DS}{random}.log";
            string logFileName = $"{dataDir}{DS}log{DS}wx-texture-min-{random}.log";
            if (!Directory.Exists(logFileTempDir))
            {
                Directory.CreateDirectory(logFileTempDir);
            }
            //merge
            List<string> fileHeader = new List<string>();
            List<string> fileContent = new List<string>();
            foreach (string filePath in files)
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogError("日志文件缺失，请重新执行一次压缩纹理后再尝试导出.");
                    return;
                }
                string content = "";
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                fileContent.Add(content);
                fileHeader.Add(content.Length.ToString());
                FileInfo info = new FileInfo(filePath);
                fileHeader.Add(info.Name);
            }

            using (FileStream file = new FileStream(logFileTempFile, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    foreach (string item in fileHeader)
                    {
                        writer.Write(item);
                        writer.Write(",");
                    }
                    writer.Write("\n");
                    foreach(string content in fileContent)
                    {
                        writer.Write(content);
                    }
                }
            }

            var exePath = "";
#if UNITY_EDITOR_OSX
            exePath = Path.Combine(Application.dataPath, "WX-WASM-SDK/Editor/Brotli/macos/brotli");
#if UNITY_2021_2_OR_NEWER
            exePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "PlaybackEngines/WebGLSupport/BuildTools/Brotli/macos/brotli");
#endif
#else
            exePath = Path.Combine(Application.dataPath, "WX-WASM-SDK/Editor/Brotli/win_x86_64/brotli.exe");
#endif
            RunCmd(exePath, $" --force --quality 11 --input \"{logFileTempFile}\" --output \"{logFileName}\"", "", null, (bool result) => {
                Debug.Log("执行日志导出成功，路径为： " + logFileName);
                File.Delete(logFileTempFile);
                EditorUtility.RevealInFinder(logFileName);
            });
       
        }

        /// <summary>
        /// 判断导出工程是否是webgl2模式
        /// </summary>
        private static bool ParserUnityWebGL2(string sourceDir)
        {
            string unityGameJSFilePath = $"{sourceDir}{DS}minigame{DS}game.js";
            string errmsg = "";
            if (!File.Exists(unityGameJSFilePath))
            {
                errmsg = "请检查 sourceDir 目录为「转换小游戏」导出的 webgl 目录，并且项目已成功导出！";
                Debug.LogError(errmsg);
                throw new Exception(errmsg);
            }
            string content = "";
            using (FileStream file = new FileStream(unityGameJSFilePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    content = reader.ReadToEnd();
                }
            }
            string contentTemp = content.Substring(content.IndexOf("contextType:") + 12);
            string value = contentTemp.Substring(0, contentTemp.IndexOf(",")).Trim();
            return value == "2";
        }

        /// <summary>
        /// 获取Unity-Namespace文件中的常量字段数据中 String 类型数据
        /// </summary>
        private static string[] ParserUnityNamespaceString(string sourceDir, string[] keywords)
        {
            string unityNamespaceFilePath = $"{sourceDir}{DS}minigame{DS}unity-namespace.js";
            string errmsg = "";
            if (!File.Exists(unityNamespaceFilePath))
            {
                errmsg = "请检查 sourceDir 目录为「转换小游戏」导出的 webgl 目录，并且项目已成功导出！";
                Debug.LogError(errmsg);
                return null;
            }

            string content = "";
            using (FileStream file = new FileStream(unityNamespaceFilePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    content = reader.ReadToEnd();
                }
            }
            string[] results = new string[keywords.Length];
            for (int i = 0; i < keywords.Length; i++)
            {
                string key = keywords[i];
                string contentTemp = content.Substring(content.IndexOf(key) + key.Length);

                var singleSign = contentTemp.IndexOf("'");
                var doubleSign = contentTemp.IndexOf("\"");
                char sign;
                if (singleSign == -1 && doubleSign == -1)
                {
                    errmsg = "无法识别的游戏导出版本，请重新安装微信SDK导出游戏工程！";
                    Debug.LogError(errmsg);
                    results[i] = null;
                    continue;
                }
                if (singleSign == -1)
                {
                    singleSign = 1000;
                }
                if (doubleSign == -1)
                {
                    doubleSign = 1000;
                }
                if (singleSign < doubleSign)
                {
                    sign = '\'';
                }
                else
                {
                    sign = '"';
                }
                string value = contentTemp.Split(sign)[1];
                results[i] = value;
            }
            return results;
        }

        /// <summary>
        /// 检查插件版本号，使用纹理压缩工具处理的资源必须保证项目也是当前版本插件导出的
        /// </summary>
        public static bool CheckPluginVersion(string version)
        {
            if (version.Equals(WXPluginVersion.pluginVersion))
            {
                return true;
            }
            else
            {
                string errmsg = $"游戏工程由[{version}]版本插件导出，与当前插件版本[{WXPluginVersion.pluginVersion}]不符合，无法跨版本使用微信纹理压缩工具，请重新前往「转换小游戏」面板进行工程导出。";
                Debug.LogError(errmsg);
                return false;
            }
        }

    }
}