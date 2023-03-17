using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;

public class ServerWin : Window
{
    public static string Name = "ServerWin";

    private Action<ClientServerInfo> _action;
    private TMP_Text _txtAccount;
    private TMP_Text _txtPassword;

    protected override string path()
    {
        return "Assets/Prefab/ServerWin.prefab";
    }

    public Button btnClose;
    public Button btnOpen;

    protected override string[] eventList()
    {
        return new string[]{
            Notifications.UpdateItem,
            Notifications.OpenBag
        };
    }
    protected override void onEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Notifications.UpdateItem:
                Debug.Log("收到消息");
                Debug.Log(data);
                break;
            case Notifications.OpenBag:
                Debug.Log("收到消息");
                Debug.Log(data);
                break;
        }
    }

    public override void OnInit()
    {
        _txtAccount = this.GetTextTMP("txtAccount");
        _txtPassword = this.GetTextTMP("txtPassword");

        btnClose = this.GetButton("btnClose");
        btnClose.onClick.AddListener(OnClickClose);
    }

    protected override void OnShow()
    {
        Debug.Log("BagWin OnShow");
        //view.
        this._action = (Action<ClientServerInfo>)this.data;
    }

    protected override void OnHide()
    {
        Debug.Log("BagWin OnHide");
    }

    private void OnClickClose()
    {
        Hide();
    }
    private void OnClickOpen()
    {
        //var win = GRoot.inst.GetWindow<BackGround>();
        //GRoot.inst.ShowWindow(win);
    }
}
