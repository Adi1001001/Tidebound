using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
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
        }
        if (nearbyCannon != null)
        {
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
}
