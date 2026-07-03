using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AbilityManager : MonoBehaviour {
    PlayerController playerController;
    [HideInInspector] public bool turtleOn = false;
    [HideInInspector] public bool sharkOn = false;
    [HideInInspector] public bool swordfishOn = false;
    private Dictionary<int, string> characterIndex = new Dictionary<int, string>
    {{0, "Anglerfish"}, {1, "Dolphin"}, {2, "Shark"}, {3, "Eel"}, {4, "Swordfish"}, {5, "Turtle"}};
    public float anglerfishAbilityDuration = 5f;
    public bool anglerfishAbilityVisionBoost = false;
    public bool anglerfishCooldownBool = false;
    public float anglerfishCooldown = 15f;
    public float dolphinAbilityDuration = 3f;
    public float dolphinAbilityMultiplier = 1.5f;
    public bool dolphinCooldownBool = false;
    public float dolphinCooldown = 10f;
    public float sharkAbilityDuration = 1f;
    public float sharkAbilityForce = 500f;
    public bool sharkCooldownBool = false;
    public float sharkCooldown = 8f;
    public float eelRadius = 10f;
    public bool eelCooldownBool = false;
    public float eelCooldown = 12f;
    public float swordfishAbilityDuration = 3f;
    public float swordfishAbilitySlowFactor = 0.25f;
    public bool swordfishCooldownBool = false;
    public float swordfishCooldown = 10f;
    public float turtleAbilityDuration = 5f;
    public bool turtleCooldownBool = false;
    public float turtleCooldown = 15f;
    public void UseAbility() {
        playerController = FindAnyObjectByType<PlayerController>();
        string characterName = characterIndex[DataCarrier.Instance.selectedCharacterIndex];
        switch (characterName) {
            case "Anglerfish":
                anglerfishAbility();
                break;
            case "Dolphin":
                dolphinAbility();
                break;
            case "Shark":
                sharkAbility();
                break;
            case "Eel":
                eelAbility();
                break;
            case "Swordfish":
                swordfishAbility();
                break;
            case "Turtle":
                turtleAbility();
                break;
        }
    }
    public void anglerfishAbility() { // increases your vision for 5 seconds, cooldown of 15 seconds after use
        if (anglerfishCooldownBool) {
            Debug.Log("Anglerfish ability is on cooldown!");
            AbilityNotReady();
            return;
        }
        Debug.Log("Anglerfish ability activated");
        StartCoroutine(AnglerfishVisionBoost());
        StartCoroutine(AnglerfishCooldown());
    }
    public void dolphinAbility() { // increases your acceleration and max speed for 3 seconds, cooldown of 10 seconds after use
        if (dolphinCooldownBool) {
            Debug.Log("Dolphin ability is on cooldown!");
            AbilityNotReady();
            return;
        }
        Debug.Log("Dolphin ability activated");
        StartCoroutine(DolphinSpeedBoost());
        StartCoroutine(DolphinCooldown());
    }
    public void sharkAbility() { // small invulnerable dash that breaks obstacles, cooldown of 8 seconds after use
        if (sharkCooldownBool) {
            Debug.Log("Shark ability is on cooldown!");
            AbilityNotReady();
            return;
        }
        Debug.Log("Shark ability activated");
        StartCoroutine(SharkAttack());
        StartCoroutine(SharkCooldown());
    }
    public void eelAbility() { // disables nearby obstacles for the rest of the race, cooldown of 12 seconds after use
        if (eelCooldownBool) {
            Debug.Log("Eel ability is on cooldown!");
            AbilityNotReady();
            return;
        }
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
        StartCoroutine(EelCooldown());
    }
    public void swordfishAbility() { // slows down time for 2 seconds, cooldown of 10 seconds after use
        if (swordfishCooldownBool) {
            Debug.Log("Swordfish ability is on cooldown!");
            AbilityNotReady();
            return;
        }
        Debug.Log("Swordfish ability activated");
        StartCoroutine(SwordfishTime());
        StartCoroutine(SwordfishCooldown());
    }
    public void turtleAbility() { // make the player immune to obstacles for 5 seconds, cooldown of 15 seconds after use
         if (turtleCooldownBool) {
            Debug.Log("Turtle ability is on cooldown!");
            AbilityNotReady();
            return;
        }
        Debug.Log("Turtle ability activated");
        StartCoroutine(TurtleInvincibility());
        StartCoroutine(TurtleCooldown());
    }
    IEnumerator AnglerfishVisionBoost() {
        anglerfishAbilityVisionBoost = true;
        yield return new WaitForSeconds(anglerfishAbilityDuration); 
        anglerfishAbilityVisionBoost = false;
    }
    IEnumerator AnglerfishCooldown() {
        anglerfishCooldownBool = true;
        yield return new WaitForSeconds(anglerfishCooldown); 
        anglerfishCooldownBool = false;
    }
    IEnumerator DolphinSpeedBoost() {
        float originalAcceleration = playerController.accelerationForce;
        float originalMaxSpeed = playerController.maxSpeed;
        playerController.accelerationForce *= dolphinAbilityMultiplier;
        playerController.maxSpeed *= dolphinAbilityMultiplier; 
        yield return new WaitForSeconds(dolphinAbilityDuration); 
        playerController.accelerationForce = originalAcceleration;
        playerController.maxSpeed = originalMaxSpeed;
    }
    IEnumerator DolphinCooldown() {
        dolphinCooldownBool = true;
        yield return new WaitForSeconds(dolphinCooldown); 
        dolphinCooldownBool = false;
    }
    IEnumerator TurtleInvincibility() {
        turtleOn = true;
        yield return new WaitForSeconds(turtleAbilityDuration); 
        turtleOn = false;
    }
    IEnumerator TurtleCooldown() {
        turtleCooldownBool = true;
        yield return new WaitForSeconds(turtleCooldown); 
        turtleCooldownBool = false;
    }
    IEnumerator EelCooldown() {
        eelCooldownBool = true;
        yield return new WaitForSeconds(eelCooldown); 
        eelCooldownBool = false;
    }
    IEnumerator SharkAttack() {
        sharkOn = true;
        Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
        playerRb.AddRelativeForce(Vector2.up * sharkAbilityForce); // adding a quick dash forward
        yield return new WaitForSeconds(sharkAbilityDuration);
        sharkOn = false;
    }
    IEnumerator SharkCooldown() {
        sharkCooldownBool = true;
        yield return new WaitForSeconds(sharkCooldown); 
        sharkCooldownBool = false;
    }
    IEnumerator SwordfishTime() {
        swordfishOn = true;
        yield return new WaitForSecondsRealtime(swordfishAbilityDuration); // wait for real time seconds
        swordfishOn = false;
    }
    IEnumerator SwordfishCooldown() {
        swordfishCooldownBool = true;
        yield return new WaitForSeconds(swordfishCooldown); 
        swordfishCooldownBool = false;
    }
    public void AbilityNotReady() {
        // you can add a UI popup or sound effect here to indicate that the ability is not ready yet
    }
}