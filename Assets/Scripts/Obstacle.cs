using UnityEngine;

public class Obstacle : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Player hit an obstacle!");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.GetSlowed();
            }
        }
    }
}
