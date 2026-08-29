using Unity.VisualScripting;
using UnityEngine;

public class TeleporterGuidance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(DataCarrier.Instance.GetProgress() < 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
