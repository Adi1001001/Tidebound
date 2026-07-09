using UnityEngine;

public enum CharacterType
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

    [HideInInspector] public CharacterType currentCharacter = CharacterType.Anglerfish;

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

    public void SetCharacter(CharacterType character)
    {
        currentCharacter = character;
        Debug.Log("Current character: " + currentCharacter);
    }
}