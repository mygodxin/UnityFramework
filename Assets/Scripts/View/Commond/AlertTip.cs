using TMPro;
using UnityFramework;

public class AlertTip : Window
{
    public static string Name = "AlertTip";
    
    protected override string path()
    {
        return "AlertTip"; 
    }

    public TMP_Text txtContent;

    public override void OnInit()
    {
        this.modal = false;
    }

    protected override void OnShow()
    {
        this.txtContent.text = (string)this.data;
        Timer.inst.Add(2, 1, this.Hide);
    }

    protected override void OnHide()
    {
    }
}
