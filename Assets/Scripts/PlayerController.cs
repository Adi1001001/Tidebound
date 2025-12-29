using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float moveInput;
    public BoundaryManager boundaries;
    public BoundaryGeneration boundaryGenerator;
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
        float currentRiverCenter = boundaryGenerator.centreQueue.Peek(); // Peek gets the first item without removing it
        float currentRiverWidth = boundaryGenerator.widthQueue.Peek();

        float leftEdge = currentRiverCenter - (currentRiverWidth / 2);
        float rightEdge = currentRiverCenter + (currentRiverWidth / 2);

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
            Debug.Log("Cannot attack, game not in playing state");
            return; // do not attack if not in playing state
        }
    }
}
