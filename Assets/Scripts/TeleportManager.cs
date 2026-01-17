using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportManager : MonoBehaviour {
    public string teleportTag;
    public InputAction enterRace;
    private Collider2D teleportCollider;
    private HoverableInteract hoverableInteract;
    void Start() {
        enterRace.performed += ctx => OnRaceClick();
        teleportCollider = GetComponent<Collider2D>();
        hoverableInteract = GetComponent<HoverableInteract>();
    }
    void OnEnable() {
        enterRace.Enable();
    } void OnDisable() {
        enterRace.Disable();
    }
    void OnRaceClick() {
        Debug.Log("Teleport triggered");
        // PlayerController playerController = FindFirstObjectByType<PlayerController>();
        // Collider2D playerCollider = playerController.GetComponent<Collider2D>();
        // if (playerController == null) {
        //     Debug.LogWarning("PlayerController not found in the scene.");
        //     return;
        // }
        if (GameStateManager.Instance.CheckGameState() != GameStateManager.GameStates.Playing) {
            Debug.Log("Cannot teleport, game not in playing state");
            return;
        }
        if (hoverableInteract != null && hoverableInteract.isHovering) {
            Debug.Log("Player is at the teleport point. Teleporting to race start.");
            GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Racing); // actually supposed to go to the character select state. will create new function to go to the character select screen and also save the race it is going to.
            LevelManager.Instance.LoadSceneByName(teleportTag);
        }
    }
}
