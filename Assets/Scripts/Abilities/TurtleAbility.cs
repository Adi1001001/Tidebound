using UnityEngine;
using System.Collections;

public class TurtleAbility : Ability
{
    private PlayerActionManager actionManager;
    private TrailRenderer playerTrail;
    protected override void Start()
    {
        base.Start();
        duration = 5f;
        cooldown = 8f;
        actionManager = GetComponent<PlayerActionManager>();
        playerTrail = GetComponent<TrailRenderer>();
        SetPurpleGradient();
    }

    private void SetPurpleGradient()
    {
        Gradient gradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(new Color(0.65f, 0.15f, 0.95f), 0.0f); // Bright Neon Purple
        colorKeys[1] = new GradientColorKey(new Color(0.35f, 0.05f, 0.6f), 1.0f);  // Rich Purple at end

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);                            // Fully visible at start
        alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);                            // Fades out completely

        gradient.SetKeys(colorKeys, alphaKeys);
        playerTrail.colorGradient = gradient;

        AnimationCurve widthCurve = new AnimationCurve();
        widthCurve.AddKey(0.0f, 0.5f);                                              // Full width at head
        widthCurve.AddKey(1.0f, 0.1f);                                              // Tapered thin width at tail
        playerTrail.widthCurve = widthCurve;
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Turtle ability activated");
        playerController.prevPlayerState = GameStateManager.PlayerStates.Bouncy;
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Bouncy);
        actionManager.alwaysBouncy = true;
        
        playerTrail.enabled = true;
        playerTrail.Clear(); 
        yield return RunTimer(duration);
    }

    protected override void OnAbilityEnd()
    {
        playerTrail.enabled = false;
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.Bouncy)
        {
            GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Normal);
        }
        playerController.prevPlayerState = GameStateManager.PlayerStates.Normal;
        actionManager.alwaysBouncy = false;
    }
}