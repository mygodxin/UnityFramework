using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// UI»ùÀà
/// </summary>
public class GComponent : EventTarget
{
    EventCallback AddToStage;
    EventCallback RemoveToStage;
    GameObject gameObject;
    public string Path;
    public object Param;
    bool inited = false;
    bool loading = false;

    public GComponent()
    {
        //AddToStage += onAddToStage;
    }

    public void Show()
    {
        if (!inited)
        {
            init();
        }
        else
        {
            DoShowAnimation();
        }
    }

    virtual protected void DoShowAnimation()
    {
        OnShow();
    }

    void init()
    {
        if (loading)
            return;
        gameObject = Addressables.LoadAssetAsync<GameObject>(Path).WaitForCompletion();
        Object.Instantiate(gameObject);
        gameObject.SetActive(true);

        OnInit();

        DoShowAnimation();
    }

    public void Hide()
    {

    }

    public virtual void OnInit()
    {

    }

    protected virtual void OnShow()
    {

    }

    protected virtual void OnHide()
    {

    }
}
