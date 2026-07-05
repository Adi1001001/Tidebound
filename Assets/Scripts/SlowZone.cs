using UnityEngine;

public class SlowZone : MonoBehaviour {
    public float speedFactor = 0.4f;
    public float accelerationFactor = 0.7f;
    public bool breakableByShark;
    public bool active = true;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();

        if (sr != null && poly != null && sr.sprite != null)
        {
            // Rebuild the collider from the sprite's physics shape
            poly.pathCount = sr.sprite.GetPhysicsShapeCount();

            for (int i = 0; i < poly.pathCount; i++)
            {
                var points = new System.Collections.Generic.List<Vector2>();
                sr.sprite.GetPhysicsShape(i, points);
                poly.SetPath(i, points);
            }
        }
    }
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
