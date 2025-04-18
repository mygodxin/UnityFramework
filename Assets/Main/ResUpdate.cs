
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class ResUpdate
{
    /// <summary>
    /// 默认包名
    /// </summary>
    public static readonly string PackageName = "DefaultPackage";

    public static async Task Start(EPlayMode playMode)
    {
        YooAssets.Initialize();

        //初始化包
        var initializationOperation = InitPackage(playMode);
        await initializationOperation.Task;
        if (initializationOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"{initializationOperation.Error}");
        }
        var gamePackage = YooAssets.GetPackage(PackageName);

        //更新包版本
        var updatePackageVersionOperation = gamePackage.RequestPackageVersionAsync();
        await updatePackageVersionOperation.Task;
        if (updatePackageVersionOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning(updatePackageVersionOperation.Error);
        }
        var packageVersion = updatePackageVersionOperation.PackageVersion;
        Debug.Log($"PackageVersion:当前{/*gamePackage.GetPackageVersion()*/""},最新{packageVersion}");

        //更新Manifest
        var updatePackageManifestOperation = gamePackage.UpdatePackageManifestAsync(packageVersion);
        await updatePackageManifestOperation.Task;
        if (updatePackageManifestOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning(updatePackageManifestOperation.Error);
        }

        //下载
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var downloader = gamePackage.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("not need update resources!");
        }
        // 发现新更新文件后，挂起流程系统
        // 注意：开发者需要在下载前检测磁盘空间不足
        // 需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;
        //注册回调方法
        downloader.DownloadFinishCallback = OnDownloadFinishFunction; //当下载器结束（无论成功或失败）
        downloader.DownloadErrorCallback = OnDownloadErrorFunction; //当下载器发生错误
        downloader.DownloadUpdateCallback = OnDownloadUpdateFunction; //当下载进度发生变化
        downloader.DownloadFileBeginCallback = OnDownloadFileBeginFunction; //当开始下载某个文件

        //开启下载
        downloader.BeginDownload();
        await downloader.Task;
        //检测下载结果
        if (downloader.Status == EOperationStatus.Succeed)
        {
            //下载成功
        }
        else
        {
            //下载失败
        }
    }

    public static void OnDownloadErrorFunction(DownloadErrorData downloadErrorData)
    {
    }
    public static void OnDownloadUpdateFunction(DownloadUpdateData downloadUpdateData)
    {
        //Message = $"正在下载更新文件:{currentDownloadSizeBytes / 1048576f:F2}MB/{totalDownloadSizeBytes / 1048576f:F2}MB";
        //CurrentProgress = currentDownloadSizeBytes / totalDownloadSizeBytes;
    }
    public static void OnDownloadFinishFunction(DownloaderFinishData downloaderFinishData)
    {
    }
    public static void OnDownloadFileBeginFunction(DownloadFileData downloadFileData)
    {

    }

    private static InitializationOperation InitPackage(EPlayMode playMode)
    {
        // 创建资源包裹类
        var package = YooAssets.TryGetPackage(PackageName);
        if (package == null)
            package = YooAssets.CreatePackage(PackageName);
        YooAssets.SetDefaultPackage(package);

        // 编辑器下的模拟模式
        InitializationOperation initializationOperation = null;
        if (playMode == EPlayMode.EditorSimulateMode)
        {
            var buildResult = EditorSimulateModeHelper.SimulateBuild(PackageName);
            var packageRoot = buildResult.PackageRootDirectory;
            var createParameters = new EditorSimulateModeParameters();
            createParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // 单机运行模式
        if (playMode == EPlayMode.OfflinePlayMode)
        {
            var createParameters = new OfflinePlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // 联机运行模式
        if (playMode == EPlayMode.HostPlayMode)
        {
            string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var createParameters = new HostPlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // WebGL运行模式
        if (playMode == EPlayMode.WebPlayMode)
        {
            var createParameters = new WebPlayModeParameters();
#if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
			string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            string packageRoot = $"{WeChatWASM.WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE"; //注意：如果有子目录，请修改此处！
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            createParameters.WebServerFileSystemParameters = WechatFileSystemCreater.CreateWechatFileSystemParameters(packageRoot, remoteServices);
#else
            createParameters.WebServerFileSystemParameters = FileSystemParameters.CreateDefaultWebServerFileSystemParameters(new WebDecryption());
#endif
            initializationOperation = package.InitializeAsync(createParameters);
        }

        return initializationOperation;
    }

    /// <summary>
    /// 获取资源服务器地址
    /// </summary>
    private static string GetHostServerURL()
    {
        //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
        string hostServerIP = "http://192.168.1.240:88/XZResUpdate";
        string appVersion = "1.0";

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
    }

    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    private class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

    private class WebDecryption : IWebDecryptionServices
    {
        public const byte KEY = 64;

        public WebDecryptResult LoadAssetBundle(WebDecryptFileInfo fileInfo)
        {
            byte[] copyData = new byte[fileInfo.FileData.Length];
            Buffer.BlockCopy(fileInfo.FileData, 0, copyData, 0, fileInfo.FileData.Length);

            for (int i = 0; i < copyData.Length; i++)
            {
                copyData[i] ^= KEY;
            }

            WebDecryptResult decryptResult = new WebDecryptResult();
            decryptResult.Result = AssetBundle.LoadFromMemory(copyData);
            return decryptResult;
        }
    }
}