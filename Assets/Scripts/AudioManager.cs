using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField]
    private List<AudioClip> audiosClips;
    [SerializeField]
    private AudioSource source;
    public static float Volume
    {
        get => Source.volume;
        set => Source.volume = value;
    }
    public static AudioSource Source => _instance.source;
    public override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey(SettingsPanel.volume_tag))
        {
            source.volume = PlayerPrefs.GetFloat(SettingsPanel.volume_tag);
        }
    }
    public static bool HasInstance => _instance != null;

    public static void StopMusic()
    {
        if (!_instance.source) return;
        _instance.source.Stop();
    }
    public static void PlayAsMusic(string key) => _instance.PlayAsMusicInstance(key);
    private void PlayAsMusicInstance(string key)
    {
        var clip = audiosClips.Find(x => x.name == key);
        source.Stop();
        source.clip = clip;
        source.Play();

    }
    public static void PlayAsSound(string key, float delay = 0) => _instance.PlayAsSoundInstance(key, delay);
    private void PlayAsSoundInstance(string key, float delay = 0)
    {
        IEnumerator Play()
        {
            yield return new WaitForSeconds(delay);
            var clip = audiosClips.Find(x => x.name == key);
            _instance.source.PlayOneShot(clip, 0.5f);
        }
        StartCoroutine(Play());
    }
}
