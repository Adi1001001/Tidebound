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
            collision.GetComponent<PlayerActionManager>().SetNearbyTeleporter(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerActionManager>().SetNearbyTeleporter(null);
        }
    }
    public void OnTeleportClick() {
        Debug.Log("Teleport triggered");
        if (teleportTag == "Overworld 2")
        {
            DataCarrier.Instance.SetBiomeNum(2);
            LevelManager.Instance.ToOverworld();
        }
        else if (teleportTag == "Overworld 3")
        {
            DataCarrier.Instance.SetBiomeNum(3);
            LevelManager.Instance.ToOverworld();
        }
        else
        {
            DataCarrier.Instance.UpdateTag(teleportTag);
            LevelManager.Instance.ToCharacterSelect();
        }
    }
}
