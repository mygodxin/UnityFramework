using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ConfigManager
{
    private static ConfigManager instance = null;
    public static ConfigManager Instance()
    {
        if(instance == null)
            instance = new ConfigManager();
        return instance;
    }

   public void Init()
    {
    }
}
