using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// “Ù∆µπ‹¿Ì¿‡
/// </summary>
public class AudioManager : MonoBehaviour
{
    AudioSource[] audioSources;
    AudioSource music;
    AudioSource effect;

    float musicVolume;
    float effectVolume;

    Dictionary<string, AudioClip> audioClipDic;

    public static AudioManager Inst = null;

    private void Awake()
    {
        if (Inst == null)
            Inst = new AudioManager();
        if (audioClipDic == null)
            audioClipDic = new Dictionary<string, AudioClip>();

        audioSources = this.GetComponents<AudioSource>();
        music = audioSources[0];
        effect = audioSources[1];

        musicVolume = MusicVolume;
        effectVolume = EffectVolume;

        music.volume = musicVolume;
        effect.volume = effectVolume;
    }

    public void Init(AudioSource audio)
    {

    }

    public void PlayMusic(string name)
    {
        if (musicVolume <= 0) return;

        audioClipDic.TryGetValue(name, out var audio);
        if (audio == null)
        {
            audio = Addressables.LoadAssetAsync<AudioClip>("audio").WaitForCompletion();
            audioClipDic.Add(name, audio);
        }

        if (music.clip == audio) return;

        music.clip = audio;
        music.Play();
    }

    public void StopMusic()
    {
        if (music.isPlaying)
            music.Stop();
    }

    public void PlayEffect(string name)
    {
        if (effectVolume <= 0) return;

        audioClipDic.TryGetValue(name, out var audio);
        if (audio == null)
        {
            audio = Addressables.LoadAssetAsync<AudioClip>("audio").WaitForCompletion();
            audioClipDic.Add(name, audio);
        }

        if (music.clip == audio) return;

        effect.PlayOneShot(audio);
    }

    public float MusicVolume
    {
        set
        {
            music.volume = value;
            LocalStorage.Save<float>("music_volume", value);
        }
        get
        {
            var volume = LocalStorage.Read<float>("music_volume");
            return volume;
        }
    }
    public float EffectVolume
    {
        set
        {
            music.volume = value;
            LocalStorage.Save<float>("effect_volume", value);
        }
        get
        {
            var volume = LocalStorage.Read<float>("effect_volume");
            return volume;
        }
    }

}
