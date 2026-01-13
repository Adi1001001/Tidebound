using UnityEngine;

public class Currents : MonoBehaviour {
    public float pushForce = 10f;
    private Rigidbody2D playerRb;
    private CameraController cameraController;
    private PlayerController playerController;
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Player has entered a current!");
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            cameraController = FindFirstObjectByType<CameraController>();
            playerController = FindFirstObjectByType<PlayerController>();
            playerController.inCurrent = true;

            if (playerRb != null) {
                playerRb.AddForce(transform.up * pushForce); // you have to adjust the direction of the current in unity
                cameraController.CurrentExtraLead();
            }
        }
    }
}
