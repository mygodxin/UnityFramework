using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertWin : Window
{
    public static string Name = "AlertWin";
    
    protected override string path()
    {
        return "Assets/Prefab/AlertWin.prefab"; 
    }

    public Button btnClose;
    public Button btnLeft;
    public Button btnRight;

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
                Debug.Log("�յ���Ϣ");
                Debug.Log(data);
                break;
            case Notifications.OpenBag:
                Debug.Log("�յ���Ϣ");
                Debug.Log(data);
                break;
        }
    }

    public override void OnInit()
    {
        btnClose = view.transform.Find("Canvas/btnClose").GetComponent<Button>();
        btnClose.onClick.AddListener(OnClickClose);

        btnLeft = view.transform.Find("Canvas/btnLeft").GetComponent<Button>();
        btnLeft.onClick.AddListener(OnClickLeft);

        btnRight = view.transform.Find("Canvas/btnRight").GetComponent<Button>();
        btnRight.onClick.AddListener(OnClickRight);
    }
    public void OnClickClose()
    {
        Hide();
    }
    public void OnClickLeft()
    {
        Debug.Log("点击左按钮");
    }
    public void OnClickRight()
    {
        Debug.Log("点击右按钮");
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
