using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// µÇÂ¼¹ÜÀí
/// </summary>
public class UIManager
{
    private static UIManager _inst = null;
    public static UIManager inst
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

    public void ShowWindow<T>(object data = null)
    {
        var win = GRoot.inst.GetWindow<T>();
        GRoot.inst.ShowWindow(win, data);
    }

    public void ShowAlert()
    {

    }

    public void ShowTip()
    {

    }
}
