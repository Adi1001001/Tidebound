using UnityEngine;

public class Current : MonoBehaviour {
    
    public float pushForce = 10f;
    private Rigidbody2D playerRb;
    private CameraController cameraController;
    private PlayerController playerController;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            cameraController = FindAnyObjectByType<CameraController>();
            
            playerController.inCurrent = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (playerRb != null && GameStateManager.Instance.GetPlayerState() != GameStateManager.PlayerStates.Lilypad) {
                playerRb.AddForce(transform.up * pushForce); // you have to adjust the direction of the current in unity
                cameraController.CameraShake();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerController.inCurrent = false;
            playerRb = null;
            cameraController = null;
            playerController = null;
        }
    }
}
