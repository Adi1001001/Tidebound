using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    private AbilityManager abilityManager;
    public InputAction playerAbility;
    private Cannon nearbyCannon;
    private Teleporter nearbyTeleporter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityManager = GetComponent<AbilityManager>();
        playerAbility.performed += ctx => OnAbility(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        playerAbility.Enable();
    }

    void OnDisable() {
        playerAbility.Disable();
    }

    void OnAbility()
    {
        if (nearbyTeleporter != null)
        {
            nearbyTeleporter.OnRaceClick();
            return;
        }
        if (nearbyCannon != null)
        {
            if (!nearbyCannon.playerInCannon)
            {
                abilityManager.CancelAbility();
            }
            nearbyCannon.ToggleCannon();
            return;
        }

        if (GameStateManager.Instance.IsGameplayFrozen())
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

    public void SetNearbyCannon(Cannon cannon)
    {
        nearbyCannon = cannon;
    }

    public void SetNearbyTeleporter(Teleporter teleporter)
    {
        nearbyTeleporter = teleporter;
    }

    public void SetNearbyBouncyArea(BouncyArea bouncyArea)
    {
        GameStateManager.PlayerStates newState = bouncyArea != null ? GameStateManager.PlayerStates.Bouncy : GameStateManager.PlayerStates.Normal;
        GameStateManager.Instance.SetPlayerState(newState);
    }
}
