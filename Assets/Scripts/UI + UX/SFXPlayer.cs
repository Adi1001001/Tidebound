using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource[] audioSources;
    void Start()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        foreach (AudioSource source in audioSources)
        {
            source.volume = DataCarrier.Instance.GetVolume(Volume.Master) * DataCarrier.Instance.GetVolume(Volume.SFX);
        }
    }
}
