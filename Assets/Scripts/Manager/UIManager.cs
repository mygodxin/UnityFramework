using System;
using HS;

/// <summary>
/// UIπ‹¿Ì¿‡
/// </summary>
public class UIManager
{
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
        UIRoot.Inst.ShowScene(type, data);
    }

    public void ShowWindow<T>(object data = null)
    {
        this.ShowWindow(typeof(T), data);
    }

    public void ShowWindow(Type type, object data = null)
    {
        UIRoot.Inst.ShowWindow(type, data);
    }

    public void ShowAlert(AlertParam param)
    {
        this.ShowWindow<AlertWin>(param);
    }
    public void ShowAlert(string content)
    {
        var alertParam = new AlertParam();
        alertParam.content = content;
        this.ShowWindow<AlertWin>(alertParam);
    }

    public void ShowTip(string content)
    {
        this.ShowWindow<AlertTip>(content);
    }
}
