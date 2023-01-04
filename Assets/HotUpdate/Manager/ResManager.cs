using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ResManager
{
    private static ResManager inst = null;
    public static ResManager Inst
    {
        get
        {
            if (inst == null)
                inst = new ResManager();
            return inst;
        }
    }

    public void Init()
    {
    }
}
