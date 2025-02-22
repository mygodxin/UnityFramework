using HS;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    /// <summary>
    /// 音频管理类
    /// </summary>
    public class AudioManager
    {
        public AudioSource[] AudioSources;
        private AudioSource music;
        private AudioSource effect;
        private Dictionary<string, AudioClip> audioClipDic;

        public static AudioManager Inst = new AudioManager();

        public AudioSource Music { get { return music; } }

        public AudioManager()
        {
            var musicGO = new GameObject("AudioMusic");
            //musicGO.hideFlags = HideFlags.HideInHierarchy;
            musicGO.SetActive(true);
            Object.DontDestroyOnLoad(musicGO);
            music = musicGO.AddComponent<AudioSource>();
            music.loop = true;

            var effectGO = new GameObject("AudioEffect");
            //effectGO.hideFlags = HideFlags.HideInHierarchy;
            effectGO.SetActive(true);
            Object.DontDestroyOnLoad(effectGO);
            effect = effectGO.AddComponent<AudioSource>();
            effect.loop = false;

            if (audioClipDic == null)
                audioClipDic = new Dictionary<string, AudioClip>();
            music.volume = LocalStorage.Read<float>("MusicVolume", 1.0f);
            effect.volume = LocalStorage.Read<float>("EffectVolume", 1.0f);
        }
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param url="url"></param>
        public async void PlayMusic(string url)
        {
            if (this.music.volume <= 0) return;

            audioClipDic.TryGetValue(url, out var audio);
            if (audio == null)
            {
                audio = await ResLoader.LoadAssetAsync<AudioClip>(url);
                audioClipDic.Add(url, audio);
            }

            if (music.clip == audio) return;

            music.clip = audio;
            music.Play();
        }
        /// <summary>
        /// 停止音乐
        /// </summary>
        public void StopMusic()
        {
            if (music.isPlaying)
                music.Stop();
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name"></param>
        public async void PlayEffect(string url)
        {
            if (this.effect.volume <= 0) return;

            audioClipDic.TryGetValue(url, out var audio);
            if (audio == null)
            {
                audio = await ResLoader.LoadAssetAsync<AudioClip>(url);
                audioClipDic.TryAdd(url, audio);
            }

            effect.PlayOneShot(audio);
        }
        public void PlayEffect(AudioClip clip)
        {
            if (this.effect.volume <= 0) return;

            effect.PlayOneShot(clip);
        }

        public float MusicVolume
        {
            get { return music.volume; }
            set { music.volume = value; }
        }

        public float EffectVolume
        {
            get { return effect.volume; }
            set { effect.volume = value; }
        }

        /// <summary>
        /// 保存音乐音量，IO操作，不要频繁调用
        /// </summary>
        /// <param name="value"></param>
        public void SaveMusicVolume()
        {
            LocalStorage.Save<float>("MusicVolume", music.volume);
        }
        /// <summary>
        /// 保存音效音量，IO操作，不要频繁调用
        /// </summary>
        /// <param name="value"></param>
        public void SaveEffectVolume()
        {
            LocalStorage.Save<float>("EffectVolume", effect.volume);
        }
    }
}