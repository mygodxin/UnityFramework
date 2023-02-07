using System;
using System.ComponentModel;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine.Networking;

public class HttpRequest
{

    private static HttpRequest inst = null;
    public static HttpRequest Inst
    {
        get
        {
            if (inst == null)
                inst = new HttpRequest();
            return inst;
        }
    }

    public void Request()
    {
        UnityWebRequest wr = new UnityWebRequest(); // 完全为空
        UnityWebRequest wr2 = new UnityWebRequest("https://www.mysite.com"); // 设置目标 URL

        // 必须提供以下两项才能让 Web 请求正常工作
        wr.url = "https://www.mysite.com";
        wr.method = UnityWebRequest.kHttpVerbGET;   // 可设置为任何自定义方法，提供了公共常量

        wr.useHttpContinue = false;
        wr.chunkedTransfer = false;
        wr.redirectLimit = 0;  // 禁用重定向
        wr.timeout = 60;       // 此设置不要太小，Web 请求需要一些时间


        var handler = wr.SendWebRequest();
        handler.completed += (a)=>
        {
            if(wr.result == UnityWebRequest.Result.Success)
            {

            }
        };
    }
}
