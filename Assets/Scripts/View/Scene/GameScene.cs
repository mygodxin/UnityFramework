using DuiChongServerCommon.ClientProtocol;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityFramework;

public class GameScene : MonoBehaviour
{
    public GList list;
    public GameObject gameObject1;
    public SkeletonGraphic skeletonGraphic;
    // Start is called before the first frame update
    async void Start()
    {
        AudioManager.inst.PlayMusic("bg");

        Debug.Log("这个脚本是通过代码AddComponent直接创建的");
        //var go = Instantiate(gameObject1);
        //var animator = gameObject1.GetComponent<Animator>();
        //animator.Play("中毒",0);
        //animator.StartPlayback();
        //animator.transform.position = new Vector3(0, 0, 1);
        //go.transform.SetParent(GameObject.Find("Canvas/Tex").GetComponent<Image>().transform);
        SpineManager.inst.PlaySpine(skeletonGraphic, "Assets/AssetsPackage/Spines/萨满1/tangsanzang", "", "attack", true, true,0);

        var evt = skeletonGraphic.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.callback.AddListener((a) =>
        {
            AudioManager.inst.PlayEffect("兵种进阶 1");
        });
        entry.eventID = EventTriggerType.PointerClick;
        evt.triggers.Add(entry);

        var aStar = new AStar();
        UnityFramework.Grid[,] map = new UnityFramework.Grid[10, 10];
        var obstacles = new List<int> { 2, 4, 6, 8, 10, 22, 32, 44 };
        for (int i = 0; i < 100; i++)
        {
            var grid = new UnityFramework.Grid();
            grid.x = i % 10;
            grid.y = Mathf.FloorToInt(i / 10);
            grid.type = obstacles.IndexOf(i) >= 0 ? GridType.obstacle : GridType.normal;
            map[i % 10, Mathf.FloorToInt(i / 10)] = grid;
        }
        list.itemRenderer = OnItemRenderer;
        list.SetVirtual();
        list.data = map;
        list.numItems = map.Length;
        aStar.CreateMap(map);
        var path = aStar.FindPath(map[0, 0], map[2, 0]);
        Debug.Log("找到路径");
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log("[x=" + path[i].x + ",y=" + path[i].y + "]");
            var ig = list.children[path[i].x + path[i].y * 10].transform.Find("Image").GetComponent<Image>();
            ig.color = Color.blue;
        }
        var tex = await HttpRequest.inst.GetTexture("http://192.168.1.17:82/web-desktop/splash.85cfd.png");
        var img = GameObject.Find("Canvas/Tex").GetComponent<Image>();
        Debug.Log("下载完成" + tex.name);
        img.sprite = Sprite.Create(tex as Texture2D, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
    private void OnItemRenderer(int index, GameObject gameObject)
    {
        //Debug.Log("绘制" + index);
        var text = gameObject.transform.Find("Text (TMP)");
        var t = text.GetComponentInChildren<TMP_Text>();
        var data = list.data as UnityFramework.Grid[,];
        var img = gameObject.transform.Find("Image").GetComponent<Image>();
        img.color = data[index % 10, Mathf.FloorToInt(index / 10)].type == GridType.obstacle ? Color.red : Color.white;
        t.text = index + "";
    }
    // Update is called once per frame
    void Update()
    {
    }
}
