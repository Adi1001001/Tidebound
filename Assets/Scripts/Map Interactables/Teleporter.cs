using UnityEngine;

public class Teleporter : MonoBehaviour {
    [System.Serializable]
    public class TeleporterSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }
    public string teleportTag;
    [SerializeField] private TeleporterSprite[] sprites;
    void Start() {
        SetSprite();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        CircleCollider2D cc = GetComponent<CircleCollider2D>();

        Bounds bounds = sr.sprite.bounds; // Local-space bounds of the sprite

        // Radius that encloses the entire sprite
        cc.radius = Mathf.Max(bounds.extents.x, bounds.extents.y);

        // Center the collider on the sprite
        cc.offset = bounds.center;
    }

    private void SetSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int currentBiome = DataCarrier.Instance.GetBiomeNum();
        foreach (TeleporterSprite teleporterSprite in sprites)
        {
            if (teleporterSprite.biomeNum == currentBiome)
            {
                sr.sprite = teleporterSprite.sprite;
                return;
            }
        }
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
    public void OnRaceClick() {
        Debug.Log("Teleport triggered");
        DataCarrier.Instance.UpdateTag(teleportTag);
        LevelManager.Instance.ToCharacterSelect();
    }
}
