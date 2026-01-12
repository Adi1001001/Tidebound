using UnityEngine;

public class Obstacle : MonoBehaviour {
    public float slowDuration = 1f;
    public float slowFactor = 0.5f;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Player hit an obstacle!");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.GetSlowed(slowDuration, slowFactor);
            }
        }
    }
}
