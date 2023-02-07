using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagWin : Window
{
    public static string Name = "BagWin";
    
    protected override string path()
    {
        return "Assets/Prefab/BagWin.prefab"; 
    }

    public Button btnClose;
    public Button btnOpen;
    public GList list;

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
                Debug.Log("�յ���Ϣ");
                Debug.Log(data);
                break;
            case Notification.OpenBag:
                Debug.Log("�յ���Ϣ");
                Debug.Log(data);
                break;
        }
    }

    public override void OnInit()
    {
        btnClose = view.transform.Find("Canvas/btnClose").GetComponent<Button>();
        btnClose.onClick.AddListener(onClick);
        list = view.transform.Find("Canvas/Scroll View").GetComponent<GList>();
        list.itemRenderer = ItemRenderer;
    }

    private void ItemRenderer(int index, GameObject item)
    {
        GameObject text = item.transform.Find("txt").gameObject;
        Debug.Log(text);
        var t = text.GetComponent<TextMeshPro>();

        t.text = index + "";
    }

    public void onClick()
    {
        Hide();
    }

    protected override void OnShow()
    {
        Debug.Log("BagWin OnShow");
        list.data = new int[1, 2, 3, 4, 5,6,7,8,9,10,11];
        list.numItems = 5;
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
