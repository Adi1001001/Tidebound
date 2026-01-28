using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {
    PlayerController playerController;
    [HideInInspector] public bool turtleOn = false;
    [HideInInspector] public bool sharkOn = false;
    private Dictionary<int, string> characterIndex = new Dictionary<int, string>
    {{0, "Clownfish"}, {1, "Dolphin"}, {2, "Shark"}, {3, "Octopus"}, {4, "Swordfish"}, {5, "Turtle"}};
    float clownfishAbilityDuration = 4f;
    float dolphinAbilityDuration = 3f;
    float dolphinAbilityMultiplier = 1.5f;
    float sharkAbilityDuration = 1f;
    float sharkAbilityForce = 500f;
    float octopusAbilityDuration = 6f;
    float swordfishAbilityDuration = 2f;
    float turtleAbilityDuration = 5f;
    float abilityCooldown = 10f;
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
    public void swordfishAbility(PlayerController playerController) {
        
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
}
// you need to add a cooldown, ui button, popup text to tell you that you can't use it again yet