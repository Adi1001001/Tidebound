using UnityEngine;

public class MaxSpeedManager : MonoBehaviour {
    private Rigidbody2D playerRb;
    private float currentMaxSpeed;
    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (GameStateManager.Instance.CheckGameState() != GameStateManager.GameStates.Racing) {
            return;
        }
        if (currentMaxSpeed < playerRb.linearVelocity.magnitude) {
            currentMaxSpeed = playerRb.linearVelocity.magnitude;
            Debug.Log("New Max Speed: " + currentMaxSpeed);
        }
    }

    public float GetCurrentMaxSpeed() { // and reset speed
        float maxSpeed = currentMaxSpeed;
        currentMaxSpeed = 0f; // reset for next race
        return maxSpeed;
    }
}
