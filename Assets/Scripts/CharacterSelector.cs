using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelector : MonoBehaviour {
    public GameObject[] characterPanels;
    public PlayerController playerController;
    void Start() {
        EventSystem.current.SetSelectedGameObject(characterPanels[0]);
    }
    public void SelectCharacter(int index) {
        playerController.selectedCharacterIndex = index;

        for (int i = 0; i < characterPanels.Length; i++) {
            characterPanels[i].SetActive(false);
        }

        if (index >= 0 && index < characterPanels.Length) {
            characterPanels[index].SetActive(true);
        }
    }
}
