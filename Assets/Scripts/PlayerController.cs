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

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAbility.performed += ctx => OnAbility(); // when the ability button (e) is pressed, call the function
    }

    void OnEnable() {
        playerMovement.Enable();
        playerAbility.Enable();
    } void OnDisable() {
        playerMovement.Disable();
        playerAbility.Disable();
    }

    void Update() {
        moveInput = playerMovement.ReadValue<Vector2>(); // reading the 2D input value
    }
    void FixedUpdate() {
        Vector2 targetVelocity = new Vector2(moveInput.x * maxSpeed, moveInput.y * maxSpeed);
        playerRb.linearVelocity = Vector2.MoveTowards(playerRb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        float leftEdge = boundaries.leftEdge; // Peek gets the first item without removing it
        float rightEdge = boundaries.rightEdge;
        float clampedX = Mathf.Clamp(transform.position.x, leftEdge, rightEdge); // Clamping player position within river boundaries
        
        if (transform.position.x != clampedX) { // Updating position if the player actually hit the boundary
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y); // Zeroing out the X velocity so the player doesn't push against the wall
        }
    }
    void OnAbility() {
        Debug.Log("Ability triggered");
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState != GameStateManager.GameStates.Playing) {
            Debug.Log("Cannot use ability, game not in playing state");
            return; // do not use ability if not in playing state
        }
    }
}
