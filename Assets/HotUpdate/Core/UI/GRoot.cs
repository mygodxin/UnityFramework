using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI¸ù½Úµã
/// </summary>
public class GRoot : GComponent
{
    public readonly int DesignWidth = 1920;
    public readonly int DesignHeight = 1080;
    public Dictionary<string, Window> winDic;

    private static GRoot instance = null;
    public static GRoot Instance
    {
        get
        {
            if (instance == null)
                instance = new GRoot();
            return instance;
        }
    }
    public GRoot()
    {
        winDic = new Dictionary<string, Window>();
    }

    public Window getWindow(string winName)
    {
        winDic.TryGetValue(winName, out var win);
        if (win == null)
        {
            Type classType = Type.GetType(winName, true);
            var inst = Activator.CreateInstance(classType);
            // instHistory.push({ type: type, inst: inst });
            winDic.Add(winName, inst as Window);
        }
        return win;
    }

    public void ShowWindow(Window win, object data = null)
    {
        win?.Emit("onAddedToStage", data);
    }

    public void HideWindow(Window win)
    {
        win?.Emit("onRemovedFromStage");
    }

    public void ScreenUISelfAdptation(Transform scaleUI)
    {
        float widthrate = UnityEngine.Screen.width / 1920.0f;
        float heightrate = UnityEngine.Screen.height / 1080.0f;
        float postion_x = scaleUI.GetComponent<RectTransform>().anchoredPosition.x * widthrate;
    }
}
