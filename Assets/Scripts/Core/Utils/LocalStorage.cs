using UnityEngine;
using UnityEngine.Pool;

namespace HS
{
    /// <summary>
    /// ±¾µØ´æ´¢
    /// </summary>
    public class LocalStorage
    {
        public static void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data, true);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T Read<T>(string key,T defaultValue)
        {
            var str = JsonUtility.ToJson(defaultValue, true);
            var json = PlayerPrefs.GetString(key, str);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }

        public static T Read<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
    }
}
