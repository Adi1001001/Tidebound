using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using Unity.Collections.LowLevel.Unsafe;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private AbilityManager abilityManager;
    private Vector2 moveInput;
    public BoundaryManager boundaries;
    private GameStateManager.GameStates currentGameState;
    private GameStateManager.PlayerStates currentPlayerState = GameStateManager.PlayerStates.Normal;
    private Cannon nearbyCannon;
    public InputAction playerMovement;
    public InputAction playerAbility;
    public InputAction playerPause;
    public InputAction tempCountdown;
    public InputAction retryLevel;
    // public InputAction enterRace;
    public float driftFactor = 0.8f; // How much sideways "slide" to keep (0.9 = slippery, 0.1 = sharp)
    public float accelForce = 25f;
    private float accelFactor = 1f;
    public float minRotationSpeed = 150f;
    public float maxRotationSpeed = 250f;
    public float highSpeed = 25f;
    private float resistanceSpeed;
    public float reverseForce = 7f;
    public float brakeStrength = 0.5f;
    public float speedMultiplier = 10f; // to make the numbers displayed look bigger
    public TextMeshProUGUI speedText;
    public Slider speedBar;
    public Image speedBarFill;
    private Vector3 barOrigin;
    private bool isSlowed = false;
    [Header("Collision Feel")]
    [SerializeField] private float bounceStrength = 0.6f;
    [SerializeField] private float maxBounceSpeed = 25f;

    private float collisionLockTimer = 0f;
    private bool originSaved = false;
    // public CameraController cameraController;
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

    void Update()
    {
        if (currentPlayerState == GameStateManager.PlayerStates.InCannon)
        {
            moveInput = Vector2.zero;
        }
        else if (GameStateManager.Instance != null && !GameStateManager.Instance.IsGameplayFrozen())
        {
            moveInput = playerMovement.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        if (speedText != null)
        {
            float currentSpeed = playerRb.linearVelocity.magnitude * speedMultiplier;

            speedText.text =
                "Speed: " + currentSpeed.ToString("F0") + " KNOTS";
        }
    }
    void FixedUpdate()
    {
        if (currentPlayerState == GameStateManager.PlayerStates.InCannon)
        {
            UpdateSpeedUI();
            return;
        }
        if (collisionLockTimer > 0f)
        {
            collisionLockTimer -= Time.fixedDeltaTime;
        }
        ApplyRotation();
        ApplyForwardForce();
        KillOrthogonalVelocity();
        if (isSlowed)
        {
            ApplyResistance(resistanceSpeed, 35, 3.5f);
        }
        else
        {
            ApplyResistance(highSpeed, 15, 2.5f);
        }
        UpdateSpeedUI();
    }

    public void SetVisible(bool visible)
    {
        GetComponent<SpriteRenderer>().enabled = visible;
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
    void OnAbility()
    {
        if (nearbyCannon != null)
        {
            nearbyCannon.ToggleCannon();
            return;
        }

        currentGameState = GameStateManager.Instance.CheckGameState();

        if (currentGameState != GameStateManager.GameStates.Playing &&
            currentGameState != GameStateManager.GameStates.Racing)
        {
            Debug.Log("Cannot use ability, game not in playing/racing state");
            return;
        }

        if (abilityManager != null)
        {
            abilityManager.UseAbility();
        }
        else
        {
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

    void ApplyRotation()
    {
        float steeringFalloffSpeed = highSpeed * 1.25f;
        float speedPercent = Mathf.Clamp01(playerRb.linearVelocity.magnitude / steeringFalloffSpeed);
        float currentRotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, speedPercent);
        float rotationAmount = moveInput.x * currentRotationSpeed * Time.fixedUnscaledDeltaTime;

        playerRb.MoveRotation(playerRb.rotation - rotationAmount);
    }

    void ApplyForwardForce()
    {
        float accel = accelForce * accelFactor;
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

    }

    void KillOrthogonalVelocity()
    {
        if (collisionLockTimer > 0f)
            return;

        Vector2 forwardVelocity = transform.up * Vector2.Dot(playerRb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(playerRb.linearVelocity, transform.right);

        playerRb.linearVelocity = forwardVelocity + (rightVelocity * driftFactor);
    }

    void ApplyResistance(float resistanceSpeed, float resistanceStrength, float fallOff)
    {
        Vector2 velocity = playerRb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed <= resistanceSpeed)
            return;

        float excessSpeed = speed - resistanceSpeed;
        float excessRatio = excessSpeed / resistanceSpeed;

        float exponent = 1/fallOff;

        float resistanceMultiplier =
            Mathf.Pow(excessRatio, exponent);

        float resistanceForce =
            resistanceMultiplier *
            resistanceStrength *
            playerRb.mass;

        playerRb.AddForce(
            -velocity.normalized * resistanceForce,
            ForceMode2D.Force
        );
    }

    void UpdateSpeedUI() {
        if (speedText == null || speedBar == null) return;

        float currentSpeed = playerRb.linearVelocity.magnitude;
        float speedPercent = Mathf.Min(1f, currentSpeed / highSpeed); // 0 to 1

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionLockTimer > 0f)
            return;

        collisionLockTimer = 0.1f; 

        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;

        Vector2 v = collision.relativeVelocity;

        float speedIntoWall = Vector2.Dot(v, normal);

        if (speedIntoWall <= 0f)
            return;

        float t = Mathf.Clamp01(speedIntoWall / maxBounceSpeed);
        float strength = Mathf.SmoothStep(0f, 1f, t);

        Vector2 reflected = Vector2.Reflect(v, normal);

        Vector2 bounceVelocity = Vector2.Lerp(v, reflected, 0.6f * strength);

        Vector2 correction = bounceVelocity - v;

        playerRb.AddForce(
            -correction * bounceStrength,
            ForceMode2D.Impulse
        );
    }
    public void EnterSlowZone(float speedLimit, float accelFactor)
    {
        if (abilityManager != null && abilityManager.turtleOn)
            return;

        isSlowed = true;
        this.resistanceSpeed = speedLimit;
        this.accelFactor = accelFactor;
    }

    public void ExitSlowZone()
    {
        isSlowed = false;
        resistanceSpeed = highSpeed;
        accelFactor = 1f;
    }

    public void SetNearbyCannon(Cannon cannon)
    {
        nearbyCannon = cannon;
    }

    public void EnterCannon()
    {
        currentPlayerState = GameStateManager.PlayerStates.InCannon;
        moveInput = Vector2.zero;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.angularVelocity = 0f;
    }

    public void FireFromCannon(float speed, Vector2 direction)
    {
        currentPlayerState = GameStateManager.PlayerStates.Normal;
        SetVisible(true);
        playerRb.linearVelocity = direction.normalized * (speed/speedMultiplier);
        playerRb.angularVelocity = 0f;
    }
}
