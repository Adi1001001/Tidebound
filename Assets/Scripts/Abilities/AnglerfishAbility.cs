using UnityEngine;
using System.Collections;

public class AnglerfishAbility : Ability
{
    [SerializeField] private float visionBuff = 1.5f;

    protected override void Start()
    {
        base.Start();
        duration = 5f;
        cooldown = 10f;
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Anglerfish ability activated");

        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        camera.ZoomCamera(visionBuff);
        timer = duration;
        yield return RunTimer(duration);
    }

    protected override void OnAbilityEnd()
    {
        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        camera.ZoomCamera(1f / visionBuff);
    }
}