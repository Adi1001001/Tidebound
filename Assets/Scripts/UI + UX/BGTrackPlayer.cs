using UnityEngine;

public class BGTrackPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip backgroundMusic;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        UpdateVolume();
        PlayLoopingMusic();
    }

    public void UpdateVolume()
    {
        audioSource.volume = DataCarrier.Instance.GetVolume(Volume.Master) * DataCarrier.Instance.GetVolume(Volume.BG);
    }

    private void PlayLoopingMusic()
    {
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
}
