using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // The player transform
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Offset from the target
    private Camera cam;

    void Start() {
        if (target == null) {
            target = FindFirstObjectByType<PlayerController>().transform; // finding the player
        }
        cam = GetComponent<Camera>();
    }

    void LateUpdate() // this function happens after all the other update calls
    {
        if (target == null) return; // fallback if player is still not assigned

        Vector3 desiredPosition = target.position + offset;
        transform.position = desiredPosition;
    }
}
