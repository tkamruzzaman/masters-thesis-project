using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")] 
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")] 
    [SerializeField] private AudioClip bgMusicClip;
    [SerializeField] private AudioClip buttonSFXClip;

    public bool IsMusicOn = true;
    public bool IsSFXOn = true;


    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonSFXClip);
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
