using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ConfigManager
{
    public string connectURL = "http://192.168.1.184:88";
    public string resoucesURL = "http://192.168.1.3:88";
    public int[] encryptKeys = new int[4]{
           56,
           23123,
           -34332,
           -7878441
       };
    private static ConfigManager _inst = null;
    public static ConfigManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new ConfigManager();
            return _inst;
        }
    }

    public void Init()
    {
        //JsonConvert.DeserializeObject<>()
    }
}
