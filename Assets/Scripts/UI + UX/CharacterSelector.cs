using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour {
    public GameObject[] selectionPanels;
    public GameObject[] infoPanels;
    public Sprite[] unlockedSprites;
    public Sprite[] selectedSprites;
    public Sprite[] lockedSprites;    
    private int currIndex = 0;
    void Start() {
        SelectCharacter(0);
        Button defaultSelection = GameObject.Find("Anglerfish").GetComponent<Button>();
        defaultSelection.Select();
        LockCharacters();
    }

    public void SelectCharacter(int index)
    {
        DataCarrier.Instance.SetCharacter((Character)index);
        SetInfoPanel(index);
        AdjustSelection(index);
    }

    private void AdjustSelection(int index)
    {
        GameObject currCharacter = selectionPanels[currIndex];
        currCharacter.GetComponent<Image>().sprite = unlockedSprites[currIndex];

        currIndex = index;
        currCharacter = selectionPanels[currIndex];
        currCharacter.GetComponent<Image>().sprite = selectedSprites[currIndex];
    }

    private void SetInfoPanel(int index)
    {
        for (int i = 0; i < infoPanels.Length; i++)
        {
            infoPanels[i].SetActive(false);
        }

        if (index >= 0 && index < infoPanels.Length)
        {
            infoPanels[index].SetActive(true);
        }
    }

    private void LockCharacters()
    {
        int currProgress = DataCarrier.Instance.GetProgress();
        if (currProgress < 1)
        {
            GameObject dolphin = selectionPanels[1];
            dolphin.GetComponent<Button>().interactable = false;
            dolphin.GetComponent<Image>().sprite = lockedSprites[0];
        }
        if (currProgress < 3)
        {
            GameObject swordfish = selectionPanels[2];
            swordfish.GetComponent<Button>().interactable = false;
            swordfish.GetComponent<Image>().sprite = lockedSprites[1];
        }
        if (currProgress < 5)
        {
            GameObject turtle = selectionPanels[3];
            turtle.GetComponent<Button>().interactable = false;
            turtle.GetComponent<Image>().sprite = lockedSprites[2];
        }
    }
}
