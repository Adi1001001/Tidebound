using UnityEngine;

[ExecuteAlways]
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
    private Vector3 lastScale;
    private Quaternion lastRotation;
    [SerializeField] private CurrentArrowSprite[] sprites;


    private void OnEnable()
    {
        lastScale = transform.localScale;
        lastRotation = transform.localRotation;

        GenerateArrows();
    }

    void Start()
    {
        SetSprite();
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isPlaying)
            return;


        if (transform.localScale != lastScale ||
            transform.localRotation != lastRotation)
        {
            lastScale = transform.localScale;
            lastRotation = transform.localRotation;

            GenerateArrows();
        }
    }
#endif

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
            return;


        ClearArrows();


        SpriteRenderer current = GetComponent<SpriteRenderer>();

        if (current == null || current.sprite == null)
            return;



        /*
         * Everything below is LOCAL SPACE.
         * Rotation does not affect these values.
         */

        Vector2 spriteSize =
            current.sprite.bounds.size;


        float rectWidth = spriteSize.x;
        float rectHeight = spriteSize.y;



        // Resize the rectangle collider
        BoxCollider2D box =
            GetComponent<BoxCollider2D>();

        if (box != null)
        {
            box.size = new Vector2(
                rectWidth,
                rectHeight
            );

            box.offset = Vector2.zero;
        }



        float ppu =
            arrowSprite.pixelsPerUnit;



        float arrowWidth =
            arrowWidthPixels / ppu /
            transform.localScale.x;


        float arrowHeight =
            arrowHeightPixels / ppu /
            transform.localScale.y;


        float gapX =
            horizontalGapPixels / ppu /
            transform.localScale.x;


        float gapY =
            verticalGapPixels / ppu /
            transform.localScale.y;



        // 10% empty border on each side
        float usableWidth =
            rectWidth *
            (1f - boundaryMargin * 2f);


        float usableHeight =
            rectHeight *
            (1f - boundaryMargin * 2f);



        int columns =
            Mathf.Max(
                1,
                Mathf.FloorToInt(
                    (usableWidth + gapX) /
                    (arrowWidth + gapX)
                )
            );


        int rows =
            Mathf.Max(
                1,
                Mathf.FloorToInt(
                    (usableHeight + gapY) /
                    (arrowHeight + gapY)
                )
            );



        float totalWidth =
            columns * arrowWidth +
            (columns - 1) * gapX;


        float totalHeight =
            rows * arrowHeight +
            (rows - 1) * gapY;



        // Center inside the usable area
        float startX =
            -usableWidth * 0.5f +
            (usableWidth - totalWidth) * 0.5f +
            arrowWidth * 0.5f;


        float startY =
            -usableHeight * 0.5f +
            (usableHeight - totalHeight) * 0.5f +
            arrowHeight * 0.5f;



        float arrowScaleX =
            arrowWidthPixels /
            arrowSprite.rect.width /
            transform.localScale.x;


        float arrowScaleY =
            arrowHeightPixels /
            arrowSprite.rect.height /
            transform.localScale.y;



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



    private void CreateArrow(
        float x,
        float y,
        float scaleX,
        float scaleY)
    {
        GameObject arrow =
            new GameObject("Arrow");


        arrow.transform.SetParent(
            transform,
            false
        );


        arrow.transform.localPosition =
            new Vector3(x, y, 0f);


        arrow.transform.localRotation =
            Quaternion.identity;


        arrow.transform.localScale =
            new Vector3(
                scaleX,
                scaleY,
                1f
            );



        SpriteRenderer sr =
            arrow.AddComponent<SpriteRenderer>();


        sr.sprite = arrowSprite;

        sr.sortingOrder = sortingOrder;
    }



    private void ClearArrows()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).name == "Arrow")
            {
#if UNITY_EDITOR
                DestroyImmediate(
                    transform.GetChild(i).gameObject
                );
#else
                Destroy(
                    transform.GetChild(i).gameObject
                );
#endif
            }
        }
    }
}