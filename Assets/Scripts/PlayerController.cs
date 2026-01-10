using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    public BoundaryManager boundaries;
    private GameStateManager.GameStates currentGameState;
    public InputAction playerMovement;
    public InputAction playerAbility;
    public InputAction playerPause;
    public InputAction tempCountdown;
    public float driftFactor = 0.8f; // How much sideways "slide" to keep (0.9 = slippery, 0.1 = sharp)
    public float accelerationForce = 25f;
    public float rotationSpeed = 150f;
    public float maxSpeed = 25f;
    public float reverseForce = 7f;
    public float brakeStrength = 0.5f;
    public float speedMultiplier = 10f; // to make the numbers displayed look bigger
    public TextMeshProUGUI speedText;
    public Slider speedBar;
    public Image speedBarFill;
    private Vector3 barOrigin;
    private bool originSaved = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAbility.performed += ctx => OnAbility(); // when the ability button (e) is pressed, call the function
        playerPause.performed += ctx => OnPause();
        tempCountdown.performed += ctx => OnTempCountdown();
    }

    void OnEnable() {
        playerMovement.Enable();
        playerAbility.Enable();
        playerPause.Enable();
        tempCountdown.Enable();
    } void OnDisable() {
        playerMovement.Disable();
        playerAbility.Disable();
        playerPause.Disable();
        tempCountdown.Disable();
    }
    void OnAbility() {
        Debug.Log("Ability triggered");
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState != GameStateManager.GameStates.Playing && currentGameState != GameStateManager.GameStates.Racing) {
            Debug.Log("Cannot use ability, game not in playing/racing state");
            return;
        }
    }
    void OnPause() {
        Debug.Log("Pause triggered");
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState == GameStateManager.GameStates.Paused) {
            LevelManager.Instance.ResumeGame();
        } else {
            LevelManager.Instance.PauseGame();
        }
    }
    void OnTempCountdown() {
        Debug.Log("Temp Countdown triggered");
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Countdown);
        TimerManager timerManager = FindFirstObjectByType<TimerManager>();
        if (timerManager != null) {
            timerManager.StartCountdown();
        } else {
            Debug.LogWarning("TimerManager not found in the scene.");
        }
    }
    void Update() { // the movement
        moveInput = playerMovement.ReadValue<Vector2>(); // reading the 2D input value
        if (speedText != null) {
                // .magnitude gives us the raw speed value
                float currentSpeed = playerRb.linearVelocity.magnitude * speedMultiplier;
                
                // "F0" removes decimals (e.g., 10 instead of 10.245)
                speedText.text = "Speed: " + currentSpeed.ToString("F0") + " KNOTS";
            }
    }
    void FixedUpdate() {
        ApplyRotation();
        ApplyForwardForce();
        KillOrthogonalVelocity();
        UpdateSpeedUI();
    }

    void ApplyRotation() {
        float rotationAmount = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        playerRb.MoveRotation(playerRb.rotation - rotationAmount);
    }

    void ApplyForwardForce() {
        // moving forward
        if (moveInput.y > 0) {
            playerRb.AddRelativeForce(Vector2.up * accelerationForce);
        }
        // braking or reversing
        else if (moveInput.y < 0) {
            float forwardSpeed = Vector2.Dot(playerRb.linearVelocity, transform.up);

            if (forwardSpeed > 0.1f) {
                playerRb.AddRelativeForce(Vector2.down * accelerationForce * brakeStrength); // braking
            } else {
                playerRb.AddRelativeForce(Vector2.down * reverseForce); // reversing
            }
        }
        playerRb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, maxSpeed); // clamp speed
    }

    void KillOrthogonalVelocity() {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(playerRb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(playerRb.linearVelocity, transform.right);

        playerRb.linearVelocity = forwardVelocity + (rightVelocity * driftFactor);
    }
    void UpdateSpeedUI() {
        if (speedText == null || speedBar == null) return;

        float currentSpeed = playerRb.linearVelocity.magnitude;
        float speedPercent = currentSpeed / maxSpeed; // 0 to 1

        speedBar.value = speedPercent;
        Color speedColor;
        if (speedPercent < 0.5f) { // transition from green to red depending on speed
            speedColor = Color.Lerp(Color.green, Color.yellow, speedPercent * 2f);
        } else {
            speedColor = Color.Lerp(Color.yellow, Color.red, (speedPercent - 0.5f) * 2f);
        }

        if (speedPercent > 0.9f) { // shake effect when going very fast
            if (!originSaved) {
                barOrigin = speedBar.transform.localPosition;
                originSaved = true;
            }

            Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * 2.0f; // calculating offset from starting position
            speedBar.transform.localPosition = barOrigin + shakeOffset;
        } 
        else if (originSaved) { // when we slow down return to original position
            speedBar.transform.localPosition = barOrigin;
            originSaved = false;
        }

        speedText.text = "Speed: " + (currentSpeed * speedMultiplier).ToString("F0"); // applying colour now
        speedText.color = speedColor;
        
        if (speedBarFill != null) {
            speedBarFill.color = speedColor;
        }
    }
}
