using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerController playerController;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Offset from the playerTransform
    public float leadAmount = 1.5f;
    public float smoothSpeed = 5.0f;
    public float shakeIntensity = 0.07f;

    void Start() {
        if (playerTransform == null) {
            playerController = FindFirstObjectByType<PlayerController>();
            playerTransform = playerController.transform; // finding the player
        }
    }

    void LateUpdate() { // this function happens after all the other update calls
        if (playerTransform == null) return; // fallback if player is still not assigned

        if (playerController.inCurrent) return; // do not update position if in current, handled by CurrentExtraLead()

        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = desiredPosition;
    }

    public void CameraShake() {
        if (playerTransform == null) return;
        if (playerController.inCurrent == false) return;
        Debug.Log("Shaking Camera!!!");
        Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * shakeIntensity;
        transform.localPosition = playerTransform.position + offset + shakeOffset;
    }
}
