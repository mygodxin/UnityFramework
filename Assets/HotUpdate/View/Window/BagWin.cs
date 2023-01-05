using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BagWin : Window
{
    public static string Name = "BagWin";
    
    protected override string path()
    {
        return "Assets/Prefab/BagWin.prefab"; 
    }

    public Button btnTest;
    public Button btnClose;


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
        //Debug.Log(view);
        //btnTest = view.transform.Find("Canvas/btnTest").GetComponent<Button>();
        //btnTest.onClick.AddListener(onClick);
        var rootVisualElement = view.GetComponent<UIDocument>().rootVisualElement;

        //matchOverLabel = rootVisualElement.Q<Label>("match-over-label");

        //winnerIsLabel = rootVisualElement.Q<Label>("winner-is-label");

        //winnerTeamNameLabel = rootVisualElement.Q<Label>("winner-label");

        btnClose = rootVisualElement.Q<Button>("btnClose");
        Debug.Log(1);
        // Attaching callback to the button.
        btnClose.RegisterCallback<ClickEvent>(ev => OnMainMenuButton());
    }

    public void onClick()
    {
        Debug.Log("µã»÷²âÊÔ");
    }

    protected override void OnShow()
    {
        Debug.Log("BagWin OnShow");
    }

    protected override void OnHide()
    {
        Debug.Log("BagWin OnHide");
    }

    private void OnMainMenuButton()
    {
        Hide();
    }
}
