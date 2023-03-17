using System;
using TMPro;
using UnityEngine.UI;
using UnityFramework;

public class AlertParam
{
    public string title;
    public string content;
    public Action leftCallback;
    public Action rightCallback;
    public Action closeCallback;
}

public class AlertWin : Window
{
    public static string Name = "AlertWin";

    protected override string path()
    {
        return "AlertWin";
    }

    public Button btnClose;
    public Button btnLeft;
    public Button btnRight;
    public TMP_Text txtContent;
    public TMP_Text txtTitle;

    public override void OnInit()
    {
        btnClose.onClick.AddListener(OnClickClose);
        btnLeft.onClick.AddListener(OnClickLeft);
        btnRight.onClick.AddListener(OnClickRight);
    }
    public void OnClickClose()
    {
        Hide();
        var param = (AlertParam)this.data;
        if (param.closeCallback != null)
            param.closeCallback.Invoke();
    }
    public void OnClickLeft()
    {
        Hide();
        var param = (AlertParam)this.data;
        if (param.leftCallback != null)
            param.leftCallback.Invoke();
    }
    public void OnClickRight()
    {
        Hide();
        var param = (AlertParam)this.data;
        if (param.rightCallback != null)
            param.rightCallback.Invoke();
    }

    protected override void OnShow()
    {
        var param = (AlertParam)this.data;
        this.txtContent.text = param.content;
        this.txtTitle.text = param.title;
    }

    protected override void OnHide()
    {
    }
}
