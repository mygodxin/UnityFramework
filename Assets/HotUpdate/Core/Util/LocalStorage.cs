using UnityEngine;

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

    public static T Read<T>(string key)
    {
        var json =  PlayerPrefs.GetString(key, null);
        var data = JsonUtility.FromJson<T>(json);
        return data;
    }
}
