using UnityEngine;

public class Boundaries : MonoBehaviour {
    public float currentCentre = 0f;
    public float targetCentre = 0f;
    public float currentWidth = 5f;
    public float shiftAmount = 1.5f; // How far it can move left/right per update (rn its one second)
    public float smoothSpeed = 2f; // How quickly the boundary smoothly moves to the target position
    
    void Start() {
        InvokeRepeating("CalculateNewPath", 1f, 1f); // Runs every 1 second
    }

    void CalculateNewPath() {
        targetCentre = Random.Range(-shiftAmount, shiftAmount);
        targetCentre = Mathf.Clamp(targetCentre, -2.5f, 2.5f); // Keeping the center within a certain range so it doesn't leave the screen
        Debug.Log("River center updated to: " + currentCentre + ", width: " + currentWidth);  
    }

    void Update() {
        currentCentre = Mathf.Lerp(currentCentre, targetCentre, Time.deltaTime * smoothSpeed);
    }
}
// this script works in real time, and updates the boundary when the player is already there.
// this means the player will not be able to see the boundaries ahead of time.
// so we need to pre-calculate the boundaries and show the visuals ahead of time.
// we can do this by creating a list of boundary positions and updating them as the player moves forward.
// and when the player reaches a certain point, we can remove the old boundaries and add new ones at the front. (stack-like behavior - queueing and dequeuing)
// and we need to know the player distance from the top of the screen where the boundaries are generated,
// so we can update the real time boundaries based on the player's position.