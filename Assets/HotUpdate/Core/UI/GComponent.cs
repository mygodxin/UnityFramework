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

    public GComponent()
    {
        //AddToStage += onAddToStage;
    }

    private void createGameObject()
    {
        gameObject = Addressables.LoadAssetAsync<GameObject>(Path).WaitForCompletion();
        Object.Instantiate(gameObject);
        gameObject.SetActive(true);
        this.AddToStage(1);
    }

    public virtual void onInit()
    {

    }

    public virtual void OnShow()
    {

    }

    public virtual void OnHide()
    {

    }
}
