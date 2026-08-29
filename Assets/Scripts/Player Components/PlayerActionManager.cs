using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    private Ability ability;
    public InputAction playerAbility;
    private Cannon nearbyCannon;
    private Teleporter nearbyTeleporter;
    [SerializeField] private IconUpdater iconUpdater;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ability = GetComponent<Ability>();
        playerAbility.performed += ctx => OnAbility();    
        StartCoroutine(FindAbility());
    }

    // Update is called once per frame
    void Update()
    {
        if (ability == null) {return;}
        UpdateIcon();
        iconUpdater.onAbility = ability.onAbility ? true : false;
        if (nearbyTeleporter == null && nearbyCannon == null)
        {
            UpdateAbilityDuration();
        }
    }

    private IEnumerator FindAbility()
    {
        while (ability == null)
        {
            ability = GetComponent<Ability>();

            if (ability == null)
            {
                yield return null;
            }
        }
    }

    void OnEnable() {
        playerAbility.Enable();
    }

    void OnDisable() {
        playerAbility.Disable();
    }

    public void OnAbility()
    {
        if (nearbyTeleporter != null)
        {
            nearbyTeleporter.OnTeleportClick();
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

        if (GameStateManager.Instance.IsGameplayFrozen()) {return;}

        if (ability != null)
        {
            ability.UseAbility();
        }
    }

    void UpdateIcon()
    {
        if (nearbyTeleporter)
        {
            iconUpdater.SetIcon(IconType.Teleport);
            iconUpdater.timer = 0f;
            return;
        }
        if (nearbyCannon != null)
        {
            iconUpdater.timer = 0f;
            if (!nearbyCannon.playerInCannon)
            {
                iconUpdater.SetIcon(IconType.CannonIn);
            }
            else
            {
                iconUpdater.SetIcon(IconType.CannonOut);
            }
            return;
        }
        iconUpdater.timer = ability.timer;
        iconUpdater.SetAbilityIcon();
    }

    void UpdateAbilityDuration()
    {
        if (ability.onAbility)
        {
            iconUpdater.timer = ability.timer;
            iconUpdater.timerMax = ability.duration;
        }
        else if (ability.onCooldown)
        {
            iconUpdater.timer = ability.timer;
            iconUpdater.timerMax = ability.cooldown;
        }
        else
        {
            iconUpdater.timer = 0;
            iconUpdater.timerMax = 0;
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
