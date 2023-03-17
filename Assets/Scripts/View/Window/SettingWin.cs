using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;

public class SettingWin : Window
{
    public static string Name = "SettingWin";
    
    protected override string path()
    {
        return "Assets/UI/SettingWin.prefab"; 
    }

    public Button btnClose;
    public Button btnOpen;
    public Button openBag;
    public TMP_Text txtName;

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
        //btnClose = this.GetButton("btnClose");
        btnClose.onClick.AddListener(onClick);
        openBag.onClick.AddListener(() =>
        {
            UIManager.inst.ShowWindow<BagWin>();
        });
        this.txtName.text = "78910";
    }

    public void onClick()
    {
        Hide();
    }

    protected override void OnShow()
    {
        Debug.Log("BagWin OnShow");
        //view.
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
