using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class SpineManager
{
    private static SpineManager instance = null;
    public static SpineManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SpineManager();
            return instance;
        }
    }

    public void Init()
    {
    }
}
