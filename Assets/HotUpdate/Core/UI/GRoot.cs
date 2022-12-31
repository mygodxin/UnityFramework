using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI¸ù½Úµã
/// </summary>
public class GRoot : GComponent
{
    public Dictionary<Window, Window> winDic;

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
        winDic = new Dictionary<Window, Window>();
    }

    public void ShowWindow(string type)
    {
        Window win = Activator.CreateInstance(Type.GetType(type)) as Window;
        win.Show();
    }

    public void HideWindow(Window win)
    {
        win.Hide();
    }

    public void ScreenUISelfAdptation(Transform scaleUI)
    {
        float widthrate = UnityEngine.Screen.width / 1920.0f; 
        float heightrate = UnityEngine.Screen.height / 1080.0f;
        float postion_x = scaleUI.GetComponent<RectTransform>().anchoredPosition.x * widthrate;
    }
}
