using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    public InputAction playerMovement;
    public InputAction playerPause;
    public InputAction cheatCode;
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
    public bool inCurrent = false;
    [Header("Collision Feel")]
    [SerializeField] private float minBounceSpeed = 7f;
    [SerializeField] private float maxBounceSpeed = 18f;
    private float movementLockTimer = 0f;
    private bool originSaved = false;
    private Coroutine airtimeCoroutine;    
    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        playerPause.performed += ctx => OnPause();
        cheatCode.performed += ctx => ActivateCheatCode();
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Normal);
    }

    void Update()
    {
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.InCannon)
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
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.Lilypad)
        {
            playerRb.angularVelocity = 0f;
        }
        if (speedText != null)
        {
            float currentSpeed = playerRb.linearVelocity.magnitude * speedMultiplier;
            speedText.text = "Speed: " + currentSpeed.ToString("F0") + " KNOTS";
        }
    }
    void FixedUpdate()
    {
        if (movementLockTimer > 0f)
        {
            movementLockTimer -= Time.fixedDeltaTime;
        }
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.InCannon)
        {
            UpdateSpeedUI();
            return;
        }
        if (GameStateManager.Instance.GetPlayerState() != GameStateManager.PlayerStates.Lilypad)
        {
            ApplyRotation();
            ApplyForwardForce();
            KillOrthogonalVelocity();
        }
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.Lilypad)
        {
            ApplyResistance(highSpeed, 5, 2f);
        }
        else if (isSlowed)
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
        playerPause.Enable();
        cheatCode.Enable();
    } 
    void OnDisable() {
        playerMovement.Disable();
        playerPause.Disable();
        cheatCode.Disable();
    }
    public void OnPause() {
        GameStateManager.GameStates currentGameState = GameStateManager.Instance.GetGameState();

        if (currentGameState == GameStateManager.GameStates.Paused) 
        {
            LevelManager.Instance.ResumeGame();
        } 
        else 
        {
            LevelManager.Instance.PauseGame();
        }
    }

    public void ActivateCheatCode()
    {
        if (GameStateManager.Instance.GetGameState() == GameStateManager.GameStates.Playing)
        {
            LevelManager.Instance.OpenCheats();
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
        if (movementLockTimer > 0f) {return;}
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
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;

        float speedIntoWall = Vector2.Dot(collision.relativeVelocity, normal);

        if (speedIntoWall <= 0f)
            return;

        float bounceSpeed = Mathf.Clamp(speedIntoWall, minBounceSpeed, maxBounceSpeed);
        playerRb.AddForce(normal * bounceSpeed, ForceMode2D.Impulse);
        movementLockTimer = 0.2f;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;

        float speedIntoWall = Vector2.Dot(collision.relativeVelocity, normal);

        if (speedIntoWall <= 0f)
            return;

        playerRb.AddForce(normal * minBounceSpeed, ForceMode2D.Impulse);
        movementLockTimer = 0.2f;
    }
    
    public void EnterSlowZone(float resistanceSpeed, float accelFactor)
    {
        isSlowed = true;
        this.resistanceSpeed = resistanceSpeed;
        this.accelFactor = accelFactor;
    }

    public void ExitSlowZone()
    {
        isSlowed = false;
        resistanceSpeed = highSpeed;
        accelFactor = 1f;
    }

    public void EnterCannon(Vector3 cannonPos)
    {
        CancelAirTime();
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.InCannon);
        transform.position = cannonPos;
        moveInput = Vector2.zero;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.angularVelocity = 0f;
    }

    public void FireFromCannon(float speed, Vector2 direction)
    { 
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Normal);
        playerRb.linearVelocity = direction.normalized * speed;
        playerRb.angularVelocity = 0f;
    }

    public void StartAirTime(float duration)
    {
        if (airtimeCoroutine != null)
        {
            StopCoroutine(airtimeCoroutine);
        }

        airtimeCoroutine = StartCoroutine(AirTimeRoutine(duration));
    }

    private IEnumerator AirTimeRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Normal);
        playerRb.linearDamping = 1f;
        airtimeCoroutine = null;
    }

    public void CancelAirTime()
    {
        if (airtimeCoroutine != null)
        {
            StopCoroutine(airtimeCoroutine);
            GetComponent<PlayerAppearance>().StopAirborneEffect();
            airtimeCoroutine = null;
        }
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Normal);
        playerRb.linearDamping = 1f;
    }
}
