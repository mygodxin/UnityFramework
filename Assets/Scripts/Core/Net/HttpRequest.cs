
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// http请求封装
/// </summary>
public class HttpRequest
{
    private GameObject _gameObject;
    private HttpRequestMono _mono;
    private static HttpRequest _inst = null;
    public static HttpRequest inst
    {
        get
        {
            if (_inst == null)
                _inst = new HttpRequest();
            return _inst;
        }
    }
    public HttpRequest()
    {
        _gameObject = new GameObject();
        _gameObject.hideFlags = HideFlags.HideInHierarchy;
        _gameObject.SetActive(true);
        UnityEngine.Object.DontDestroyOnLoad(_gameObject);
        _mono = _gameObject.AddComponent<HttpRequestMono>();
    }
    /// <summary>
    /// 下载图片
    /// </summary>
    public void GetTexture(string url, Action<Texture> callback)
    {
        _mono.GetTexture(url, callback);
    }
    public void Get(string url, Action<string> callback)
    {
        _mono.Get(url, callback);
    }
    public void Post(string url, List<IMultipartFormSection> formData, Action<string> callback)
    {
        _mono.Post(url, formData, callback);
    }
    public void Upload(string url, List<IMultipartFormSection> formData)
    {
        _mono.Upload(url, formData);
    }
    public void Put(string url, byte[] bodyData)
    {
        _mono.Put(url, bodyData);
    }
}
class HttpRequestMono : MonoBehaviour
{
    public void Get(string url, Action<string> callback)
    {
        StartCoroutine(_Get(url, callback));
    }
    IEnumerator _Get(string url, Action<string> callback)
    {
        UnityWebRequest wr = UnityWebRequest.Get(url);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            // 以文本形式显示结果
            Debug.Log(wr.downloadHandler.text);

            // 或者获取二进制数据形式的结果
            byte[] results = wr.downloadHandler.data;
            callback.Invoke(wr.downloadHandler.text);
        }
    }

    public void GetTexture(string url, Action<Texture> callback)
    {
        StartCoroutine(_GetTexture(url, callback));
    }
    IEnumerator _GetTexture(string url, Action<Texture> callback)
    {
        UnityWebRequest wr = UnityWebRequestTexture.GetTexture(url);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            Texture myTexture = DownloadHandlerTexture.GetContent(wr);
            callback.Invoke(myTexture);
        }
    }

    public void Post(string url, List<IMultipartFormSection> formData, Action<string> callback)
    {
        StartCoroutine(_Post(url, formData, callback));
    }
    IEnumerator _Post(string url, List<IMultipartFormSection> formData, Action<string> callback)
    {
        UnityWebRequest wr = UnityWebRequest.Post(url, formData);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            // 以文本形式显示结果
            Debug.Log(wr.downloadHandler.text);

            // 或者获取二进制数据形式的结果
            byte[] results = wr.downloadHandler.data;
            callback.Invoke(wr.downloadHandler.text);
        }
    }

    public void Upload(string url, List<IMultipartFormSection> formData)
    {
        StartCoroutine(_Upload(url, formData));
    }
    IEnumerator _Upload(string url, List<IMultipartFormSection> formData)
    {
        UnityWebRequest wr = UnityWebRequest.Post(url, formData);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    public void Put(string url, byte[] bodyData)
    {
        StartCoroutine(_Put(url, bodyData));
    }
    IEnumerator _Put(string url, byte[] bodyData)
    {
        UnityWebRequest wr = UnityWebRequest.Put(url, bodyData);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            Debug.Log("upload complete!");
        }
    }
}

