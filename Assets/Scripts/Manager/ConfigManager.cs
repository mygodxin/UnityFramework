using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ConfigManager
{
    private static ConfigManager _inst = null;
    public static ConfigManager inst
    {
        get
        {
        if(_inst == null)
            _inst = new ConfigManager();
        return _inst;
        }
    }

   public void Init()
    {
        //JsonConvert.DeserializeObject<>()
    }
}
