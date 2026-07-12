using UnityEngine;
using TMPro;

public class HoverableInteract : MonoBehaviour {
    public string message;
    public GameObject textUI;
    [HideInInspector] public bool isHovering;

    void OnTriggerEnter2D(Collider2D collision) {
        isHovering = true;
        if (textUI != null)
        {
            textUI.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        isHovering = false;
        if (textUI != null)
        {
            textUI.SetActive(false);
        }
    }
}
