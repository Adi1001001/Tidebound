using UnityEngine;

public class AbilityLoader : MonoBehaviour
{
    private Ability currentAbility;

    private void Start()
    {
        switch (DataCarrier.Instance.currentCharacter)
        {
            case Character.Anglerfish:
                currentAbility = gameObject.AddComponent<AnglerfishAbility>();
                break;

            case Character.Dolphin:
                // currentAbility = gameObject.AddComponent<DolphinAbility>();
                break;

            case Character.Shark:
                // currentAbility = gameObject.AddComponent<SharkAbility>();
                break;

            case Character.Eel:
                // currentAbility = gameObject.AddComponent<EelAbility>();
                break;

            case Character.Swordfish:
                // currentAbility = gameObject.AddComponent<SwordfishAbility>();
                break;

            case Character.Turtle:
                // currentAbility = gameObject.AddComponent<TurtleAbility>();
                break;
        }
    }

    public void UseAbility()
    {
        currentAbility.UseAbility();
    }
}