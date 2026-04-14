using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")] [SerializeField]
    private AudioSource sfxSource;

    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")] [SerializeField]
    private AudioClip bgMusicClip;

    [SerializeField] private AudioClip buttonSfxClip;

    //Source: https://freesound.org/people/BMacZero/sounds/160678/
    [SerializeField] private AudioClip typingSfxClip;

    public bool IsMusicOn = true;
    public bool IsSFXOn = true;

    public void PlaySfx(AudioClip clip, float volume = 1)
    {
        if (!clip) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayButtonClick()
    {
        PlaySfx(buttonSfxClip, volume: 0.5f);
    }

    public void PlayTypingSound(float volume)
    {
        PlaySfx(typingSfxClip, volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}