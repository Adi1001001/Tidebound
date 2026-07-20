using UnityEngine;

public class VineGate : MonoBehaviour
{
    [System.Serializable]
    public class VineGateSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }
    public int requiredProgress;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private VineGateSprite[] sprites;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        SetSprite();
        ResizeCollider();
        CheckGate();
    }

    private void SetSprite()
    {
        int currentBiome = DataCarrier.Instance.GetBiomeNum();
        foreach (VineGateSprite vineGateSprite in sprites)
        {
            if (vineGateSprite.biomeNum == currentBiome)
            {
                spriteRenderer.sprite = vineGateSprite.sprite;
                return;
            }
        }
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
        if (DataCarrier.Instance.GetProgress() >= requiredProgress)
        {
            Destroy(gameObject);
        }
    }
}