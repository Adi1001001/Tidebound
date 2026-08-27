using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private Transform playerTransform;
    private PlayerController playerController;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothSpeed = 5f;
    private bool rotateWithPlayer = true;
    private float rotationSmoothSpeed = 3f;
    private float shakeIntensity = 0.07f;
    private float normalFOV = 70f;
    public bool stickToPlayer = true;

    void Start()
    {
        // Find the player if one hasn't been assigned
        if (playerTransform == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();

            if (playerController != null)
            {
                playerTransform = playerController.transform;
            }
        }

        cam = GetComponent<Camera>();

        if (cam != null)
        {
            cam.fieldOfView = normalFOV;
        }
    }


    void LateUpdate()
    {
        if (playerTransform == null)
            return;

        if (stickToPlayer)
        {
            FollowPlayer();

            if (rotateWithPlayer)
            {
                FollowPlayerRotation();
            }
        }
    }


    private void FollowPlayer()
    {
        transform.position = playerTransform.position + offset;
    }


    private void FollowPlayerRotation()
    {
        Quaternion desiredRotation = playerTransform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation,rotationSmoothSpeed * Time.deltaTime);
    }


    public void CameraShake()
    {
        if (playerTransform == null)
            return;

        if (playerController == null)
            return;

        if (!playerController.inCurrent)
            return;

        Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * shakeIntensity;
        transform.position += shakeOffset;
    }


    public void ZoomCamera(float zoomFactor)
    {
        if (cam == null)
            return;

        cam.fieldOfView *= zoomFactor;
    }
}