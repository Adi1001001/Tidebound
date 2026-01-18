using UnityEngine;
// used for temporary data (will delete after player closes the game)
public class DataCarrier : MonoBehaviour
{
    public static DataCarrier Instance;
    [HideInInspector] public string nextRaceTag; // teleport tag

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This is the magic line!
        } else {
            Destroy(gameObject);
        }
    }
    public void UpdateTag(string tag) {
        nextRaceTag = tag;
        Debug.Log("Updated nextRaceTag to: " + nextRaceTag);
    }
}
