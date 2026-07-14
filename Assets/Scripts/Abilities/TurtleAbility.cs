using UnityEngine;
using System.Collections;

public class TurtleAbility : Ability
{
    [SerializeField] private float duration = 5f;
    private PlayerActionManager actionManager;
    protected override void Start()
    {
        base.Start();
        cooldown = 8f;
        actionManager = GetComponent<PlayerActionManager>();
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Turtle ability activated");
        playerController.prevPlayerState = GameStateManager.PlayerStates.Bouncy;
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Bouncy);
        actionManager.alwaysBouncy = true;
        yield return new WaitForSeconds(duration);
    }

    protected override void OnAbilityEnd()
    {
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.Bouncy)
        {
            GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Normal);
        }
        playerController.prevPlayerState = GameStateManager.PlayerStates.Normal;
        actionManager.alwaysBouncy = false;
    }
}