
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace UFO
{
    /// <summary>
    /// http请求
    /// </summary>
    public class HttpRequest
    {
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
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        public async Task<Texture> GetTexture(string url)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                return ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }
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
        public async Task<byte[]> Post(string url, string data)
        {
            //.net版http请求
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(3000),
            };
            var content = new StringContent(data, Encoding.UTF8);
            var rep = await client.PostAsync(url, content);
            var repData = await rep.Content.ReadAsStringAsync();
            return Convert.FromBase64String(repData);

            //unity原版http请求
            //var formData = new List<IMultipartFormSection>
            //{
            //    new MultipartFormDataSection(data)
            //};
            //UnityWebRequest wr = UnityWebRequest.Post(url, data);
            //await wr.SendWebRequest();
            //if (wr.result != UnityWebRequest.Result.Success)
            //{
            //    Debug.Log(wr.error);
            //    return null;
            //}
            //else
            //{
            //    // 以文本形式显示结果
            //    return wr.downloadHandler.data;
            //}
        }
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
        private Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            continuation();
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

