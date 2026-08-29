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
            if (zone.saveZoneID == DataCarrier.Instance.GetSaveZone())
            {
                transform.position = zone.transform.position;
                transform.rotation = zone.transform.rotation;
                return;
            }
        }
    }
}