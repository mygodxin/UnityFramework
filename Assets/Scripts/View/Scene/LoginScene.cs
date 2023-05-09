using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HS;



public class UnityItem : Attribute
{
    public string Path;
    public UnityItem(string path)
    {
        this.Path = path;
    }

}
public class LoginScene : MonoBehaviour
{
    [UnityItem("xx/xxx")]
    public Button btnStart;
    public Button btnSetting;
    public Button btnExit;
    public TMP_Text txtStart;
    //private int i = 0;
    // Start is called before the first frame update
    void Start()
    {

        foreach (var item in this.GetType().GetFields(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic))
        {
            var att = item.GetCustomAttribute<UnityItem>();
            if (att != null)
            {
                var path = att.Path;


            }


        }

        btnStart.onClick.AddListener(this.OnClickStart);
        btnSetting.onClick.AddListener(OnClickSetting);
        btnExit.onClick.AddListener(OnClickExit);

        //gameObject.AddComponent<GameScene>();

        //Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/HotUpdate.prefab");

        EventManager.Inst.On("test", onEvent);
        //EventManager.Instance.Off("test", onEvent);
        EventManager.Inst.Emit("test", "你好啊");
        //Timers.inst.Add(1, 2, (dt) =>
        //{
        //    Debug.Log("timers");
        //});
        ConfigManager.Inst.Init();
        var player = new Player();
        player.name = "HelloWorld";
        player.age = 11;
        LocalStorage.Save("player", player);
        var p = LocalStorage.Read<Player>("player");
        //Debug.Log(p.name);
        //Debug.Log(p.age);
        var states = new Dictionary<EState, BaseState>
        {
            { EState.walk, new WalkState() },
            { EState.run, new RunState() }
        };
        //var fsm = new FSM(EState.walk, states);
        //while (true)
        //{
        //    fsm.Update();
        //}
    }

    public void onEvent(object param)
    {
        //Debug.Log("测试事件");
        Debug.Log(param);
    }

    public void onEmit()
    {
        EventManager.Inst.Emit(Notifications.UpdateItem, "UpdateItem消息");
    }

    private void OnClickStart()
    {
        //UIManager.inst.ShowAlert("HelloWorld");
        //var go = Addressables.LoadAssetAsync<GameObject>("Assets/AssetsPackage/UI/SettingWin.prefab").WaitForCompletion();
        //Instantiate(go);
        this.txtStart.text = "click2";

        //var login = new LoginAccount();
        //login.Account = "ceshi11";
        //login.Password = "12345678";
        //login.Platform = Platform.Web;
        //// login.Platform = GameManager.Instance.Platform;
        //login.Name = "";
        //login.AvatarUrl = "";
        //login.InvitationCode = "";
        //LoginManager.inst.Login(login);

        //Addressables.LoadSceneAsync("Assets/Scenes/GameScene.unity");
        //var win = GRoot.inst.GetWindow<BagWin>();
        //UIManager.inst.ShowWindow<SettingWin>(123);
        //Facade.inst.Emit(Notification.OpenBag, "OpenBag消息");
        GuideManager.inst.ShowGuide(this.btnSetting.GetComponent<RectTransform>());
    }

    private void OnClickSetting()
    {
        GuideManager.inst.HideGuide();
        //UIManager.inst.ShowWindow<AlertWin>();
        Debug.Log("点击设置1");
    }

    private void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
