using UnityEngine;
using TMPro;

public class HoverableInteract : MonoBehaviour {
    public string message;
    public GameObject textUI;
    private TMP_Text uiText;
    [HideInInspector] public bool isHovering;

    void Start() {
        if (textUI != null) {
            uiText = textUI.GetComponent<TMP_Text>();
            textUI.SetActive(false);
        } else {
            Debug.LogWarning("Text UI GameObject is not assigned in HoverableInteract script on " + gameObject.name);
        }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        isHovering = true;
        ShowText(true);
    }
    void OnTriggerExit2D(Collider2D collision) {
        isHovering = false;
        ShowText(false);
    }
    void ShowText(bool state) {
        if (textUI != null) {
            uiText.text = message;
            textUI.SetActive(state);
        }
    }

}
