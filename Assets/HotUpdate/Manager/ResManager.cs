using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ResManager
{
    private static ResManager instance = null;
    public static ResManager Instance()
    {
        if(ResManager.instance == null)
        {
            ResManager.instance = new ResManager();
        }
        return ResManager.instance;
    }

   public void Init()
    {
    }
}
