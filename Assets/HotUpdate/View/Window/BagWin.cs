using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BagWin : Window
{
    public static string Name = "BagWin";
    public BagWin() : base()
    {
        Path = "Assets/Prefab/BagWindow.prefab";
    }
    public Button btnClose;
    public TextField txtName;

    public override void OnInit()
    {
        Debug.Log("BagWin OnInit");
    }

    protected override void OnShow()
    {
        Debug.Log("BagWin OnShow");
    }

    protected override void OnHide()
    {
        Debug.Log("BagWin OnHide");
    }
}
