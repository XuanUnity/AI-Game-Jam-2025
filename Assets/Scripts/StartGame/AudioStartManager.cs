using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AudioStartManager : Singleton<AudioStartManager>
{

    [Header("Audio Sources")]
    public AudioSource bgmSource;   // Nhạc nền
    public AudioSource sfxSource;   // Hiệu ứng

    [Header("Audio Clips")]
    public AudioClip bgmClips;    // Danh sách nhạc nền
    public AudioClip sfxButtonClick;    // Danh sách hiệu ứng

    void Start()
    {
        ApplyVolume();
    }

    // 🎵 Play nhạc nền theo index
    public void PlayBGM(bool loop = true)
    {
        bgmSource.clip = bgmClips;
        bgmSource.loop = loop;
        bgmSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bgmSource.Play();
    }

    // 🔊 Play hiệu ứng theo index
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot( clip , PlayerPrefs.GetFloat("SoundVolume", 1));
    }
    public void PlayButtonClick()
    {
        PlaySFX(sfxButtonClick);
    }

    // 🔇 Tắt/mở nhạc nền
    public void ToggleBGM()
    {
        bgmSource.mute = !bgmSource.mute;
    }

    // 🔇 Tắt/mở hiệu ứng
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    // 🔊 Set volume
    public void SetBGMVolume(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
    }

    private void ApplyVolume()
    {
        bgmSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SoundVolume", 1);
    }
}
