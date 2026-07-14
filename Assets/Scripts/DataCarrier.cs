using UnityEngine;

public enum Character
{
    Anglerfish,
    Dolphin,
    Shark,
    Eel,
    Swordfish,
    Turtle
}

public class DataCarrier : MonoBehaviour
{
    public static DataCarrier Instance;

    [HideInInspector] public string nextRaceTag;
    [HideInInspector] public Character currentCharacter = Character.Anglerfish;

    [HideInInspector] public int currentSaveZoneID = 0;
    [HideInInspector] public int overworldProgress = 0;

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
        Debug.Log("Current Save Zone ID: " + currentSaveZoneID);
    }

    public void UnlockProgress(int id)
    {
        if (id > overworldProgress)
        {
            overworldProgress = id;
            Debug.Log("Unlocked overworld progress: " + overworldProgress);
        }
    }
}