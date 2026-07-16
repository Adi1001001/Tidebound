using UnityEngine;

public class VineGate : MonoBehaviour
{
    public int requiredProgress;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ResizeCollider();
    }

    void Start()
    {
        CheckGate();
    }

    public void ResizeCollider()
    {
        if (spriteRenderer == null || boxCollider == null)
            return;

        boxCollider.offset = spriteRenderer.sprite.bounds.center;
        boxCollider.size = spriteRenderer.sprite.bounds.size;

        boxCollider.isTrigger = false;
    }

    public void CheckGate()
    {
        if (DataCarrier.Instance.overworldProgress >= requiredProgress)
        {
            Destroy(gameObject);
        }
    }
}