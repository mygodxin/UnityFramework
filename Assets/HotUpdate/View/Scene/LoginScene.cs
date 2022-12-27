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

        EventManager.Instance.On("test", onEvent);
        //EventManager.Instance.Off("test", onEvent);
        EventManager.Instance.Emit("test","你好啊");
        Timers.Instance.Add(1, 3, (dt) =>
        {
            Debug.Log("打印");
            Debug.Log(dt);
        });
    }

    public void onEvent(object param)
    {
        Debug.Log("测试事件");
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
