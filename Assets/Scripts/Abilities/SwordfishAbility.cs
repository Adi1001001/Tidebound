using UnityEngine;
using System.Collections;

public class SwordfishAbility : Ability
{
    [SerializeField] private float slowFactor = 0.5f;
    private TimerManager gameTimer = null;
    private SlowZone[] allZones;
    private Cannon[] allCannons;
    protected override void Start()
    {
        base.Start();
        duration = 4f;
        cooldown = 22f;

        GameObject timerManager = GameObject.Find("TimerManager");
        if (timerManager != null)
        {
            gameTimer = timerManager.GetComponent<TimerManager>();
        }
        allZones = FindObjectsByType<SlowZone>();
        allCannons = FindObjectsByType<Cannon>();
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Swordfish ability activated");
        if (gameTimer != null)
        {
            gameTimer.slowFactor = slowFactor;
        }
        foreach (SlowZone zone in allZones)
        {
            zone.movable = false;
        }
        yield return RunTimer(duration);
    }

    protected override void OnAbilityEnd()
    {
        if (gameTimer != null)
        {
            gameTimer.slowFactor = 1f;
        }
        foreach (SlowZone zone in allZones)
        {
            zone.movable = true;
        }
    }
}