namespace HS
{
    using UnityEngine;
    /// <summary>
    /// 本地存储
    /// </summary>
    public class LocalStorage
    {
        public static void Save<T>(string key, T data)
        {
            var json = JsonUtil.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T Read<T>(string key, T defaultValue)
        {
            var str = JsonUtil.ToJson(defaultValue);
            var json = PlayerPrefs.GetString(key, str);
            var data = JsonUtil.FromJson<T>(json);
            return data;
        }

        public static T Read<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            var data = JsonUtil.FromJson<T>(json);
            return data;
        }
    }
}
