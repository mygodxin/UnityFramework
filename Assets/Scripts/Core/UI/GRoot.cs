using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI¸ù½Úµã
/// </summary>
public class GRoot : GComponent
{
    public readonly int designWidth = 1920;
    public readonly int designHeight = 1080;
    public Dictionary<string, Window> winCache;
    public List<Window> winOpen;
    private GameObject _modalLayer;

    private static GRoot _inst = null;
    public static GRoot inst
    {
        get
        {
            if (_inst == null)
                _inst = new GRoot();
            return _inst;
        }
    }
    public GRoot()
    {
        winCache = new Dictionary<string, Window>();
        winOpen = new List<Window>();
    }

    public Window GetWindow<T>()
    {
        Type type = typeof(T);
        winCache.TryGetValue(type.ToString(), out var win);
        if (win == null)
        {
            var inst = Activator.CreateInstance(type);
            // instHistory.push({ type: type, inst: inst });
            winCache.Add(type.ToString(), inst as Window);
            win = inst as Window;
        }
        return win;
    }

    public void ShowWindow(Window win, object data = null)
    {
        win?.Emit("onAddedToStage", data);
        winOpen.Add(win);
        AdjustModalLayer();
        //var canvas = win.view.transform.Find("Canvas");
        //_modalLayer.transform.SetParent(canvas);
        //_modalLayer.SetActive(true);
    }

    public void HideWindow(Window win)
    {
        win?.Emit("onRemovedFromStage");
        //_modalLayer.SetActive(false);
        winOpen.Remove(win);
        AdjustModalLayer();

        //var canvas = win.view.transform.Find("Canvas");
        //_modalLayer.transform.SetParent(canvas);
    }

    public void ScreenUISelfAdptation(Transform scaleUI)
    {
        float widthrate = Screen.width / 1920.0f;
        float heightrate = Screen.height / 1080.0f;
        float postion_x = scaleUI.GetComponent<RectTransform>().anchoredPosition.x * widthrate;
    }

    public GameObject modalLayer
    {
        get
        {
            if (_modalLayer == null)
                CreateModalLayer();

            return _modalLayer;
        }
    }

    void CreateModalLayer()
    {
        _modalLayer = new GameObject("modalLayer");
        var canvas = GameObject.Find("Canvas").transform;
        var img = _modalLayer.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.5f);
        var rectTran = _modalLayer.GetComponent<RectTransform>();
        rectTran.sizeDelta = new Vector2(Screen.width, Screen.height);
        _modalLayer.transform.SetParent(canvas);
        _modalLayer.transform.localPosition = Vector3.zero;

        //GameObject.Instantiate
    }

    private void AdjustModalLayer()
    {
        if (_modalLayer == null)
            CreateModalLayer();

        int cnt = winOpen.Count;
        var use = false;
        for (int i = cnt - 1; i >= 0; i--)
        {
            var win = winOpen[i];
            if (win.modal)
            {
                use = true;
                if (_modalLayer.transform == null)
                    _modalLayer.SetActive(true);

                var canvas = win.view.transform.Find("Canvas").transform;
                _modalLayer.transform.SetParent(canvas);
                    _modalLayer.transform.SetSiblingIndex(0);
            }
        }
        if (!use)
        {
            _modalLayer.SetActive(false);
        }
    }
}
