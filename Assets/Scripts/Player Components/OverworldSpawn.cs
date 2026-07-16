using UnityEngine;

public class OverworldSpawn : MonoBehaviour
{
    void Start()
    {
        // Do not run if TimerManager exists
        if (GameObject.Find("TimerManager") != null)
        {
            return;
        }

        SaveZone[] saveZones = FindObjectsByType<SaveZone>();

        foreach (SaveZone zone in saveZones)
        {
            if (zone.saveZoneID == DataCarrier.Instance.currentSaveZoneID)
            {
                transform.position = zone.transform.position;
                return;
            }
        }
        Debug.LogWarning("No Save Zone found with ID " + DataCarrier.Instance.currentSaveZoneID);
    }
}