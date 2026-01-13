using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target; // The player transform
    private Rigidbody2D targetRb;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Offset from the target
    public float leadAmount = 1.5f;
    public float smoothSpeed = 5.0f;

    void Start() {
        if (target == null) {
            target = FindFirstObjectByType<PlayerController>().transform; // finding the player
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate() { // this function happens after all the other update calls
        if (target == null) return; // fallback if player is still not assigned

        Vector3 desiredPosition = target.position + offset;
        transform.position = desiredPosition;
    }

    public void CurrentExtraLead() {
        Debug.Log("Current extra lead activated");
        Vector3 leadOffset = targetRb.linearVelocity.normalized * leadAmount;
        Vector3 targetPosition = target.position + offset + leadOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
