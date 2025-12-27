using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float moveInput;
    public Boundaries boundaries;
    private GameStateManager.GameStates currentGameState;
    public float maxSpeed = 10f;    
    public float acceleration = 10f;
    public InputAction playerMovement;
    public InputAction playerAttack;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAttack.performed += ctx => OnAttack(); // when attack button (d/j) is pressed, call the function
    }

    void OnEnable() {
        playerMovement.Enable();
        playerAttack.Enable();
    } void OnDisable() {
        playerMovement.Disable();
        playerAttack.Disable();
    }

    void Update() {
        moveInput = playerMovement.ReadValue<float>(); // reading the 1D axis value
    }
    void FixedUpdate() {
        float halfWidth = boundaries.currentWidth / 2; // boundaries
        float leftEdge = boundaries.currentCenter - halfWidth;
        float rightEdge = boundaries.currentCenter + halfWidth;

        Vector2 targetVelocity = new Vector2(moveInput * maxSpeed, playerRb.linearVelocity.y);
        playerRb.linearVelocity = Vector2.MoveTowards(playerRb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, leftEdge, rightEdge); // Clamping player position within river boundaries
        
        if (transform.position.x != clampedX) { // Updating position if the player actually hit the boundary
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y); // Zeroing out the X velocity so the player doesn't push against the wall
        }
    }
    void OnAttack() {
        Debug.Log("Attack triggered");
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState != GameStateManager.GameStates.Playing) {
            return; // do not attack if not in playing state
        }
    }
}
