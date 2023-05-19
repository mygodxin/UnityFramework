using System;
using System.Collections.Generic;

using UnityEngine.Scripting;

namespace WeChatWASM
{
    public class WXBaseResponse
    {
        public string callbackId; //回调id,调用者不需要关注
        public string errMsg;   //失败示例 getUserInfo:fail auth deny; 成功示例 getUserInfo:ok
    }

    public class WXBaseActionParam<T>
    {
        public Action<T> success;  //接口调用成功的回调函数
        public Action<T> fail;   //接口调用失败的回调函数	
        public Action<T> complete;  //接口调用结束的回调函数（调用成功、失败都会执行）
    }



    public class WXTextResponse : WXBaseResponse
    {
        public int errCode;
    }

    public class WXReadFileResponse : WXBaseResponse
    {
        /// <summary>
        /// 如果返回二进制，则数据在这个字段
        /// </summary>
        public byte[] binData;
        /// <summary>
        /// 如果返回的是字符串，则数据在这个字段
        /// </summary>
        public string stringData;
    }

    public class WXUserInfoResponse : WXBaseResponse
    {
        // 具体说明可以参考 https://developers.weixin.qq.com/minigame/dev/api/open-api/user-info/wx.getUserInfo.html
        public int errCode; // 0为成功，非零为失败
        public string signature; //使用 sha1( rawData + sessionkey ) 得到字符串，用于校验用户信息，详见 https://developers.weixin.qq.com/minigame/dev/guide/open-ability/signature.html
        public string encryptedData; //包括敏感数据在内的完整用户信息的加密数据，详见 https://developers.weixin.qq.com/minigame/dev/guide/open-ability/signature.html#%E5%8A%A0%E5%AF%86%E6%95%B0%E6%8D%AE%E8%A7%A3%E5%AF%86%E7%AE%97%E6%B3%95
        public string iv; //加密算法的初始向量，详见 https://developers.weixin.qq.com/minigame/dev/guide/open-ability/signature.html#%E5%8A%A0%E5%AF%86%E6%95%B0%E6%8D%AE%E8%A7%A3%E5%AF%86%E7%AE%97%E6%B3%95
        public string cloudID; //敏感数据对应的云 ID，开通云开发的小程序才会返回，可通过云调用直接获取开放数据，详细见云调用直接获取开放数据 https://developers.weixin.qq.com/minigame/dev/guide/open-ability/signature.html#method-cloud
        public WXUserInfo userInfo; //用户信息对象，不包含 openid 等敏感信息
        public string userInfoRaw; //userinfo的序列化
    }

    public class WXADErrorResponse : WXBaseResponse
    {
        // 具体说明可以参考 https://developers.weixin.qq.com/minigame/dev/api/ad/BannerAd.onError.html
        public int errCode;
    }
    public class WXADLoadResponse : WXBaseResponse
    {
        // 具体说明可以参考 https://developers.weixin.qq.com/minigame/dev/api/ad/BannerAd.onLoad.html
        public int rewardValue;
        public int shareValue;
    }

    public class WXADResizeResponse : WXBaseResponse
    {
        // 具体说明可以参考 https://developers.weixin.qq.com/minigame/dev/api/ad/BannerAd.onResize.html
        public int width;
        public int height;
    }

    public class WXRewardedVideoAdOnCloseResponse : WXBaseResponse
    {
        /// <summary>
        /// 视频是否是在用户完整观看的情况下被关闭的,详见 https://developers.weixin.qq.com/minigame/dev/api/ad/RewardedVideoAd.onClose.html
        /// </summary>
        public bool isEnded;
    }

    public class RequestAdReportShareBehaviorParam
    {
        public int operation; // 1-曝光 2-点击 3-关闭 4-操作成功 5-操作失败 6-分享拉起
        public int currentShow; // 0-广告 1-分享，当 operation 为 1-5 时必填
        public int strategy; // 0-业务 1-微信策略
        public string inviteUser; // 当 operation 为 6 时必填，填写分享人的 openId
        public string inviteUserAdunit; // 当 operation 为 6 时必填，填写分享人分享时的广告单元
        public int shareValue; // 分享推荐值，必填
        public int rewardValue; // 激励广告推荐值，必填
        public int? depositAmount; // ⽤户产生的付费⾦额，只要发生付费，都需要回传，用于优化分享价值预估。选填，该字段要求基础库版本在2.24.7及以上
    }

