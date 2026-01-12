using UnityEngine;

public class Currents : MonoBehaviour {
    public float pushForce = 10f;
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Player has entered a current!");

            // float tiltInDegrees = transform.eulerAngles.z;
            // Debug.Log("The current tilt is: " + tiltInDegrees);

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null) {
                playerRb.AddForce(transform.up * pushForce); // you have to adjust the direction of the current in unity
            }
        }
    }
}
