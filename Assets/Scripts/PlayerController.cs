using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private AbilityManager abilityManager;
    private Vector2 moveInput;
    public BoundaryManager boundaries;
    private GameStateManager.GameStates currentGameState;
    public InputAction playerMovement;
    public InputAction playerAbility;
    public InputAction playerPause;
    public InputAction tempCountdown;
    public InputAction retryLevel;
    // public InputAction enterRace;
    public float driftFactor = 0.8f; // How much sideways "slide" to keep (0.9 = slippery, 0.1 = sharp)
    public float accelerationForce = 25f;
    private float accelFactor = 1f;
    public float rotationSpeed = 150f;
    public float maxSpeed = 25f;
    private float speedFactor = 1f;
    public float reverseForce = 7f;
    public float brakeStrength = 0.5f;
    public float speedMultiplier = 10f; // to make the numbers displayed look bigger
    public TextMeshProUGUI speedText;
    public Slider speedBar;
    public Image speedBarFill;
    private Vector3 barOrigin;
    private bool isSlowed = false;
    private float decelerationRate = 10f; // applicable in slow zones (in knots per second^2)
    private bool originSaved = false;
    // public CameraController cameraController;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool inCurrent = false;
    [HideInInspector] public bool maxSpeedReached = false;
    void Start() {
        // cameraController = FindFirstObjectByType<CameraController>();
        playerRb = GetComponent<Rigidbody2D>();
        abilityManager = GetComponent<AbilityManager>();
        playerAbility.performed += ctx => OnAbility(); // when the ability button (e) is pressed, call the function
        playerPause.performed += ctx => OnPause();
        tempCountdown.performed += ctx => OnTempCountdown();
        retryLevel.performed += ctx => LevelManager.Instance.RestartRace();
        // enterRace.performed += ctx => OnRaceClick();
    }

    void OnEnable() {
        playerMovement.Enable();
        playerAbility.Enable();
        playerPause.Enable();
        tempCountdown.Enable();
        retryLevel.Enable();
        // enterRace.Enable();
    } void OnDisable() {
        playerMovement.Disable();
        playerAbility.Disable();
        playerPause.Disable();
        tempCountdown.Disable();
        retryLevel.Disable();
        // enterRace.Disable();
    }
    void OnAbility() {
        Debug.Log("Ability triggered");
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState != GameStateManager.GameStates.Playing && currentGameState != GameStateManager.GameStates.Racing) {
            Debug.Log("Cannot use ability, game not in playing/racing state");
            return;
        }
        if (abilityManager != null) {
            abilityManager.UseAbility();
        } else {
            Debug.LogWarning("AbilityManager not found in the scene.");
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
        TimerManager timerManager = FindAnyObjectByType<TimerManager>();
        if (timerManager != null) {
            timerManager.StartCountdown();
        } else {
            Debug.LogWarning("TimerManager not found in the scene.");
        }
    }
    // void OnRaceClick() {
    //     if ()
    //     Debug.Log("Enter Race triggered");
    // }
    void Update() { // the movement
        if (canMove) {
            moveInput = playerMovement.ReadValue<Vector2>(); // reading the 2D input value
        } else {
            Debug.Log("Player cannot move");
            moveInput = Vector2.zero;
        }
        if (speedText != null) {
                float currentSpeed = playerRb.linearVelocity.magnitude * speedMultiplier; // .magnitude gives us the raw speed value
                speedText.text = "Speed: " + currentSpeed.ToString("F0") + " KNOTS"; // "F0" removes decimals
            }
    }
    void FixedUpdate() {
        ApplyRotation();
        ApplyForwardForce();
        KillOrthogonalVelocity();
        EnforceSpeedLimit(); 
        UpdateSpeedUI();
    }

    void ApplyRotation() {
        float rotationAmount = moveInput.x * rotationSpeed * Time.fixedUnscaledDeltaTime;
        playerRb.MoveRotation(playerRb.rotation - rotationAmount);
    }

    void ApplyForwardForce()
    {
        float accel = accelerationForce * accelFactor;

        // Forward
        if (moveInput.y > 0)
        {
            playerRb.AddRelativeForce(Vector2.up * accel);
        }
        // Brake / reverse
        else if (moveInput.y < 0)
        {
            float forwardSpeed = Vector2.Dot(playerRb.linearVelocity, transform.up);

            if (forwardSpeed > 0.1f)
            {
                playerRb.AddRelativeForce(Vector2.down * accel * brakeStrength);
            }
            else
            {
                playerRb.AddRelativeForce(Vector2.down * reverseForce);
            }
        }

        if (inCurrent)
            return;
    }

    void EnforceSpeedLimit()
    {
        float limit = isSlowed ? maxSpeed * speedFactor : maxSpeed;

        Vector2 vel = playerRb.linearVelocity;
        float speed = vel.magnitude;

        if (speed > limit)
        {
            float excess = speed - limit;

            float strength =
                decelerationRate + (excess * excess * 0.5f);

            float newSpeed = Mathf.MoveTowards(
                speed,
                limit,
                strength * Time.fixedDeltaTime
            );

            playerRb.linearVelocity = vel.normalized * newSpeed;
        }
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

        if (speedPercent >= 0.99f) {
            maxSpeedReached = true;
        } else {
            maxSpeedReached = false;
        }
        if (speedPercent > 0.9f) { // shake effect when going very fast
            if (!originSaved) {
                barOrigin = speedBar.transform.localPosition;
                originSaved = true;
            }

            Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * 2.0f;
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
    public void EnterSlowZone(float speedFactor, float accelFactor)
    {
        if (abilityManager != null && abilityManager.turtleOn)
            return;

        isSlowed = true;
        this.speedFactor = speedFactor;
        this.accelFactor = accelFactor;
    }

    public void ExitSlowZone()
    {
        isSlowed = false;
        speedFactor = 1f;
        accelFactor = 1f;
    }
}
