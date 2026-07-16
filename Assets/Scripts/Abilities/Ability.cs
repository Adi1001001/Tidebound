using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] public float duration;
    [SerializeField] public float cooldown;

    protected PlayerController playerController;

    [HideInInspector] public bool onAbility = false;
    [HideInInspector] public bool onCooldown = false;
    public float timer;
    private Coroutine activeCoroutine;

    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void UseAbility()
    {
        if (onAbility || onCooldown)
        {
            return;
        }
        activeCoroutine = StartCoroutine(AbilityWrapper());
    }

    public void EndAbility()
    {
        if (!onAbility)
            return;

        timer = 0;
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
        yield return RunTimer(cooldown);
        onCooldown = false;
    }

    protected IEnumerator RunTimer(float time)
    {
        timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 0;
    }

    protected virtual void OnAbilityEnd()
    {
    }

    protected abstract IEnumerator AbilityRoutine();
}