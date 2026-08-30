using UnityEngine;

public class CurrentArrowGenerator : MonoBehaviour
{
    [System.Serializable]
    public class CurrentArrowSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }

    [Header("Arrow")]
    [SerializeField] private Sprite arrowSprite;

    public float arrowWidthPixels = 32f;
    public float arrowHeightPixels = 32f;

    [Header("Spacing")]
    public float horizontalGapPixels = 32f;
    public float verticalGapPixels = 32f;

    [Header("Layout")]
    [Range(0f, 0.5f)]
    [SerializeField] private float boundaryMargin = 0.05f;

    [SerializeField] private int sortingOrder = 1;
    [SerializeField] private CurrentArrowSprite[] sprites;

    void Start()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        SetSprite();
        GenerateArrows();
    }

    private void SetSprite()
    {
        int currentBiome = DataCarrier.Instance.GetBiomeNum();

        foreach (CurrentArrowSprite currentArrowSprite in sprites)
        {
            if (currentArrowSprite.biomeNum == currentBiome)
            {
                arrowSprite = currentArrowSprite.sprite;
            }
        }
    }

    public void GenerateArrows()
    {
        if (arrowSprite == null)
        {
            return;
        }

        ClearArrows();

        SpriteRenderer current = GetComponent<SpriteRenderer>();

        if (current == null || current.sprite == null)
        {
            return;
        }

        Vector2 spriteSize;

        if (current.drawMode == SpriteDrawMode.Sliced ||
            current.drawMode == SpriteDrawMode.Tiled)
        {
            spriteSize = current.size;
        }
        else
        {
            spriteSize = current.sprite.bounds.size;
        }

        float rectWidth = spriteSize.x;
        float rectHeight = spriteSize.y;

        BoxCollider2D box = GetComponent<BoxCollider2D>();

        if (box != null)
        {
            box.size = new Vector2(rectWidth, rectHeight);
            box.offset = Vector2.zero;
        }

        float ppu = arrowSprite.pixelsPerUnit;
        float arrowWidth = arrowWidthPixels / ppu;
        float arrowHeight = arrowHeightPixels / ppu;
        float gapX = horizontalGapPixels / ppu;
        float gapY = verticalGapPixels / ppu;

        float usableWidth = rectWidth * (1f - boundaryMargin * 2f);
        float usableHeight = rectHeight * (1f - boundaryMargin * 2f);

        int columns = Mathf.Max(1, Mathf.FloorToInt(
            (usableWidth + gapX) / (arrowWidth + gapX)
        ));

        int rows = Mathf.Max(1, Mathf.FloorToInt(
            (usableHeight + gapY) / (arrowHeight + gapY)
        ));

        float totalWidth = columns * arrowWidth + (columns - 1) * gapX;
        float totalHeight = rows * arrowHeight + (rows - 1) * gapY;

        float startX = -usableWidth * 0.5f +
                       (usableWidth - totalWidth) * 0.5f +
                       arrowWidth * 0.5f;

        float startY = -usableHeight * 0.5f +
                       (usableHeight - totalHeight) * 0.5f +
                       arrowHeight * 0.5f;

        float arrowScaleX = arrowWidthPixels / arrowSprite.rect.width;
        float arrowScaleY = arrowHeightPixels / arrowSprite.rect.height;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                CreateArrow(
                    startX + x * (arrowWidth + gapX),
                    startY + y * (arrowHeight + gapY),
                    arrowScaleX,
                    arrowScaleY
                );
            }
        }
    }

    private void CreateArrow(float x, float y, float scaleX, float scaleY)
    {
        GameObject arrow = new GameObject("Arrow");

        arrow.transform.SetParent(transform, false);
        arrow.transform.localPosition = new Vector3(x, y, 0f);
        arrow.transform.localRotation = Quaternion.identity;
        arrow.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();
        sr.sprite = arrowSprite;
        sr.sortingOrder = sortingOrder;
    }

    private void ClearArrows()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).name == "Arrow")
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}