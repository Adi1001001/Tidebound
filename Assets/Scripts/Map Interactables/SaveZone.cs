using UnityEngine;

public class SaveZone : MonoBehaviour
{
    public int saveZoneID;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        boxCollider.isTrigger = true;

        ResizeCollider();
    }

    public void ResizeCollider()
    {
        if (spriteRenderer == null)
            return;

        boxCollider.offset = spriteRenderer.sprite.bounds.center;
        boxCollider.size = spriteRenderer.sprite.bounds.size;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        DataCarrier.Instance.SetSaveZone(saveZoneID);
    }
}