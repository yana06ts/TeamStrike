using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip menuMusic;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip buttonClickClip;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play(); 

        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        musicSource.volume = volume;
        sfxSource.volume = volume;
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickClip);
    }
}