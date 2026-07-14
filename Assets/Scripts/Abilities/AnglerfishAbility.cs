using UnityEngine;
using System.Collections;

public class AnglerfishAbility : Ability
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float visionBuff = 1.5f;

    protected override void Start()
    {
        base.Start();
        cooldown = 15f;
    }

    protected override IEnumerator AbilityRoutine()
    {
        Debug.Log("Anglerfish ability activated");

        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        camera.ZoomCamera(visionBuff);
        yield return new WaitForSeconds(duration);
    }

    protected override void OnAbilityEnd()
    {
        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        camera.ZoomCamera(1f / visionBuff);
    }
}