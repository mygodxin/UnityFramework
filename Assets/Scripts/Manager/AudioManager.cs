using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// “Ù∆µπ‹¿Ì¿‡
/// </summary>
public class AudioManager : MonoBehaviour
{
    AudioSource[] _audioSources;
    AudioSource _music;
    AudioSource _effect;

    float _musicVolume;
    float _effectVolume;

    Dictionary<string, AudioClip> _audioClipDic;

    public static AudioManager Inst = null;

    private void Awake()
    {
        if (Inst == null)
            Inst = new AudioManager();
        if (_audioClipDic == null)
            _audioClipDic = new Dictionary<string, AudioClip>();

        _audioSources = this.GetComponents<AudioSource>();
        _music = _audioSources[0];
        _effect = _audioSources[1];

        _musicVolume = musicVolume;
        _effectVolume = effectVolume;

        _music.volume = _musicVolume;
        _effect.volume = _effectVolume;
    }

    public void Init(AudioSource audio)
    {

    }

    public void PlayMusic(string name)
    {
        if (_musicVolume <= 0) return;

        _audioClipDic.TryGetValue(name, out var audio);
        if (audio == null)
        {
            audio = Addressables.LoadAssetAsync<AudioClip>("audio").WaitForCompletion();
            _audioClipDic.Add(name, audio);
        }

        if (_music.clip == audio) return;

        _music.clip = audio;
        _music.Play();
    }

    public void StopMusic()
    {
        if (_music.isPlaying)
            _music.Stop();
    }

    public void PlayEffect(string name)
    {
        if (_effectVolume <= 0) return;

        _audioClipDic.TryGetValue(name, out var audio);
        if (audio == null)
        {
            audio = Addressables.LoadAssetAsync<AudioClip>("audio").WaitForCompletion();
            _audioClipDic.Add(name, audio);
        }

        if (_music.clip == audio) return;

        _effect.PlayOneShot(audio);
    }

    public float musicVolume
    {
        set
        {
            _music.volume = value;
            LocalStorage.Save<float>("music_volume", value);
        }
        get
        {
            var volume = LocalStorage.Read<float>("music_volume");
            return volume;
        }
    }
    public float effectVolume
    {
        set
        {
            _music.volume = value;
            LocalStorage.Save<float>("effect_volume", value);
        }
        get
        {
            var volume = LocalStorage.Read<float>("effect_volume");
            return volume;
        }
    }

}
