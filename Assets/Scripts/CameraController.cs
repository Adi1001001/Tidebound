using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Offset from the playerTransform
    public float leadAmount = 1.5f;
    public float smoothSpeed = 5.0f;

    void Start() {
        if (playerTransform == null) {
            playerController = FindFirstObjectByType<PlayerController>();
            playerTransform = playerController.transform; // finding the player
            playerRb = playerTransform.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate() { // this function happens after all the other update calls
        if (playerTransform == null) return; // fallback if player is still not assigned

        if (playerController.inCurrent) return; // do not update position if in current, handled by CurrentExtraLead()

        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = desiredPosition;
    }

    public void CurrentExtraLead() {
        Debug.Log("Current extra lead activated");
        Vector3 leadOffset = playerRb.linearVelocity.normalized * leadAmount;
        Vector3 playerTransformPosition = playerTransform.position + offset + leadOffset;

        transform.position = Vector3.Lerp(transform.position, playerTransformPosition, smoothSpeed * Time.deltaTime);
    }
}
