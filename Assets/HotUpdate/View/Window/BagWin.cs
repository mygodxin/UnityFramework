using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BagWin : Window
{
    public static string Name = "BagWin";
    
    protected override string path()
    {
        return "Assets/Prefab/BagWindow.prefab"; 
    }

    public Button btnClose;
    public TextField txtName;

    protected override string[] eventList()
    {
        return new string[]{

        };
    }
    protected override void onEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case "1":
                break;
        }
    }

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
