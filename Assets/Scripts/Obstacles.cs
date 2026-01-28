using UnityEngine;

public class Obstacle : MonoBehaviour {
    public float slowDuration = 1f;
    public float slowFactor = 0.5f;
    public bool breakableByShark;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Player hit an obstacle!");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            AbilityManager abilityManager = collision.gameObject.GetComponent<AbilityManager>();
            if (abilityManager.sharkOn) {
                if (breakableByShark) {
                    Destroy(gameObject);
                    return;
                }
                Debug.Log("Shark ability active, obstacle not breakable");
                return;
            }
            if (playerController != null) {
                playerController.GetSlowed(slowDuration, slowFactor);
            }
        }
    }
}
