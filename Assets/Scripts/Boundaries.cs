using UnityEngine;

public class Boundaries : MonoBehaviour {
    public float currentCenter = 0f;
    public float currentWidth = 5f;
    public float shiftAmount = 1.5f; // How far it can move left/right
    
    void Start() {
        InvokeRepeating("UpdateRiverPath", 0.2f, 0.2f); // Runs every 0.2 seconds
    }

    void UpdateRiverPath() {
        currentCenter += Random.Range(-shiftAmount, shiftAmount); // Randomly shift the center to create the "winding" effect       
        currentCenter = Mathf.Clamp(currentCenter, -5f, 5f); // Keeping the center within a certain range so it doesn't leave the screen
        Debug.Log("River center updated to: " + currentCenter + ", width: " + currentWidth);
    }
}
