using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {
    PlayerController playerController;
    [HideInInspector] public bool turtleOn = false;
    [HideInInspector] public bool sharkOn = false;
    [HideInInspector] public bool swordfishOn = false;
    private Dictionary<int, string> characterIndex = new Dictionary<int, string>
    {{0, "Anglerfish"}, {1, "Dolphin"}, {2, "Shark"}, {3, "Eel"}, {4, "Swordfish"}, {5, "Turtle"}};
    public float anglerfishAbilityDuration = 5f;
    public bool anglerfishAbilityVisionBoost = false;
    public float dolphinAbilityDuration = 3f;
    public float dolphinAbilityMultiplier = 1.5f;
    public float sharkAbilityDuration = 1f;
    public float sharkAbilityForce = 500f;
    public float eelRadius = 3f;
    public float swordfishAbilityDuration = 2f;
    public float swordfishAbilitySlowFactor = 0.25f;
    public float turtleAbilityDuration = 5f;
    public float abilityCooldown = 10f;
    public void UseAbility() {
        playerController = FindFirstObjectByType<PlayerController>();
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
    public void anglerfishAbility() { // increases your vision for 5 seconds
        Debug.Log("Anglerfish ability activated");
        StartCoroutine(AnglerfishVisionBoost());
    }
    public void dolphinAbility() { // increases your acceleration and max speed for 3 seconds
        Debug.Log("Dolphin ability activated");
        StartCoroutine(DolphinSpeedBoost());
    }
    public void sharkAbility() { // small invulnerable dash that breaks obstacles
        Debug.Log("Shark ability activated");
        StartCoroutine(SharkAttack());
    }
    public void eelAbility() { // disables nearby obstacles for the rest of the race
        Debug.Log("Eel ability activated");

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, eelRadius);

        foreach (Collider2D hit in hitObjects) {
            if (hit.gameObject == gameObject) continue; // avoiding yourself
            GameObject collidedObject = hit.gameObject;
            Obstacle obstacle = collidedObject.GetComponent<Obstacle>();

            if (obstacle == null) continue; // only affect obstacles

            Renderer renderer = collidedObject.GetComponent<Renderer>();
            renderer.material.color = Color.grey; // change color to indicate that it can't effect the player anymore
            obstacle.active = false; // disable the obstacle
        }
    }
    public void swordfishAbility() { // slows down time for 2 seconds
        Debug.Log("Swordfish ability activated");
        StartCoroutine(SwordfishTime());
    }
    public void turtleAbility() { // make the player immune to obstacles for 5 seconds
        Debug.Log("Turtle ability activated");
        StartCoroutine(TurtleInvincibility());
    }
    IEnumerator AnglerfishVisionBoost() {
        anglerfishAbilityVisionBoost = true;
        yield return new WaitForSeconds(anglerfishAbilityDuration); 
        anglerfishAbilityVisionBoost = false;
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
    IEnumerator TurtleInvincibility() {
        turtleOn = true;
        yield return new WaitForSeconds(turtleAbilityDuration); 
        turtleOn = false;
    }
    IEnumerator SharkAttack() {
        sharkOn = true;
        Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
        playerRb.AddRelativeForce(Vector2.up * sharkAbilityForce); // adding a quick dash forward
        yield return new WaitForSeconds(sharkAbilityDuration);
        sharkOn = false;
    }
    IEnumerator SwordfishTime() {
        swordfishOn = true;
        yield return new WaitForSecondsRealtime(swordfishAbilityDuration); // wait for real time seconds
        swordfishOn = false;
    }
}
// you need to add a cooldown, ui button, popup text to tell you that you can't use it again yet