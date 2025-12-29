using UnityEngine;

public class Boundaries : MonoBehaviour {
    public float currentCentre = 0f;
    public float currentWidth = 15f;

    public float targetCentre = 0f;
    public float targetWidth = 15f;

    public float minWidth = 10f; // Minimum width of the river
    public float maxWidth = 18f; // Maximum width of the river
    public float shiftAmount = 2f; // How far it can move left/right per update (rn its one second)
    public float smoothSpeed = 3f; // How quickly the boundary smoothly moves to the target position
    
    void Start() {
        InvokeRepeating("CalculateNewPath", 1f, 1f); // Runs every 1 second
    }

    void CalculateNewPath() {
        targetCentre = Random.Range(-shiftAmount, shiftAmount);
        targetWidth = Random.Range(minWidth, maxWidth);
        // targetCentre = Mathf.Clamp(targetCentre, -5f, 5f); // Keeping the centre within a certain range so it doesn't leave the screen
        Debug.Log("River center updated to: " + targetCentre + ", width: " + targetWidth);  
    }

    void Update() {
        currentCentre = Mathf.Lerp(currentCentre, targetCentre, Time.deltaTime * smoothSpeed);
        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * smoothSpeed);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        // Drawing the left edge
        Vector3 left = new Vector3(currentCentre - (currentWidth / 2), 0, 0);
        Gizmos.DrawRay(left + Vector3.up * 10, Vector3.down * 20);

        // Drawing the right edge
        Vector3 right = new Vector3(currentCentre + (currentWidth / 2), 0, 0);
        Gizmos.DrawRay(right + Vector3.up * 10, Vector3.down * 20);
    }
}
// this script works in real time, and updates the boundary when the player is already there.
// this means the player will not be able to see the boundaries ahead of time.
// so we need to pre-calculate the boundaries and show the visuals ahead of time.
// we can do this by creating a list of boundary positions and updating them as the player moves forward.
// and when the player reaches a certain point, we can remove the old boundaries and add new ones at the front. (stack-like behavior - queueing and dequeuing)
// and we need to know the player distance from the top of the screen where the boundaries are generated,
// so we can update the real time boundaries based on the player's position.