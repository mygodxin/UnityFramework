using UnityEngine.AddressableAssets;
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
        public static TObject LoadAssetAsync<TObject>(object key)
        {
            return Addressables.LoadAssetAsync<TObject>(key).WaitForCompletion();
        }
    }
}
