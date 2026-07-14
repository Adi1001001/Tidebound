using UnityEngine;
using System.Collections;

public class SwordfishAbility : Ability
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private float slowFactor = 0.5f;
    private GameObject timerManager;
    private TimerManager timer;
    private SlowZone[] allZones;
    private Cannon[] allCannons;
    protected override void Start()
    {
        base.Start();
        cooldown = 15f;

        timerManager = GameObject.Find("TimerManager");
        if (timerManager != null)
        {
            timer = timerManager.GetComponent<TimerManager>();
        }
        allZones = FindObjectsByType<SlowZone>();
        allCannons = FindObjectsByType<Cannon>();
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Swordfish ability activated");
        
        if (timerManager != null)
        {
            timer.slowFactor = slowFactor;
        }
        foreach (SlowZone zone in allZones)
        {
            zone.moveSpeed *= slowFactor;
        }
        foreach (Cannon cannon in allCannons)
        {
            cannon.rotationDuration *= 1/slowFactor;
        }
        yield return new WaitForSecondsRealtime(duration);
    }

    protected override void OnAbilityEnd()
    {
        if (timerManager != null)
        {
            timer.slowFactor = 1f;
        }
        foreach (SlowZone zone in allZones)
        {
            zone.moveSpeed *= 1/slowFactor;
        }
        foreach (Cannon cannon in allCannons)
        {
            cannon.rotationDuration *= slowFactor;
        }
    }
}