    public class WXRewardedVideoAdReportShareBehaviorResponse : WXBaseResponse {
        public string success;
        public string message;
    }

    /// <summary>
    /// 云函数回调 https://developers.weixin.qq.com/minigame/dev/wxcloud/reference-sdk-api/functions/Cloud.callFunction.html
    /// </summary>
    public class WXCloudCallFunctionResponse : WXBaseResponse {
        /// <summary>
        /// 后端返回的经过json序列化后的数据
        /// </summary>
        public string result;
        public string requestID;
    }


    public struct ReferrerInfo
    {
        public string appid;
        /// <summary>
        /// 对应JS版里的 extraData，这里序列化成JSON字符串
        /// </summary>
        public string extraDataRaw;
    }


    public struct WXUserInfo
    {
        /// <summary>
        /// 详见 https://developers.weixin.qq.com/minigame/dev/api/open-api/user-info/UserInfo.html
        /// </summary>
        public string nickName;
        public string avatarUrl;
        public string country;
        public string province;
        public string city;
        public string language;
        public int gender;
    }


    public struct WXSafeArea
    {
        public int left;
        public int right;
        public int top;
        public int bottom;
        public int width;
        public int height;
    }

    public class WXAccountInfo : WXBaseResponse
    {
        public WXAccountInfoMiniProgram miniProgram;
        public string miniProgramRaw;
        public WXAccountInfoPlugin plugin;
        public string pluginRaw;
    }


    public struct WXAccountInfoMiniProgram
    {
        public string appId;
        public string envVersion;
    }

    public struct WXAccountInfoPlugin
    {
        public string appId;
        public string version;
    }


    public class TemplateInfo
    {
        public TemplateInfoItem[] parameterList;
    }

    public class TemplateInfoItem
    {
        public string name;
        public string value;
    }

    public class WXShareAppMessageParam
    {
        // 各字段说明详见这里，https://developers.weixin.qq.com/minigame/dev/api/share/wx.shareAppMessage.html
        public string title;
        public string imageUrl;
        public string query;
        public string imageUrlId;
        public bool toCurrentGroup;
        public string path;
    }

    /// <summary>
    /// 创建 banner 广告组件参数，参数详见 https://developers.weixin.qq.com/minigame/dev/api/ad/wx.createBannerAd.html
    /// </summary>
    public class WXCreateBannerAdParam
    {
        public string adUnitId;
        public int adIntervals;
        public Style style;
        public string styleRaw; //该字段不需要传
    }

    /// <summary>
    /// 创建激励视频广告组件参数，参数详见 https://developers.weixin.qq.com/minigame/dev/api/ad/wx.createRewardedVideoAd.html
    /// </summary>
    public class WXCreateRewardedVideoAdParam
    {
        public string adUnitId;
        public bool multiton;
    }

    /// <summary>
    /// 创建插屏广告组件参数,参数详见 https://developers.weixin.qq.com/minigame/dev/api/ad/wx.createInterstitialAd.html
    /// </summary>
    public class WXCreateInterstitialAdParam
    {
        public string adUnitId;
    }

    /// <summary>
    /// 创建格子广告参数，参数详见 https://developers.weixin.qq.com/minigame/dev/api/ad/wx.createGridAd.html
    /// </summary>
    public class WXCreateGridAdParam
    {
        public string adUnitId;
        public int adIntervals;
        public string adTheme;
        public int gridCount;
        public Style style;
        public string styleRaw; //该字段不需要传 
    }

    /// <summary>
    /// 创建原生模板广告参数，参数详见 https://developers.weixin.qq.com/minigame/dev/api/ad/wx.createCustomAd.html
    /// </summary>
    public class WXCreateCustomAdParam
    {
        public string adUnitId;
        public int adIntervals;
        public CustomStyle style;
        public string styleRaw; //该字段不需要传 
    }

    public struct Style
    {
        public int left;
        public int top;
        public int width;
        public int height;
    }

    /// <summary>
    /// 原生模板广告组件的样式
    /// </summary>
    public struct CustomStyle
    {
        /// <summary>
        /// 原生模板广告组件的左上角横坐标
        /// </summary>
        public int left;
        /// <summary>
        /// 原生模板广告组件的左上角纵坐标
        /// </summary>
        public int top;
        /// <summary>
        /// 原生模板广告组件是否固定屏幕位置（不跟随屏幕滚动）, 相当于JS api里的 fixed
        /// </summary>
        public bool isFixed;
    }

