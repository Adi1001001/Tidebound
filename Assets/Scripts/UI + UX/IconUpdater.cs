using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum IconType
{
    Teleport
}

[System.Serializable]
public class IconEntry
{
    public IconType type;
    public Sprite sprite;
}

public class IconUpdater : MonoBehaviour
{
    [SerializeField] private List<IconEntry> icons;

    private Dictionary<IconType, Sprite> iconDictionary;

    private Image buttonIcon;
    private Image cooldownOverlay;
    private Outline buttonOutline;

    private Color normalIconColor = Color.red;
    private Color cooldownOverlayColor = new Color(0f, 0f, 0f, 0.5f);
    private Color activeOutlineColor;
    [HideInInspector] public float timer;
    [HideInInspector] public float timerMax;
    [HideInInspector] public bool onAbility;

    void Awake()
    {
        buttonIcon = transform.Find("Icon").GetComponent<Image>();
        cooldownOverlay = transform.Find("Cooldown Overlay").GetComponent<Image>();
        buttonOutline = GetComponent<Outline>();

        cooldownOverlay.color = cooldownOverlayColor;
        cooldownOverlay.fillAmount = 0;

        buttonOutline.enabled = false;

        iconDictionary = new Dictionary<IconType, Sprite>();

        foreach (IconEntry entry in icons)
        {
            iconDictionary[entry.type] = entry.sprite;
        }
    }

    void Start()
    {
        switch (DataCarrier.Instance.currentCharacter)
        {
            case Character.Anglerfish:
                activeOutlineColor = new Color(1f, 0.5f, 0f);
                break;
            case Character.Dolphin:
                activeOutlineColor = new Color(0f, 1f, 1f);
                break;
            case Character.Swordfish:
                activeOutlineColor = new Color(0.4f, 1f, 0.2f);
                break;
            case Character.Turtle:
                activeOutlineColor = new Color(0.65f, 0.15f, 0.95f);
                break;
        }
    }

    void Update()
    {
        UpdateCooldownVisual();
        UpdateAbilityVisual();
    }

    private void UpdateCooldownVisual()
    {
        if (timer > 0 && timerMax > 0)
        {
            cooldownOverlay.fillAmount = timer / timerMax;
        }
        else
        {
            cooldownOverlay.fillAmount = 0;
        }
    }

    private void UpdateAbilityVisual()
    {
        if (onAbility)
        {
            buttonIcon.color = normalIconColor;
            buttonOutline.enabled = true;

            float glow = (Mathf.Sin(Time.time * 6f) + 1f) / 2f;
            Color glowColor = activeOutlineColor;
            glowColor.a = Mathf.Lerp(0.3f, 1f, glow);
            buttonOutline.effectColor = glowColor;
        }
        else if (timer > 0)
        {
            Color dimmedIcon = normalIconColor;
            dimmedIcon.a = 0.5f;
            buttonIcon.color = dimmedIcon;
            buttonOutline.enabled = false;
        }
        else
        {
            buttonIcon.color = normalIconColor;
            buttonOutline.enabled = false;
        }
    }

    public void SetIcon(IconType type)
    {
        if (iconDictionary.TryGetValue(type, out Sprite sprite))
        {
            buttonIcon.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("No sprite found for icon type: " + type);
        }
    }
}