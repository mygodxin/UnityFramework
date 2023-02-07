using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ResManager
{
    private static ResManager _inst = null;
    public static ResManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new ResManager();
            return _inst;
        }
    }

    public void Init()
    {
    }
}
