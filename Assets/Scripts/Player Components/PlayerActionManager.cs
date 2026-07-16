using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    private Ability ability;
    public InputAction playerAbility;
    private Cannon nearbyCannon;
    private Teleporter nearbyTeleporter;
    [HideInInspector] public bool alwaysBouncy;
    [SerializeField] private IconUpdater iconUpdater;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ability = GetComponent<Ability>();
        playerAbility.performed += ctx => OnAbility(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (ability == null)
            return;

        if (ability.onAbility)
        {
            iconUpdater.onAbility = true;
            iconUpdater.timer = ability.timer;
            iconUpdater.timerMax = ability.duration;
        }
        else if (ability.onCooldown)
        {
            iconUpdater.onAbility = false;
            iconUpdater.timer = ability.timer;
            iconUpdater.timerMax = ability.cooldown;
        }
        else
        {
            iconUpdater.onAbility = false;
            iconUpdater.timer = 0;
            iconUpdater.timerMax = 0;
        }
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
            iconUpdater.SetIcon(IconType.Teleport);
            iconUpdater.timer = 0;
            return;
        }
        if (nearbyCannon != null)
        {
            if (!nearbyCannon.playerInCannon && DataCarrier.Instance.GetCharacter() == Character.Anglerfish)
            {
                ability.EndAbility();
            }
            nearbyCannon.ToggleCannon();
            return;
        }

        if (GameStateManager.Instance.IsGameplayFrozen())
        {
            Debug.Log("Cannot use ability, game not in playing/racing state");
            return;
        }

        if (ability != null)
        {
            ability.UseAbility();
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
        if (alwaysBouncy)
        {
            GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Bouncy);
        }
        else
        {
            GameStateManager.PlayerStates newState = bouncyArea != null ? GameStateManager.PlayerStates.Bouncy : GameStateManager.PlayerStates.Normal;
            GameStateManager.Instance.SetPlayerState(newState);
        }
    }
}
