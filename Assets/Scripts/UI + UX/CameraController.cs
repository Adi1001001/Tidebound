using UnityEngine;

public class CameraController : MonoBehaviour {
    private Camera cam;
    private Transform playerTransform;
    private PlayerController playerController;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Offset from the playerTransform
    public float leadAmount = 1.5f;
    public float smoothSpeed = 5.0f;
    public float shakeIntensity = 0.07f;
    // FOV Zoom variables
    public float normalFOV = 70f;
    public float abilityFOV = 90f;
    public bool stickToPlayer = true;

    void Start() {
        if (playerTransform == null) {
            playerController = FindAnyObjectByType<PlayerController>();
            playerTransform = playerController.transform; // finding the player
        }
        cam = GetComponent<Camera>();
        cam.fieldOfView = normalFOV;
    }

    void LateUpdate() { // this function happens after all the other update calls
        if (playerTransform == null) return; // fallback if player is still not assigned

        if (stickToPlayer)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            transform.position = desiredPosition;
        }
    }

    public void CameraShake() {
        if (playerTransform == null) return;
        if (playerController.inCurrent == false) return;
        Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * shakeIntensity;
        transform.localPosition = playerTransform.position + offset + shakeOffset;
    }

    public void ZoomCamera(float zoomFactor)
    {
        cam.orthographicSize *= zoomFactor;
    }

}
