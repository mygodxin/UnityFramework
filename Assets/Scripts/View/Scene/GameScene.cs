using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UFO;

public class GameScene : MonoBehaviour
{
    public GList list;
    public SkeletonGraphic skeletonGraphic;
    public Button btnReturn;
    private QuadTree quadTree;
    public GameObject go;
    private List<GameObject> enemys;
    public GameObject player;
    public Joystick joystick;
    public float speed = 200;
    public Button btnA;
    public Button btnB;
    void Start()
    {

        AudioManager.inst.PlayMusic("bg");
        Debug.Log("这个脚本是通过代码AddComponent直接创建的");
        //var go = Instantiate(gameObject1);
        //var animator = gameObject1.GetComponent<Animator>();
        //animator.Play("中毒",0);
        //animator.StartPlayback();
        //animator.transform.position = new Vector3(0, 0, 1);
        //go.transform.SetParent(GameObject.Find("Canvas/Tex").GetComponent<Image>().transform);
        SpineManager.inst.PlaySpine(skeletonGraphic, "Assets/Textures/Spines/萨满1/tangsanzang", "", "attack", true, true, 0);
        this.btnReturn.onClick.AddListener(() => { Addressables.LoadSceneAsync("Assets/Scenes/LoginScene.unity"); });
        var evt = skeletonGraphic.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.callback.AddListener((a) =>
        {
            AudioManager.inst.PlayEffect("兵种进阶 1");
        });
        entry.eventID = EventTriggerType.PointerClick;
        evt.triggers.Add(entry);

        var aStar = new AStar();
        UFO.Grid[,] map = new UFO.Grid[10, 10];
        var obstacles = new List<int> { 2, 4, 6, 8, 10, 22, 32, 44 };
        for (int i = 0; i < 100; i++)
        {
            var grid = new UFO.Grid();
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
        //var tex = await HttpRequest.inst.GetTexture("http://192.168.1.17:82/web-desktop/splash.85cfd.png");
        //var img = GameObject.Find("Canvas/Tex").GetComponent<Image>();
        //Debug.Log("下载完成" + tex.name);
        //img.sprite = Sprite.Create(tex as Texture2D, new UnityEngine.Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        var canvas = GameObject.Find("Canvas").transform;
        var rect = GameObject.Find("Canvas/Image").transform.GetComponent<RectTransform>();
        var r = new Rect((rect.position.x + rect.rect.x) * canvas.localScale.x, (rect.position.y + rect.rect.y) * canvas.localScale.y, rect.rect.width * canvas.localScale.x, rect.rect.height * canvas.localScale.y);
        quadTree = new QuadTree(r, 0);
        enemys = new List<GameObject>();
        for (var i = 0; i < 10; i++)
        {
            var g = Instantiate(go);
            g.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = "" + i;
            g.name = "敌人" + i;
            g.transform.SetParent(canvas);
            g.transform.position = new Vector3(UnityEngine.Random.Range(50, rect.rect.width / 2), UnityEngine.Random.Range(50, rect.rect.height / 2));
            enemys.Add(g);
        }

        //摇杆
        this.joystick.onPointerDown = this.OnPointerDown;
        this.joystick.onPointerUp = this.OnPointerUp;
        this.joystick.onPointerMove = this.OnPointerMove;
    }
    private void OnPointerDown(Vector2 position)
    {
        //this.player.transform.localPosition = position;
    }
    private void OnPointerUp(Vector2 position)
    {
        //this.player.transform.localPosition = position;
    }
    private void OnPointerMove(Vector2 vector)
    {
        if (vector.magnitude != 0)
        {
            player.transform.localPosition += new Vector3(vector.x, vector.y) * this.speed * Time.deltaTime;
            //this.player .transform.Translate(vector * this.speed * Time.deltaTime);
            //this.player. transform.rotation = Quaternion.LookRotation(new Vector3(vector.x, vector.y, 0));
        }
        //this.player.transform.localPosition = position;
    }
    private void OnItemRenderer(int index, GameObject gameObject)
    {
        //Debug.Log("绘制" + index);
        var text = gameObject.transform.Find("Text (TMP)");
        var t = text.GetComponentInChildren<TMP_Text>();
        var data = list.data as UFO.Grid[,];
        var img = gameObject.transform.Find("Image").GetComponent<Image>();
        img.color = data[index % 10, Mathf.FloorToInt(index / 10)].type == GridType.obstacle ? Color.red : Color.white;
        t.text = index + "";
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    player.transform.position = Input.mousePosition;
        //    //Debug.Log("点击位置" + mouseSToW);        //foreach(var k in result)
        //    //{
        //    //    Debug.Log(k.name);
        //    //}
        //    if (quadTree == null) return;
        //    var result = quadTree.Retrieve(player.transform.GetComponent<RectTransform>());
        //    Debug.Log("本次点击---------------------------------");
        //    foreach(var k in result)
        //    {
        //        Debug.Log("敌人" + k.name);
        //    }
        //}
    }
    public void OnDrawGizmos()
    {
        var canvas = GameObject.Find("Canvas").transform;
        var rect = GameObject.Find("Canvas/Image").transform.GetComponent<RectTransform>();
        var x = (rect.position.x + rect.rect.x) * canvas.localScale.x;
        var y = (rect.position.y + rect.rect.y) * canvas.localScale.y;
        var r = new Rect(0, 0, rect.rect.width * canvas.localScale.x, rect.rect.height * canvas.localScale.y);
        quadTree = new QuadTree(r, 0);
        quadTree.Clear();

        if (enemys != null && enemys.Count >0)
        {
            for (var i = 0; i < 10; i++)
            {
                quadTree.Insert(enemys[i].transform.GetComponent<RectTransform>());
            }
        }

        Gizmos.color = Color.blue;
        var tr = player.transform.GetComponent<RectTransform>();
        var r1 = new Rect(tr.position.x + tr.rect.x, tr.position.y + tr.rect.y, tr.rect.width, tr.rect.height);
        Gizmos.DrawWireCube(r1.center, r1.size);
        Gizmos.color = Color.red;
        quadTree.DrawLine();
    }
}
