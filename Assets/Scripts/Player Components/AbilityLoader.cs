using UnityEngine;

public class AbilityLoader : MonoBehaviour
{
    [SerializeField] private GameObject lilypadPrefab;
    private void Start()
    {
        switch (DataCarrier.Instance.currentCharacter)
        {
            case Character.Anglerfish:
                gameObject.AddComponent<AnglerfishAbility>();
                break;

            case Character.Dolphin:
                gameObject.AddComponent<DolphinAbility>();
                break;

            case Character.Swordfish:
                gameObject.AddComponent<SwordfishAbility>();
                break;

            case Character.Turtle:
                TurtleAbility currentAbility = gameObject.AddComponent<TurtleAbility>();
                currentAbility.Initialise(lilypadPrefab);
                break;
        }
    }
}