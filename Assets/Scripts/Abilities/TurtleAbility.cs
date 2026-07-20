using UnityEngine;
using System.Collections;

public class TurtleAbility : Ability
{
    private GameObject spongePrefab;
    private GameObject currObject;
    protected override void Start()
    {
        base.Start();
        duration = 1f;
        cooldown = 18f;
    }

    public void Initialise(GameObject spongePrefab)
    {
        this.spongePrefab = spongePrefab;
    }

    public override void UseAbility()
    {
        if (onAbility || onCooldown)
        {
            return;
        }
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.Sponge)
        {
            return;
        }
        activeCoroutine = StartCoroutine(AbilityWrapper());
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Turtle ability activated");
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        // If the player is not moving, give them a speed of speed so the ability can be effective
        if (playerRb.linearVelocity.magnitude < 0.1f)
        {
            float newSpeed = playerController.highSpeed * 0.01f;
            playerRb.linearVelocity = transform.up * newSpeed;
        }
        currObject = Instantiate(spongePrefab, transform.position, Quaternion.identity);
        currObject.transform.localScale = new Vector3(0.25f, 0.25f, 0);
        Sponge sponge = currObject.GetComponent<Sponge>();
        sponge.pushForce = 40f;
        sponge.airTime = 1f;
        sponge.initialised = true;
        yield return RunTimer(duration);
    }

    protected override void OnAbilityEnd()
    {
        Destroy(currObject);
    }
}