using UnityEngine;

public class CameraController : MonoBehaviour {
    private Camera cam;
    private Transform playerTransform;
    private PlayerController playerController;
    private AbilityManager abilityManager;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Offset from the playerTransform
    public float leadAmount = 1.5f;
    public float smoothSpeed = 5.0f;
    public float shakeIntensity = 0.07f;
    // FOV Zoom variables
    public float normalFOV = 70f;
    public float abilityFOV = 90f;

    void Start() {
        if (playerTransform == null) {
            playerController = FindFirstObjectByType<PlayerController>();
            playerTransform = playerController.transform; // finding the player
            abilityManager = playerController.GetComponent<AbilityManager>();
        }
        cam = GetComponent<Camera>();
        cam.fieldOfView = normalFOV;
    }

    void LateUpdate() { // this function happens after all the other update calls
        FOVZoom(); // continuously check for FOV zoom
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

    public void FOVZoom(){
        if (cam == null || abilityManager == null) return;
        float target = abilityManager.clownfishAbilityVisionBoost ? abilityFOV : normalFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target, Time.unscaledDeltaTime * smoothSpeed);
    }
}
