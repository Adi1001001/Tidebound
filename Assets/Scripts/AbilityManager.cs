using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AbilityManager : MonoBehaviour {
    PlayerController playerController;
    public float anglerfishAbilityDuration = 5f;
    public float anglerfishVisionBuff = 1.5f;
    public int anglerfishCooldown = 15;
    public float dolphinAbilityDuration = 3f;
    public float dolphinAbilityMultiplier = 1.5f;
    public int dolphinCooldown = 10;
    public float sharkAbilityDuration = 1f;
    public float sharkAbilityForce = 500f;
    public int sharkCooldown = 8;
    public float eelRadius = 10f;
    public int eelCooldown = 12;
    public float swordfishAbilityDuration = 3f;
    public float swordfishAbilitySlowFactor = 0.25f;
    public int swordfishCooldown = 10;
    public float turtleAbilityDuration = 5f;
    public int turtleCooldown = 15;
    [HideInInspector] public bool onAbility = false;
    [HideInInspector] public bool onCooldown = false;
    private IEnumerator activeAbility;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
    public void UseAbility()
    {
        if (onAbility)
        {
            Debug.Log("Ability already running!");
            return;
        }
        if (onCooldown)
        {
            Debug.Log("Ability on cooldown!");
            return;
        }
        switch (DataCarrier.Instance.currentCharacter)
        {
            case Character.Anglerfish:
                activeAbility = AnglerfishAbility();
                break;
            case Character.Dolphin:
                dolphinAbility();
                break;
            case Character.Shark:
                sharkAbility();
                break;
            case Character.Eel:
                eelAbility();
                break;
            case Character.Swordfish:
                swordfishAbility();
                break;
            case Character.Turtle:
                turtleAbility();
                break;
        }
        StartCoroutine(activeAbility);
    }

    public void CancelAbility()
    {
        if (!onAbility) {return;}
        StopCoroutine(activeAbility);
        switch (DataCarrier.Instance.currentCharacter)
        {
            case Character.Anglerfish:
                AnglerFishCancel();
                break;
        }
    }
    public IEnumerator AnglerfishAbility() { // increases your vision for 5 seconds, cooldown of 15 seconds after use
        Debug.Log("Anglerfish ability activated");
        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        onAbility = true;
        camera.ZoomCamera(anglerfishVisionBuff);
        yield return new WaitForSeconds(anglerfishAbilityDuration); 
        AnglerFishCancel();
    }

    public void AnglerFishCancel()
    {
        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        camera.ZoomCamera(1/anglerfishVisionBuff);
        onAbility = false;
        StartCoroutine(Cooldown(anglerfishCooldown));
    }
    public void dolphinAbility() { // increases your acceleration and max speed for 3 seconds, cooldown of 10 seconds after use
        Debug.Log("Dolphin ability activated");
        StartCoroutine(DolphinSpeedBoost());
        StartCoroutine(Cooldown(dolphinCooldown));
    }
    public void sharkAbility() { // small invulnerable dash that breaks obstacles, cooldown of 8 seconds after use
        Debug.Log("Shark ability activated");
        StartCoroutine(SharkAttack());
        StartCoroutine(Cooldown(sharkCooldown));
    }
    public void eelAbility() { // disables nearby obstacles for the rest of the race, cooldown of 12 seconds after use
        Debug.Log("Eel ability activated");

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, eelRadius);

        foreach (Collider2D hit in hitObjects) {
            if (hit.gameObject == gameObject) continue; // avoiding yourself
            GameObject collidedObject = hit.gameObject;
            SlowZone obstacle = collidedObject.GetComponent<SlowZone>();

            if (obstacle == null) continue; // only affect obstacles

            Renderer renderer = collidedObject.GetComponent<Renderer>();
            renderer.material.color = Color.grey; // change color to indicate that it can't effect the player anymore
            obstacle.active = false; // disable the obstacle
        }
        StartCoroutine(Cooldown(eelCooldown));
    }
    public void swordfishAbility() { // slows down time for 2 seconds, cooldown of 10 seconds after use
        Debug.Log("Swordfish ability activated");
        StartCoroutine(SwordfishTime());
        StartCoroutine(Cooldown(swordfishCooldown));
    }
    public void turtleAbility() { // make the player immune to obstacles for 5 seconds, cooldown of 15 seconds after use
        Debug.Log("Turtle ability activated");
        StartCoroutine(TurtleInvincibility());
        StartCoroutine(Cooldown(turtleCooldown));
    }

    IEnumerator DolphinSpeedBoost() {
        float originalAcceleration = playerController.accelForce;
        float originalMaxSpeed = playerController.highSpeed;
        playerController.accelForce *= dolphinAbilityMultiplier;
        playerController.highSpeed *= dolphinAbilityMultiplier; 
        yield return new WaitForSeconds(dolphinAbilityDuration); 
        playerController.accelForce = originalAcceleration;
        playerController.highSpeed = originalMaxSpeed;
    }
    IEnumerator TurtleInvincibility() {
        onAbility = true;
        yield return new WaitForSeconds(turtleAbilityDuration); 
        onAbility = false;
    }
    IEnumerator SharkAttack() {
        onAbility = true;
        Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
        playerRb.AddRelativeForce(Vector2.up * sharkAbilityForce); // adding a quick dash forward
        yield return new WaitForSeconds(sharkAbilityDuration);
        onAbility = false;
    }
    IEnumerator SwordfishTime() {
        onAbility = true;
        yield return new WaitForSecondsRealtime(swordfishAbilityDuration); // wait for real time seconds
        onAbility = false;
    }

    IEnumerator Cooldown(int cooldown)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown); 
        onCooldown = false;
    }
    public void AbilityNotReady() {
        // you can add a UI popup or sound effect here to indicate that the ability is not ready yet
    }
}