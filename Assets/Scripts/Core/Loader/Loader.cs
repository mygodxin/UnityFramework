using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace HS
{
    /// <summary>
    /// 资源加载类
    /// </summary>
    class Loader
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="location"></param>
        /// <returns></returns>
        public static async Task<TObject> LoadAssetAsync<TObject>(object key)
        {
            //在微信小游戏上使用有问题
            //return Addressables.LoadAssetAsync<TObject>(key).WaitForCompletion();
            var handle = Addressables.LoadAssetAsync<TObject>(key);
            await handle.Task;
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.Log("加载异常:" + key);
                return default;
            }
        }
    }
}
