# UnityFramework

简易Unity框架，方便开发。由HybridCLR热更Code+YooAsset热更Bundle+Luban配置工具

一、UI开发流程
支持UI自动绑定，快捷键为F1，可参考UIBind.cs中的代码，可再CollectSetting中自定义配置,格式类似于Game_Button。
比如新增LoginWindow.cs界面文件，生成的UI代码定义有预制体的路径，支持"UIRoot.Inst.ShowWindow<LoginWindow>(自定义参数);"这种快捷打开UI的方式，
自动加载，同时也会处理ModalLayer这种放在UI下面的半透明黑色背景的层级

二、部分文件说明：
1.GList.cs 一个支持列表项不等宽或高无限循环滚动的列表组件，同时使用对象池优化超大数量列表项
2.GProgresBar.cs 简化版Slider，更方便使用
3.AlignTool.cs UI对齐工具，方便拼UI使用
4.MovieClip.cs 序列帧动画组件
5.CompressFont.cs 压缩TextMeshPro生成的中文字体png
6.Redpoint.cs 红点组件
7.GuideBase.cs 新手引导的反向遮罩，有圆形和方矩形两种形状 
8.QuadTree.cs 四叉树碰撞检测
9.Joystick.cs 遥感
10.StateMachine.cs 状态机
11.Timer.cs 计时器，实现了类似TS中的SetTimeOut和SetInterval这类方法
12.LocalStorage.cs 本地数据存储
