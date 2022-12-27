using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI»ùÀà
/// </summary>
public class GComponent : EventTarget
{
    EventCallback AddToStage;
    EventCallback RemoveToStage;
    GameObject gameObject;

    public GComponent()
    {
        AddToStage += onAddToStage;
    }

    private void createGameObject()
    {
        gameObject = new GameObject();
        gameObject.SetActive(true);
    }

    public virtual void onAddToStage(object param)
    {

    }
}
