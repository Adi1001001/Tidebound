using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] protected float cooldown;

    protected PlayerController playerController;

    [HideInInspector] public bool onAbility = false;
    [HideInInspector] public bool onCooldown = false;

    private Coroutine activeCoroutine;

    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void UseAbility()
    {
        if (onAbility)
        {
            Debug.Log("Ability already active!");
            return;
        }

        if (onCooldown)
        {
            Debug.Log("Ability on cooldown!");
            return;
        }
        activeCoroutine = StartCoroutine(AbilityWrapper());
    }

    public void EndAbility()
    {
        if (!onAbility)
            return;

        StopCoroutine(activeCoroutine);
        onAbility = false;
        OnAbilityEnd();
        StartCoroutine(StartCooldown());
    }

    private IEnumerator AbilityWrapper()
    {
        onAbility = true;
        yield return AbilityRoutine();
        EndAbility();
    }

    private IEnumerator StartCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    protected virtual void OnAbilityEnd()
    {
    }

    protected abstract IEnumerator AbilityRoutine();
}