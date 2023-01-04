using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class SpineManager
{
    private static SpineManager inst = null;
    public static SpineManager Inst
    {
        get
        {
            if (inst == null)
                inst = new SpineManager();
            return inst;
        }
    }

    public void Init()
    {
    }
}
