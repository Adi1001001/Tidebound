using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float moveInput;
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
        Vector2 targetVelocity = new Vector2(moveInput * maxSpeed, playerRb.linearVelocity.y);
        playerRb.linearVelocity = Vector2.MoveTowards(playerRb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }
    void OnAttack() {
        Debug.Log("Attack triggered");
    }
}
