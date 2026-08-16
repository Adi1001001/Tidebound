using UnityEngine;
using System.Collections;

public class SwordfishAbility : Ability
{
    public float slowFactor = 0.25f;
    private TimerManager gameTimer = null;
    private SlowZone[] allZones;
    protected override void Start()
    {
        base.Start();
        duration = 1.5f;
        cooldown = 22f;

        GameObject timerManager = GameObject.Find("TimerManager");
        if (timerManager != null)
        {
            gameTimer = timerManager.GetComponent<TimerManager>();
        }
        allZones = FindObjectsByType<SlowZone>();
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
        Time.timeScale = 1-slowFactor*2;
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
        Time.timeScale = 1f;
    }
}