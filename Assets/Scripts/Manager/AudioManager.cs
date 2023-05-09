using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using HS;

/// <summary>
/// 音频管理类
/// </summary>
public class AudioManager
{
    public AudioSource[] _audioSources;
    AudioSource _music;
    AudioSource _effect;

    Dictionary<string, AudioClip> _audioClipDic;

    private static AudioManager _inst = null;
    public static AudioManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new AudioManager();
            return _inst;
        }
    }

    public AudioManager()
    {
        var musicGO = new GameObject();
        musicGO.hideFlags = HideFlags.HideInHierarchy;
        musicGO.SetActive(true);
        Object.DontDestroyOnLoad(musicGO);
        _music = musicGO.AddComponent<AudioSource>();

        var effectGO = new GameObject();
        effectGO.hideFlags = HideFlags.HideInHierarchy;
        effectGO.SetActive(true);
        Object.DontDestroyOnLoad(effectGO);
        _effect = effectGO.AddComponent<AudioSource>();

        if (_audioClipDic == null)
            _audioClipDic = new Dictionary<string, AudioClip>();

        _music.volume = LocalStorage.Read<float>("music_volume", 1.0f);
        _effect.volume = LocalStorage.Read<float>("effect_volume", 1.0f);
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayMusic(string name)
    {
        if (this._music.volume <= 0) return;

        _audioClipDic.TryGetValue(name, out var audio);
        if (audio == null)
        {
            audio = Addressables.LoadAssetAsync<AudioClip>("Assets/AssetsPackage/Audios/" + name + ".mp3").WaitForCompletion();
            _audioClipDic.Add(name, audio);
        }

        if (_music.clip == audio) return;

        _music.clip = audio;
        _music.Play();
    }
    /// <summary>
    /// 停止音乐
    /// </summary>
    public void StopMusic()
    {
        if (_music.isPlaying)
            _music.Stop();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlayEffect(string name)
    {
        if (this._effect.volume <= 0) return;

        _audioClipDic.TryGetValue(name, out var audio);
        if (audio == null)
        {
            audio = Addressables.LoadAssetAsync<AudioClip>("Assets/AssetsPackage/Audios/sound/" + name + ".mp3").WaitForCompletion();
            _audioClipDic.Add(name, audio);
        }

        if (_music.clip == audio) return;

        _effect.PlayOneShot(audio);
    }
    /// <summary>
    /// 保存音乐音量，IO操作，不要频繁调用
    /// </summary>
    /// <param name="value"></param>
    public void SaveMusicVolume(float value)
    {
        this._music.volume = value;
        LocalStorage.Save<float>("music_volume", value);
    }
    /// <summary>
    /// 保存音效音量，IO操作，不要频繁调用
    /// </summary>
    /// <param name="value"></param>
    public void SaveEffectVolume(float value)
    {
        this._effect.volume = value;
        LocalStorage.Save<float>("effect_volume", value);
    }
}