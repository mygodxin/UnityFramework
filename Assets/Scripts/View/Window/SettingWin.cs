using UnityEngine;
using UnityEngine.UI;

public class SettingWin : Window
{
    public static string Name = "SettingWin";
    
    protected override string path()
    {
        return "Assets/Prefab/SettingWin.prefab"; 
    }

    public Button btnClose;
    public Button btnOpen;

    protected override string[] eventList()
    {
        return new string[]{
            Notification.UpdateItem,
            Notification.OpenBag
        };
    }
    protected override void onEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Notification.UpdateItem:
                Debug.Log("收到消息");
                Debug.Log(data);
                break;
            case Notification.OpenBag:
                Debug.Log("收到消息");
                Debug.Log(data);
                break;
        }
    }

    public override void OnInit()
    {
        btnClose = view.transform.Find("Canvas/btnClose").GetComponent<Button>();
        btnClose.onClick.AddListener(onClick);
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
