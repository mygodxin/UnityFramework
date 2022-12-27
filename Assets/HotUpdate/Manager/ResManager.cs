using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ResManager
{
    private static ResManager instance = null;
    public static ResManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ResManager();
            return instance;
        }
    }

    public void Init()
    {
    }
}
