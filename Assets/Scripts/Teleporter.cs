using UnityEngine;

public class Teleporter : MonoBehaviour {
    public string teleportTag;
    void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        CircleCollider2D cc = GetComponent<CircleCollider2D>();

        Bounds bounds = sr.sprite.bounds; // Local-space bounds of the sprite

        // Radius that encloses the entire sprite
        cc.radius = Mathf.Max(bounds.extents.x, bounds.extents.y);

        // Center the collider on the sprite
        cc.offset = bounds.center;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HoverManager>().SetNearbyTeleporter(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HoverManager>().SetNearbyTeleporter(null);
        }
    }
    public void OnRaceClick() {
        Debug.Log("Teleport triggered");
        DataCarrier.Instance.UpdateTag(teleportTag);
        LevelManager.Instance.ToCharacterSelect();
    }
}
