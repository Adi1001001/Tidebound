using UnityEngine;
using System.Collections;

public class DolphinAbility : Ability
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float abilityBuff = 1.25f;
    private TrailRenderer playerTrail;

    protected override void Start()
    {
        base.Start();
        cooldown = 10f;

        GameObject player = GameObject.FindWithTag("Player");
        playerTrail = player.GetComponentInChildren<TrailRenderer>();
        SetTealGradient();
        playerTrail.time = 1f; 
        playerTrail.enabled = false;
    }
    private void SetTealGradient()
    {
        Gradient gradient = new Gradient();
        
        // Define your color transition (Teal to Darker Teal)
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(new Color(0f, 1f, 1f), 0.0f);     // Solid Teal at start
        colorKeys[1] = new GradientColorKey(new Color(0f, 0.4f, 0.4f), 1.0f);  // Deeper Teal at end

        // Define your visibility transition (Opaque to Transparent)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);                      // Fully visible at start
        alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);                      // Fades out completely

        gradient.SetKeys(colorKeys, alphaKeys);
        playerTrail.colorGradient = gradient;
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Dolphin ability activated");
        playerController.accelForce *= abilityBuff;
        playerController.highSpeed *= abilityBuff; 

        playerTrail.enabled = true;
        playerTrail.Clear(); 

        yield return new WaitForSeconds(duration);
    }

    protected override void OnAbilityEnd()
    {
        playerController.accelForce *= 1/abilityBuff;
        playerController.highSpeed *= 1/abilityBuff; 
        playerTrail.enabled = false;
    }
}