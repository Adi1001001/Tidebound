using UnityEngine;

public class FinishLine : MonoBehaviour {
    private RaceManager raceManager;
    BoxCollider2D boxCollider;

    void Start() {
        raceManager = FindAnyObjectByType<RaceManager>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        Bounds bounds = sr.sprite.bounds; // Local-space bounds of the sprite

        // Radius that encloses the entire sprite
        boxCollider.size = bounds.size;

        // Center the collider on the sprite
        boxCollider.offset = bounds.center;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            raceManager.FinishRace();
        }
        boxCollider.enabled = false;
    }
}
