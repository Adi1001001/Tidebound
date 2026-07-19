using UnityEngine;
using System.Collections;

public class TurtleAbility : Ability
{
    protected override void Start()
    {
        base.Start();
        duration = 5f;
        cooldown = 8f;
    }

    protected override IEnumerator AbilityRoutine()
    {
        yield return RunTimer(duration);
    }

    protected override void OnAbilityEnd()
    {
    }
}