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
    }

    void Start()
    {
        ResizeCollider();
        CheckGate();
    }

    public void ResizeCollider()
    {
        if (spriteRenderer == null || boxCollider == null)
            return;

        Bounds bounds = spriteRenderer.bounds;

        Vector3 localMin = transform.InverseTransformPoint(bounds.min);
        Vector3 localMax = transform.InverseTransformPoint(bounds.max);

        boxCollider.offset = (localMin + localMax) / 2f;
        boxCollider.size = localMax - localMin;

        boxCollider.isTrigger = false;
    }

    public void CheckGate()
    {
        if (DataCarrier.Instance.GetProgress() >= requiredProgress)
        {
            Destroy(gameObject);
        }
    }
}