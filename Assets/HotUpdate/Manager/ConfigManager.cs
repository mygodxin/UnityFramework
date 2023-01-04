using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ConfigManager
{
    private static ConfigManager inst = null;
    public static ConfigManager Inst
    {
        get
        {
        if(inst == null)
            inst = new ConfigManager();
        return inst;
        }
    }

   public void Init()
    {
        //JsonConvert.DeserializeObject<>()
    }
}
