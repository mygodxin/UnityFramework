
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace HS
{
    /// <summary>
    /// http请求
    /// </summary>
    public class HttpRequest
    {
        public static HttpRequest Inst = new HttpRequest();

        /// <summary>
        /// 下载图片
        /// </summary>
        public async Task<Texture2D> GetTexture(string url)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log(www.error);
                return null;
            }
            else
            {
                return ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> Get(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                // 以文本形式显示结果
                return www.downloadHandler.text;
            }
        }



        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> GetBinary(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                // 以文本形式显示结果
                return www.downloadHandler.data;
            }
        }








        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<byte[]> Post(string url, string data)
        {
            //contentType说明
            //application/json： 该 Content-Type 用于指定请求正文为 JSON 格式。通常在使用 RESTful API 时常见。
            //application/X-www-form-urlencoded：这是默认的 Content-Type 类型，用于发送表单数据。在 POST 请求中，表单数据将作为键值对的形式发送到服务器。
            //multipart/form-Data：当需要发送包含文件上传的表单数据时，使用 multipart/form-Data。这种 Content-Type 可以用于上传文件和其他表单字段。
            //text/plain：当请求正文为纯文本时，可以使用 text/plain。这种 Content-Type 不会对文本进行任何特殊处理。
            UnityWebRequest wr = UnityWebRequest.Post(url, data, "text/plain");
            await wr.SendWebRequest();
            if (wr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(wr.error);
                return null;
            }
            else
            {
                // 以文本形式显示结果
                return wr.downloadHandler.data;
            }
        }

        /// <summary>
        /// put请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyData"></param>
        /// <returns></returns>
        public async Task<bool> Put(string url, byte[] bodyData)
        {
            UnityWebRequest www = UnityWebRequest.Put(url, bodyData);
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                return false;
            }
            else
            {
                Debug.Log("Upload complete!");
                return true;
            }
        }
    }

    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation asyncOp;
        private Action Continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.Continuation += continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            Continuation();
        }
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}

