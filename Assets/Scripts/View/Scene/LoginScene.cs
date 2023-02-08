using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnExit;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        btnStart.onClick.AddListener(this.OnClickStart);
        btnSetting.onClick.AddListener(OnClickSetting);
        btnExit.onClick.AddListener(OnClickExit);

        gameObject.AddComponent<GameScene>();

        Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/HotUpdate.prefab");

        Facade.inst.On("test", onEvent);
        //EventManager.Instance.Off("test", onEvent);
        Facade.inst.Emit("test", "你好啊");
        Timers.inst.Add(1, 2, (dt) =>
        {
            Debug.Log("timers");
        });

        var player = new Player();
        player.name = "HelloWorld";
        player.age = 11;
        LocalStorage.Save("player", player);
        var p = LocalStorage.Read<Player>("player");
        Debug.Log(p.name);
        Debug.Log(p.age);
        var states = new Dictionary<EState, StateBase>
        {
            { EState.walk, new WalkState() },
            { EState.run, new RunState() }
        };
        //var fsm = new FSM(EState.walk, states);
        //while (true)
        //{
        //    fsm.Update();
        //}

        var aStar = new AStar();
        Grid[,] map = new Grid[10, 10];
        for (int i = 0; i < 100; i++)
        {
            var grid = new Grid();
            grid.x = i % 10;
            grid.y = Mathf.FloorToInt(i / 10);
            grid.type = GridType.normal;
            map[i % 10, Mathf.FloorToInt(i / 10)] = grid;
        }
        aStar.CreateMap(map);
        var path = aStar.FindPath(map[0, 0], map[9, 9]);
        Debug.Log("找到路径");
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log("[x=" + path[i].x + ",y=" + path[i].y + "]");
        }
    }

    public void onEvent(object param)
    {
        Debug.Log("测试事件");
        Debug.Log(param);
    }

    public void onEmit()
    {
        Facade.inst.Emit(Notification.UpdateItem, "UpdateItem消息");
    }

    private void OnClickStart()
    {
        var win = GRoot.inst.GetWindow<BagWin>();
        GRoot.inst.ShowWindow(win, 123);

        Facade.inst.Emit(Notification.OpenBag, "OpenBag消息");
    }

    private void OnClickSetting()
    {
        var win = GRoot.inst.GetWindow<SettingWin>();
        GRoot.inst.ShowWindow(win, 123);
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
