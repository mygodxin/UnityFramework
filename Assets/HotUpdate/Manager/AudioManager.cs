using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class AudioManager
{
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = new AudioManager();
            return instance;
        }
    }

    public void Init()
    {
    }
}