    /// <summary>
    /// 将当前 Canvas 保存为一个临时文件的同步版本,详见 https://developers.weixin.qq.com/minigame/dev/api/render/canvas/Canvas.toTempFilePathSync.html
    /// </summary>
    public class WXToTempFilePathSyncParam
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public int destWidth;
        public int destHeight;
        public string fileType = "png";
        public float quality = 1.0f;
    }

    [Preserve]
    public class ToTempFilePathParamSuccessCallbackResult : WXBaseResponse {
        /// <summary> 
        /// canvas 生成的临时文件路径 (本地路径)
        /// </summary>
        public string tempFilePath;
    }

    /// <summary>
    /// 将当前 Canvas 保存为一个临时文件的异步版本,详见 https://developers.weixin.qq.com/minigame/dev/api/render/canvas/Canvas.toTempFilePath.html
    /// </summary>

    public class WXToTempFilePathParam : WXToTempFilePathSyncParam
    {
        /// <summary> 
        /// 接口调用结束的回调函数（调用成功、失败都会执行）
        /// </summary>
        public Action<WXTextResponse> complete;
        /// <summary> 
        /// 接口调用失败的回调函数
        /// </summary>
        public Action<WXTextResponse> fail;
        /// <summary> 
        /// 接口调用成功的回调函数
        /// </summary>
        public Action<ToTempFilePathParamSuccessCallbackResult> success;
    }

    /// <summary>
    /// https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.access.html
    /// </summary>
    public class AccessParam : WXBaseActionParam<WXTextResponse>
    {
        public string path;
    }

    /// <summary>
    /// https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.unlink.html
    /// </summary>
    public class UnlinkParam : WXBaseActionParam<WXTextResponse>
    {
        public string filePath;
    }

    /// <summary>
    /// https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.mkdir.html
    /// </summary>
    public class MkdirParam : WXBaseActionParam<WXTextResponse>
    {
        /// <summary>
        /// 创建的目录路径 (本地路径)
        /// </summary>
        public string dirPath;
        /// <summary>
        /// 是否在递归创建该目录的上级目录后再创建该目录。如果对应的上级目录已经存在，则不创建该上级目录。如 dirPath 为 a/b/c/d 且 recursive 为 true，将创建 a 目录，再在 a 目录下创建 b 目录，以此类推直至创建 a/b/c 目录下的 d 目录。
        /// </summary>
        public bool recursive = false;
    }

    /// <summary>
    /// https://developers.weixin.qq.com/miniprogram/dev/api/file/FileSystemManager.rmdir.html
    /// </summary>
    public class RmdirParam : WXBaseActionParam<WXTextResponse>
    {
        /// <summary>
        /// 要删除的目录路径 (本地路径)
        /// </summary>
        public string dirPath;
        /// <summary>
        /// 是否递归删除目录。如果为 true，则删除该目录和该目录下的所有子目录以及文件。
        /// </summary>
        public bool recursive = false;
    }

    /// <summary>
    /// https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.copyFile.html
    /// </summary>
    public class CopyFileParam : WXBaseActionParam<WXTextResponse>
    {
        public string srcPath;
        public string destPath;
    }

    [Preserve]
    public class TouchEvent
    {
        /// <summary>
        /// 当前所有触摸点的列表
        /// </summary>
        public Touch[] touches;
        /// <summary>
        /// 触发此次事件的触摸点列表,可以通过这个知道触发当前通知的事件的位置
        /// </summary>
        public Touch[] changedTouches;
        /// <summary>
        /// 事件触发时的时间戳
        /// </summary>
        public long timeStamp;
    }


    /// <summary>
    /// 调用云函数 https://developers.weixin.qq.com/minigame/dev/wxcloud/reference-sdk-api/functions/Cloud.callFunction.html
    /// </summary>
    public class CallFunctionParam : WXBaseActionParam<WXCloudCallFunctionResponse>
    {
        public string name;
        /// <summary>
        /// 这里请将数据json序列化为字符串再赋值到data
        /// </summary>
        public string data;
        public CallFunctionConf config;
    }

    public class CallFunctionConf
    {
        public string env;
    }

    /// <summary>
    /// 云函数初始化 https://developers.weixin.qq.com/minigame/dev/wxcloud/reference-sdk-api/init/client.init.html
    /// </summary>
    public class CallFunctionInitParam
    {
        /// <summary>
        /// 必填，环境ID，指定接下来调用 API 时访问哪个环境的云资源
        /// </summary>
        public string env;
        /// <summary>
        /// 是否在将用户访问记录到用户管理中，在控制台中可见
        /// </summary>
        public bool traceUser;
    }

    public class InnerAudioContextParam
    {
        /// <summary>
        /// 音频资源的地址，用于直接播放。可以设置为网络地址，或者unity中的本地路径如 Assets/xx.wav，运行时会自动和配置的音频地址前缀做拼接得到最终线上地址
        /// </summary>
        public string src = "";
        /// <summary>
        /// 是否循环播放,默认为 false
        /// </summary>
        public bool loop = false;
        /// <summary>
        /// 开始播放的位置（单位：s），默认为 0
        /// </summary>
        public float startTime = 0;
        /// <summary>
        /// 是否自动开始播放，默认为 false
        /// </summary>
        public bool autoplay = false;
        /// <summary>
        /// 音量。范围 0~1。默认为 1
        /// </summary>
        public float volume = 1;
        /// <summary>
        /// 播放速度。范围 0.5-2.0，默认为 1。
        /// </summary>
        public float playbackRate = 1;
        /// <summary>
        /// 下载音频，设置为true后，会完全下载后再触发OnCanplay，方便后续音频复用，避免延迟
        /// </summary>
        public bool needDownload = false;
    }

    /// <summary>
    /// 网络状态变化事件的回调参数，详见 https://developers.weixin.qq.com/minigame/dev/api/device/network/wx.onNetworkStatusChange.html
    /// </summary>
    public class NetworkStatus {
        /// <summary>
        /// 当前是否有网络连接
        /// </summary>
        public bool isConnected;
        /// <summary>
        /// 网络类型
        /// </summary>
        public string networkType;
    }
    
    public class WriteFileParam : WXBaseActionParam<WXTextResponse>
    {
        /// <summary>
        /// 要写入的文件路径 (本地路径)
        /// </summary>
        public string filePath;
        /// <summary>
        /// 要写入的二进制数据
        /// </summary>
        public byte[] data;
        /// <summary>
        /// 指定写入文件的字符编码
        /// </summary>
        public string encoding = "utf8";
    }

    public class WriteFileStringParam : WXBaseActionParam<WXTextResponse>
    {
        /// <summary>
        /// 要写入的文件路径 (本地路径)
        /// </summary>
        public string filePath;
        /// <summary>
        /// 要写入的二进制数据
        /// </summary>
        public string data;
        /// <summary>
        /// 指定写入文件的字符编码
        /// </summary>
        public string encoding = "utf8";
    }

    public class ReadFileParam : WXBaseActionParam<WXReadFileResponse>
    {
        /// <summary>
        /// 要读取的文件的路径 (本地路径)
        /// </summary>
        public string filePath;
        /// <summary>
        /// 指定读取文件的字符编码，如果不传 encoding，则以 ArrayBuffer 格式读取文件的二进制内容
        /// </summary>
        public string encoding;
    }

    public class WXReadFileCallback : WXTextResponse
    {
        public string data;
        public int byteLength;
    }

    public class WXStatInfo
    {
        /// <summary>
        ///  文件的类型和存取的权限，对应 POSIX stat.st_mode
        /// </summary>
        public int mode;
        /// <summary>
        /// 文件大小，单位：B，对应 POSIX stat.st_size
        /// </summary>
        public int size;
        /// <summary>
        /// 文件最近一次被存取或被执行的时间，UNIX 时间戳，对应 POSIX stat.st_atime
        /// </summary>
        public UInt32 lastAccessedTime;
        /// <summary>
        /// 文件最后一次被修改的时间，UNIX 时间戳，对应 POSIX stat.st_mtime
        /// </summary>
        public UInt32 lastModifiedTime;
    }

    public class WXStat
    {
        /// <summary>
        ///  文件的路径
        /// </summary>
        public string path;
        public WXStatInfo stats;
    }

    public class WXStatResponse : WXBaseResponse
    {
        public System.Collections.Generic.List<WXStat> stats;
        public WXStatInfo one_stat;
    }

    public class WXStatOption : WXBaseActionParam<WXStatResponse>
    {
        /// <summary>
        /// 文件/目录路径 (本地路径)
        /// </summary>
        public string path;
        /// <summary>
        /// 是否递归获取目录下的每个文件的 Stats 信息	2.3.0
        /// </summary>
        public bool recursive = true; 
    }

    public class WXVideoCallback : WXTextResponse
    {
        /// <summary>
        /// 当前的播放位置，单位为秒
        /// </summary>
        public float position;
        /// <summary>
        /// 视频的总时长，单位为秒
        /// </summary>
        public float duration;
        /// <summary>
        /// 当前的缓冲进度，缓冲进度区间为 (0~100]，100表示缓冲完成
        /// </summary>
        public int buffered;
    }

    public class WXVideoProgress
    {
        /// <summary>
        /// 视频的总时长，单位为秒
        /// </summary>
        public float duration;
        /// <summary>
        /// 当前的缓冲进度，缓冲进度区间为 (0~100]，100表示缓冲完成
        /// </summary>
        public int buffered;
    }

    public class WXVideoTimeUpdate
    {
        /// <summary>
        /// 当前的播放位置，单位为秒
        /// </summary>
        public float position;
        /// <summary>
        /// 视频的总时长，单位为秒
        /// </summary>
        public float duration;
    }


    //创建视频，详见 https://developers.weixin.qq.com/minigame/dev/api/media/video/wx.createVideo.html
    public class WXCreateVideoParam
    {
        /// <summary>
        /// 视频的左上角横坐标
        /// </summary>
        public int x=0;
        /// <summary>
        /// 视频的左上角纵坐标
        /// </summary>
        public int y=0;
        /// <summary>
        /// 视频的宽度
        /// </summary>
        public int width=300;
        /// <summary>
        /// 视频的高度
        /// </summary>
        public int height=100;
        /// <summary>
        /// 视频的资源地址
        /// </summary>
        public string src;
        /// <summary>
        /// 视频的封面
        /// </summary>
        public string poster;
        /// <summary>
        /// 视频的初始播放位置，单位为 s 秒
        /// </summary>
        public int initialTime;
        /// <summary>
        /// 视频的播放速率，有效值有 0.5、0.8、1.0、1.25、1.5
        /// </summary>
        public float playbackRate=1.0f;
        /// <summary>
        /// 视频是否为直播
        /// </summary>
        public bool live;
        /// <summary>
        /// 视频的缩放模式
        /// </summary>
        public string objectFit= "contain";
        /// <summary>
        /// 视频是否显示控件
        /// </summary>
        public bool controls = true;
        /// <summary>
        /// 是否显示视频底部进度条
        /// </summary>
        public bool showProgress = true;
        /// <summary>
        /// 是否显示控制栏的进度条
        /// </summary>
        public bool showProgressInControlMode = true;
        /// <summary>
        /// 视频背景颜色
        /// </summary>
        public string backgroundColor ="#000000";
        /// <summary>
        /// 视频是否自动播放
        /// </summary>
        public bool autoplay;
        /// <summary>
        /// 视频是否是否循环播放
        /// </summary>
        public bool loop;
        /// <summary>
        /// 视频是否禁音播放
        /// </summary>
        public bool muted;
        /// <summary>
        /// 视频是否遵循系统静音开关设置（仅iOS）
        /// </summary>
        public bool obeyMuteSwitch;
        /// <summary>
        /// 是否启用手势控制播放进度	
        /// </summary>
        public bool enableProgressGesture = true;
        /// <summary>
        /// 是否开启双击播放的手势	
        /// </summary>
        public bool enablePlayGesture;
        /// <summary>
        /// 是否显示视频中央的播放按钮
        /// </summary>
        public bool showCenterPlayBtn = true;
        /// <summary>
        /// 视频是否显示在游戏画布之下
        /// </summary>
        public bool underGameView;
    }

    public enum EnvVersion {
        /// <summary>
        /// 开发版
        /// </summary>
        develop,
        /// <summary>
        /// 体验版
        /// </summary>
        trial,
        /// <summary>
        /// 正式版
        /// </summary>
        release
    }

    public enum GameClubButtonType
    {
        /// <summary>
        /// 可以设置背景色和文本的按钮
        /// </summary>
        text,
        /// <summary>
        /// 只能设置背景贴图的按钮，背景贴图会直接拉伸到按钮的宽高
        /// </summary>
        image
    }
    public enum GameClubButtonTextAlign
    {
        /// <summary>
        /// 居左
        /// </summary>
        left,
        /// <summary>
        /// 居中
        /// </summary>
        center,
        /// <summary>
        /// 居右
        /// </summary>
        right,
    }
    public struct GameClubButtonStyle
    {
        public int left;
        public int top;
        public int width;
        public int height;
        public string backgroundColor;
        public string borderColor;
        public int borderWidth;
        public int borderRadius;
        public string color;
        public GameClubButtonTextAlign textAlign;
        public int fontSize;
        public int lineHeight;
    }
    public enum GameClubButtonIcon
    {
        green,
        white,
        dark,
        light,
    }
    /// <summary>
    /// 创建游戏圈参数，详见 https://developers.weixin.qq.com/minigame/dev/api/open-api/game-club/wx.createGameClubButton.html
    /// </summary>
    public class WXCreateGameClubButtonParam
    {
        /// <summary>
        /// 必填，按钮类型
        /// </summary>
        public GameClubButtonType type;
        /// <summary>
        /// 按钮上的文本，仅当 type 为 text 时有效
        /// </summary>
        public string text;
        /// <summary>
        /// 按钮的背景图片，仅当 type 为 image 时有效
        /// </summary>
        public string image;
        /// <summary>
        /// 必填，按钮的样式
        /// </summary>
        public GameClubButtonStyle style;
        public string styleRaw;
        /// <summary>
        /// 必填，游戏圈按钮的图标，仅当 object.type 参数为 image 时有效。
        /// </summary>
        public GameClubButtonIcon icon;
    }

    /// <summary>
    /// 清理文件缓存的结果
    /// </summary>
    public enum ReleaseResult {
        /// <summary>
        /// 无需清理，空间足够
        /// </summary>
        noNeedRelease = 1,
        /// <summary>
        /// 超过最大存储容量，不清理
        /// </summary>
        exceedMax,
        /// <summary>
        /// 清理成功
        /// </summary>
        releaseSuccess,
    }
    /// <summary>
    /// 启动数据
    /// </summary>
    public class LaunchEvent
    {
        public LaunchEventType type;
        /// <summary>
        /// 当前阶段耗时
        /// </summary>
        public int costTimeMs;
        /// <summary>
        /// 自插件启动后总运行时间
        /// </summary>
        public int runTimeMs;
        /// <summary>
        /// 是否需要下载资源包
        /// </summary>
        public bool needDownloadDataPackage;
        /// <summary>
        /// 首包资源是否作为小游戏代码分包下载
        /// </summary>
        public bool loadDataPackageFromSubpackage;
        /// <summary>
        /// 当前阶段完成时是否处于前台
        /// </summary>
        public bool isVisible;
        /// <summary>
        /// 是否开启了代码分包
        /// </summary>
        public bool useCodeSplit;
        /// <summary>
        /// 是否iOS高性能模式
        /// </summary>
        public bool isHighPerformance;
    }
    /// <summary>
    /// 启动阶段类型定义
    /// </summary>
    public enum LaunchEventType {
        /// <summary>
        /// 插件启动
        /// </summary>
        launchPlugin,
        /// <summary>
        /// 下载wasm代码
        /// </summary>
        loadWasm,
        /// <summary>
        /// 编译wasm
        /// </summary>
        compileWasm,
        /// <summary>
        /// 下载首包资源
        /// </summary>
        loadAssets,
        /// <summary>
        /// 读取首包资源
        /// </summary>
        readAssets = 5,
        /// <summary>
        /// 引擎初始化(callmain)
        /// </summary>
        prepareGame,
    }

    /// <summary>
    /// reportScene接口参数
    /// </summary>
    public class ReportSceneParams : WXBaseActionParam<GeneralCallbackResult>
    {
        /// <summary>
        /// 场景ID，在「小程序管理后台」获取
        /// </summary>
        public int sceneId;
        /// <summary>
        /// 此场景耗时，单位 ms
        /// </summary>
        public int costTime;
        /// <summary>
        /// 自定义维度数据，key在「小程序管理后台」获取。只支持能够通过JSON.stringify序列化的对象，且序列化后长度不超过1024个字符
        /// </summary>
        public Dictionary<string, string> dimension;
        /// <summary>
        /// 自定义指标数据，key在「小程序管理后台」获取。只支持能够通过JSON.stringify序列化的对象，且序列化后长度不超过1024个字符
        /// </summary>
        public Dictionary<string, string> metric;
    }
}
