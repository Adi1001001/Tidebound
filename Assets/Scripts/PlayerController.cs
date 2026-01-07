using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    public BoundaryManager boundaries;
    private GameStateManager.GameStates currentGameState;
    public float maxSpeed = 10f;
    public float acceleration = 10f;
    public InputAction playerMovement;
    public InputAction playerAbility;
    public InputAction playerPause;
    public InputAction tempCountdown;
    public GameObject pauseMenuUI;

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
            GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
        } else {
            GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Paused);
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
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
    }
    void FixedUpdate() {
        Vector2 targetVelocity = new Vector2(moveInput.x * maxSpeed, moveInput.y * maxSpeed);
        playerRb.linearVelocity = Vector2.MoveTowards(playerRb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        float leftEdge = boundaries.leftEdge;
        float rightEdge = boundaries.rightEdge;
        float clampedX = Mathf.Clamp(transform.position.x, leftEdge, rightEdge); // Clamping player position within river boundaries (might be redundant with physics colliders later on)
        
        if (transform.position.x != clampedX) { // Updating position if the player actually hit the boundary
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y); // Zeroing out the X velocity so the player doesn't push against the wall
        }
    }
}
