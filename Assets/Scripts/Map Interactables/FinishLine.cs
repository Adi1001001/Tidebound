using UnityEngine;

public class FinishLine : MonoBehaviour {
    private RaceManager raceManager;
    CircleCollider2D circleCollider;

    void Start() {
        raceManager = FindAnyObjectByType<RaceManager>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        Bounds bounds = sr.sprite.bounds; // Local-space bounds of the sprite

        // Radius that encloses the entire sprite
        circleCollider.radius = Mathf.Max(bounds.extents.x, bounds.extents.y);

        // Center the collider on the sprite
        circleCollider.offset = bounds.center;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            raceManager.FinishRace();
        }
        circleCollider.enabled = false;
    }
}
