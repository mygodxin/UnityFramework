using System;
using System.Threading.Tasks;
using HS;

/// <summary>
/// UIπ‹¿Ì¿‡
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
    public async void ShowScene(Type type, object data = null)
    {
        if (this._curScene != null)
        {
            this._curScene.Hide();
        }
        this._curScene = (BaseScene)(await this.ShowWindow(type, data));
    }

    public async void ShowWindow<T>(object data = null)
    {
       await this.ShowWindow(typeof(T), data);
    }

    public async Task<BaseView> ShowWindow(Type type, object data = null)
    {
        return (BaseView)(await UIRoot.Inst.ShowWindow(type, data));
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
