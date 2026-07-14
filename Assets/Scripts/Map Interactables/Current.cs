using UnityEngine;

public class Current : MonoBehaviour {
    public float pushForce = 10f;
    private Rigidbody2D playerRb;
    private CameraController cameraController;
    private PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            cameraController = FindAnyObjectByType<CameraController>();
            playerController = collision.gameObject.GetComponent<PlayerController>();
            
            if (playerController != null) playerController.inCurrent = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (playerRb != null) {
                playerRb.AddForce(transform.up * pushForce); // you have to adjust the direction of the current in unity
                cameraController.CameraShake();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (playerController != null) playerController.inCurrent = false;
            // clear references so we don't accidentally try to push a player who left the current
            playerRb = null;
            cameraController = null;
            playerController = null;
        }
    }
}
