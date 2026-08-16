using UnityEngine;

public enum Character
{
    Anglerfish,
    Dolphin,
    Swordfish,
    Turtle
}

public class DataCarrier : MonoBehaviour
{
    public static DataCarrier Instance;

    [HideInInspector] public string nextRaceTag;
    [HideInInspector] public Character currentCharacter = Character.Anglerfish;

    private int currentSaveZoneID = 0;
    private int overworldProgress = 10;
    private int biomeNum = 1;
    private int discoveryID = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTag(string tag)
    {
        nextRaceTag = tag;
        Debug.Log("Updated nextRaceTag to: " + nextRaceTag);
    }

    public void SetCharacter(Character character)
    {
        currentCharacter = character;
        Debug.Log("Current character: " + currentCharacter);
    }

    public Character GetCharacter()
    {
        return currentCharacter;
    }

    public void SetSaveZone(int id)
    {
        currentSaveZoneID = id;
    }

    public int GetSaveZone()
    {
        return currentSaveZoneID;
    }

    public void UnlockProgress(int id)
    {
        if (id > overworldProgress)
        {
            overworldProgress = id;
        }
    }

    public int GetProgress() {
        return overworldProgress;
    }

    public void SetBiomeNum(int id)
    {
        biomeNum = id;
    }

    public int GetBiomeNum()
    {
        return biomeNum;
    }

    public void SetDiscoveryID(int id)
    {
        discoveryID = id;
    }

    public int GetDiscoveryID()
    {
        return discoveryID;
    }
}