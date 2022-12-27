using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI¸ù½Úµã
/// </summary>
public class GRoot : GComponent
{

    public GRoot()
    {

    }

    public void ShowWindow(Window win)
    {
       // win.onAddToStage();
    }

    public void HideWindow(Window win)
    {
        win.OnHide();
    }
}
