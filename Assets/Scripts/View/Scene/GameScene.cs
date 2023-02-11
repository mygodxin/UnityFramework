using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GList list;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("这个脚本是通过代码AddComponent直接创建的");

        var aStar = new AStar();
        Grid[,] map = new Grid[10, 10];
        var obstacles = new List<int> { 2, 4, 6, 8, 10 };
        for (int i = 0; i < 100; i++)
        {
            var grid = new Grid();
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
        var path = aStar.FindPath(map[0, 0], map[9, 5]);
        //Debug.Log("找到路径");
        //for (int i = 0; i < path.Count; i++)
        //{
        //    Debug.Log("[x=" + path[i].x + ",y=" + path[i].y + "]");
        //}
    }
    private void OnItemRenderer(int index,GameObject gameObject)
    {
        //Debug.Log("绘制" + index);
        var text = gameObject.transform.Find("Text (TMP)");
        var t = text.GetComponentInChildren<TMP_Text>();
        t.text = index + "";
    }
    // Update is called once per frame
    void Update()
    {

    }
}
