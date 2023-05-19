using System;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    public partial class LoginScene : BaseScene
    {
        public static string path = "LoginScene";

        protected override string[] eventList()
        {
            return new string[]{
                Notifications.OpenBag
            };

        }
        protected override void OnEvent(string eventName, object data)
        {
            switch (eventName)
            {
                case Notifications.OpenBag:
                    break;
            }
        }

        protected override void OnInit()
        {
            this.BindComponent(this.gameObject);

            this._startButton.onClick.AddListener(this.OnClickStart);
            this._settingButton.onClick.AddListener(OnClickSetting);
            this._exitButton.onClick.AddListener(OnClickExit);
        }

        protected override void OnShow()
        {
            Debug.Log("打印LoginScene-openData" + this.data);

            var player = new Player();
            player.name = "HelloWorld";
            player.age = 11;
            LocalStorage.Save("player", player);
            var p = LocalStorage.Read<Player>("player");
            //Debug.Log(p.name);
            //Debug.Log(p.age);
        }

        protected override void OnHide()
        {
        }


        private void OnClickStart()
        {
            //UIManager.inst.ShowAlert("HelloWorld");
            //var go = Addressables.LoadAssetAsync<GameObject>("Assets/AssetsPackage/UI/SettingWin.prefab").WaitForCompletion();
            //Instantiate(go);
            this._startTMP.text = "开始游戏";

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
            UIManager.Inst.ShowWindow<SettingWin>(123);
            //Facade.inst.Emit(Notification.OpenBag, "OpenBag消息");
            //GuideManager.inst.ShowGuide(this._settingButton.GetComponent<RectTransform>());
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
    }
}
