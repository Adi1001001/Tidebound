using UnityEngine;

public class SlowZone : MonoBehaviour {
    public float speedFactor = 0.4f;
    public float accelerationFactor = 0.7f;
    public bool breakableByShark;
    public bool active = true;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        PlayerController player = collision.GetComponent<PlayerController>();
        AbilityManager ability = collision.GetComponent<AbilityManager>();

        if (!active) return;
        if (ability != null && ability.sharkOn)
        {
            if (breakableByShark)
                Destroy(gameObject);
            return;
        }
        if (player != null)
        {
            player.EnterSlowZone(speedFactor, accelerationFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

    PlayerController player = collision.GetComponent<PlayerController>();

    if (player != null)
        {
            player.ExitSlowZone();
        }
    }
}
