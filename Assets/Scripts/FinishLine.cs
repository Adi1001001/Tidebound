using UnityEngine;

public class FinishLine : MonoBehaviour {
    private RaceManager raceManager;

    void Start() {
        raceManager = FindAnyObjectByType<RaceManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Finish line crossed by player.");
            raceManager.FinishRace();
        }
    }
}
