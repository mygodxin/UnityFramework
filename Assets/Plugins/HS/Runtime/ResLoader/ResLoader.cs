using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;
namespace HS
{
    /// <summary>
    /// 资源加载类
    /// </summary>
    public class ResLoader
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        public static async Task<TObject> LoadAssetAsync<TObject>(string key) where TObject : UnityEngine.Object
        {
            //在微信小游戏上使用有问题 改为await
            //return Addressables.LoadAssetAsync<TObject>(key).WaitForCompletion();
            var handle = YooAssets.LoadAssetAsync<TObject>(key);
            await handle.Task;
            if (handle.Status != EOperationStatus.Succeed)
            {
                return null;
                //Debug.LogWarning("LoadAssetAsync Fail:" + handle.LastError);
            }
            //handle.Release();
            return (TObject)handle.AssetObject;
        }


        /// <summary>
        /// 加载整个文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="assetInfos"></param>
        /// <returns></returns>
        public static async Task<TObject[]> LoadDir<TObject>(string dirPath, AssetInfo[] assetInfos) where TObject : UnityEngine.Object
        {
            List<Task<TObject>> allTasks = new List<Task<TObject>>();
            foreach (var item in assetInfos)
            {
                if (item.AssetPath.StartsWith(dirPath))
                {
                    allTasks.Add(LoadAssetAsync<TObject>(item.AssetPath));
                }
            }
            await Task.WhenAll(allTasks);
            var array = new TObject[allTasks.Count];
            for (int i = 0; i < allTasks.Count; i++)
            {
                array[i] = allTasks[i].Result;
            }
            return array;
        }







        /// <summary>
        /// 释放资源
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        public static void Release<TObject>(TObject obj)
        {
        }
        public static void ReleaseInstance(GameObject go)
        {
        }
    }
}
