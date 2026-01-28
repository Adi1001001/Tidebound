using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class AbilityManager : MonoBehaviour {
    PlayerController playerController;
    TimerManager timerManager;
    [HideInInspector] public bool turtleOn = false;
    [HideInInspector] public bool sharkOn = false;
    [HideInInspector] public bool swordfishOn = false;
    private Dictionary<int, string> characterIndex = new Dictionary<int, string>
    {{0, "Clownfish"}, {1, "Dolphin"}, {2, "Shark"}, {3, "Octopus"}, {4, "Swordfish"}, {5, "Turtle"}};
    public float clownfishAbilityDuration = 4f;
    public float dolphinAbilityDuration = 3f;
    public float dolphinAbilityMultiplier = 1.5f;
    public float sharkAbilityDuration = 1f;
    public float sharkAbilityForce = 500f;
    public float octopusAbilityDuration = 6f;
    public float swordfishAbilityDuration = 2f;
    public float swordfishAbilitySlowFactor = 0.25f;
    public float turtleAbilityDuration = 5f;
    public float abilityCooldown = 10f;
    public void UseAbility() {
        playerController = FindFirstObjectByType<PlayerController>();
        string characterName = characterIndex[DataCarrier.Instance.selectedCharacterIndex];
        switch (characterName) {
            case "Clownfish":
                clownfishAbility(playerController);
                break;
            case "Dolphin":
                dolphinAbility(playerController);
                break;
            case "Shark":
                sharkAbility(playerController);
                break;
            case "Octopus":
                octopusAbility(playerController);
                break;
            case "Swordfish":
                swordfishAbility(playerController);
                break;
            case "Turtle":
                turtleAbility();
                break;
        }
    }
    public void clownfishAbility(PlayerController playerController) {
        
    }
    public void dolphinAbility(PlayerController playerController) { // increases your acceleartion and max speed for 3 seconds
        Debug.Log("Dolphin ability activated");
        playerController.StartCoroutine(DolphinSpeedBoost());
    }
    public void sharkAbility(PlayerController playerController) {
        Debug.Log("Shark ability activated");
        playerController.StartCoroutine(SharkAttack());
    }
    public void octopusAbility(PlayerController playerController) {
        
    }
    public void swordfishAbility(PlayerController playerController) { // slows down time for 2 seconds
        Debug.Log("Swordfish ability activated");
        playerController.StartCoroutine(SwordfishTime());
    }
    public void turtleAbility() { // make the player immune to obstacles for 5 seconds
        Debug.Log("Turtle ability activated");
        StartCoroutine(TurtleInvincibility());
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