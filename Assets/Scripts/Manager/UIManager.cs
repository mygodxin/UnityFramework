using System;
using System.Threading.Tasks;
using HS;

/// <summary>
/// UI管理类
/// </summary>
public class UIManager
{
    private BaseScene _curScene;
    private static UIManager _inst = null;
    public static UIManager Inst
    {
        get
        {
            if (_inst == null)
                _inst = new UIManager();
            return _inst;
        }
    }

    public void Init()
    {

    }
    public void ShowScene<T>(object data = null)
    {
        this.ShowScene(typeof(T), data);
    }
    public void ShowScene(Type type, object data = null)
    {
        if (this._curScene != null)
        {
            this._curScene.Hide();
        }
        this._curScene = this.ShowWindow(type, data);
    }

    public void ShowWindow<T>(object data = null)
    {
       this.ShowWindow(typeof(T), data);
    }

    public void ShowWindow(Type type, object data = null)
    {
        UIRoot.Inst.ShowWindow(type, data);
    }
    /// <summary>
    /// 打开弹窗
    /// </summary>
    /// <param name="param"></param>
    public void ShowAlert(AlertParam param)
    {
        this.ShowWindow<AlertWin>(param);
    }
    /// <summary>
    /// 打开弹窗
    /// </summary>
    /// <param name="content">弹窗内容</param>
    public void ShowAlert(string content)
    {
        var alertParam = new AlertParam();
        alertParam.content = content;
        this.ShowWindow<AlertWin>(alertParam);
    }
    /// <summary>
    /// 打开提示界面
    /// </summary>
    /// <param name="content">提示文本</param>
    public void ShowTip(string content)
    {
        this.ShowWindow<AlertTip>(content);
    }
}
