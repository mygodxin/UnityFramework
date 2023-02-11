
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 窗口
/// </summary>
public abstract class Window : GComponent
{
    /// <summary>
    /// 窗口显示对象
    /// </summary>
    public GameObject view;
    /// <summary>
    /// 资源路径
    /// </summary>
    protected abstract string path();
    /// <summary>
    /// 窗口数据
    /// </summary>
    public object data;

    protected EventTarget eventTarget;

    bool _inited = false;
    bool _loading = false;
    /// <summary>
    /// 是否展示模态窗
    /// </summary>
    public bool modal = true;
    /// <summary>
    /// 点击空白处关闭
    /// </summary>
    protected bool clickClose = true;
    private GameObject _clickCloseLayer;

    public Window()
    {
    }

    protected override void onAddedToStage(object data)
    {
        this.data = data;
        if (!_inited)
        {
            init();
        }
        else
        {
            OnInited();
        }
    }

    protected override void onRemovedFromStage(object data)
    {
        OnHide();
        DoHideAnimation();
    }

    public void Show(object data = null)
    {
        GRoot.inst.ShowWindow(this, data);
    }

    virtual protected void DoShowAnimation()
    {
        view.transform.localScale.Set(.1f, .1f, .1f);
        var tween = view.transform.DOScale(Vector3.one, 0.25f);
        tween.SetEase(Ease.OutBack);
        tween.onComplete += OnShow;
    }

    void init()
    {
        if (_loading)
            return;
        if (view == null)
        {
            var gameObject = Addressables.LoadAssetAsync<GameObject>(path()).WaitForCompletion();
            view = Object.Instantiate(gameObject);
        }
        view.SetActive(true);

        OnInit();

        OnInited();
    }

    private void OnInited()
    {
        registerEvent();
        AddClickCloseLayer();
        DoShowAnimation();
    }

    public void Hide()
    {
        GRoot.inst.HideWindow(this);
    }

    protected virtual void DoHideAnimation()
    {
        var tween = view.transform.DOScale(new Vector3(.1f, .1f, .1f), 0.25f);
        tween.SetEase(Ease.InBack);
        tween.onComplete += HideImmediately;
    }

    public void HideImmediately()
    {
        removeEvent();
        view.SetActive(false);
    }

    private void AddClickCloseLayer()
    {
        if (clickClose)
        {
            if (_clickCloseLayer == null)
            {
                _clickCloseLayer = new GameObject("clickCloseLayer");
                var canvas = view.transform.Find("Canvas").transform;
                var btn = _clickCloseLayer.AddComponent<Button>();
                var rectTran = _clickCloseLayer.GetComponent<RectTransform>();
                rectTran.sizeDelta = new Vector2(Screen.width, Screen.height);
                _clickCloseLayer.transform.localPosition = Vector3.zero;
                _clickCloseLayer.transform.SetParent(canvas);
                btn.onClick.AddListener(() => { Hide(); });
            }
            _clickCloseLayer.SetActive(true);
        }
        else
        {
            if (_clickCloseLayer != null)
                _clickCloseLayer.SetActive(false);
        }
    }

    Dictionary<string, EventCallback> callbackDic;
    protected virtual string[] eventList()
    {
        return null;
    }
    protected void registerEvent()
    {
        string[] eventList = this.eventList();
        if (eventList != null)
        {
            callbackDic = new Dictionary<string, EventCallback>();
            foreach (var str in eventList)
            {
                EventCallback callback = delegate (object data)
                {
                    onEvent(str, data);
                };
                Facade.inst.On(str, callback);
                callbackDic.Add(str, callback);
            }
        }
    }
    protected void removeEvent()
    {
        string[] eventList = this.eventList();
        if (eventList != null)
        {
            foreach (var str in callbackDic)
            {
                Facade.inst.Off(str.Key, str.Value);
            }
        }
    }
    protected virtual void onEvent(string eventName, object data)
    {
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

    }
    /// <summary>
    /// 打开
    /// </summary>
    protected virtual void OnShow()
    {

    }
    /// <summary>
    /// 关闭
    /// </summary>
    protected virtual void OnHide()
    {

    }
}
