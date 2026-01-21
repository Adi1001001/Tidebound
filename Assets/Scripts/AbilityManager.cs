using UnityEngine;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {
    PlayerController playerController;
    private Dictionary<int, string> characterIndex = new Dictionary<int, string>
    {{0, "Clownfish"}, {1, "Dolphin"}, {2, "Shark"}, {3, "Octopus"}, {4, "Swordfish"}, {5, "Turtle"}};
    public void UseAbility(int selectedCharacterIndex) {
        playerController = FindFirstObjectByType<PlayerController>();
        string characterName = characterIndex[selectedCharacterIndex];
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
                turtleAbility(playerController);
                break;
        }
    }
    public void clownfishAbility(PlayerController playerController) {
        
    }
    public void dolphinAbility(PlayerController playerController) {
        
    }
    public void sharkAbility(PlayerController playerController) {
        
    }
    public void octopusAbility(PlayerController playerController) {
        
    }
    public void swordfishAbility(PlayerController playerController) {
        
    }
    public void turtleAbility(PlayerController playerController) {
        
    }
}
