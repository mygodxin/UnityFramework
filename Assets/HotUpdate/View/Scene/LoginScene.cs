using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    public Button btnClick;
    public TextMeshProUGUI txt;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        btnClick.onClick.AddListener(this.OnClick);

        gameObject.AddComponent<GameScene>();

        Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/HotUpdate.prefab");

        Facade.Inst.On("test", onEvent);
        //EventManager.Instance.Off("test", onEvent);
        Facade.Inst.Emit("test","ÄãºÃ°¡");
        Timers.Inst.Add(1, 2, (dt) =>
        {
            Debug.Log("timers");
        });

        var player = new Player();
        player.Name = "HelloWorld";
        player.Age = 11;
        LocalStorage.Save("player", player);
        var p = LocalStorage.Read<Player>("player");
        Debug.Log(p.Name);
        Debug.Log(p.Age);

        GRoot.Instance.ShowWindow(BagWin.Name, 123);
    }

    public void onEvent(object param)
    {
        Debug.Log("²âÊÔÊÂ¼þ");
        Debug.Log(param);
    }

    private void OnClick()
    {
        txt.text = "regeng" + i++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
