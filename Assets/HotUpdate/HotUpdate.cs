using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotUpdate : MonoBehaviour
{
    public Button btnClick;
    public TextMeshProUGUI txt;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        btnClick.onClick.AddListener(this.OnClick);

        Debug.Log("加载DLL");

        Debug.Log("复制");
        gameObject.AddComponent<GameScene>();

        Debug.Log("=======看到此条日志代表你成功运行了示例项目的热更新代码1=======");
    }

    private void OnClick()
    {
        txt.text = "firstsss" + i;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
