using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteLoader : MonoBehaviour
{
    [System.Serializable]
    public class BiomeSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }

    [SerializeField] private BiomeSprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TrySetSprite());
    }

    private IEnumerator TrySetSprite()
    {
        while (DataCarrier.Instance == null)
        {
            yield return null;
        }

        SetSprite();
    }

    private void SetSprite()
    {
        int currentBiome = DataCarrier.Instance.GetBiomeNum();

        foreach (BiomeSprite biomeSprite in sprites)
        {
            if (biomeSprite.biomeNum == currentBiome)
            {
                spriteRenderer.sprite = biomeSprite.sprite;
                ResizeCollider();
                return;
            }
        }
    }

    private void ResizeCollider()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            return;
        }

        bool isSliced = spriteRenderer.drawMode == SpriteDrawMode.Sliced;

        Vector2 size;
        Vector2 offset;

        if (isSliced)
        {
            size = spriteRenderer.size;
            offset = spriteRenderer.sprite.bounds.center;
        }
        else
        {
            size = spriteRenderer.sprite.bounds.size;
            offset = spriteRenderer.sprite.bounds.center;
        }

        BoxCollider2D box = GetComponent<BoxCollider2D>();

        if (box != null)
        {
            box.offset = offset;
            box.size = size;
        }

        CircleCollider2D circle = GetComponent<CircleCollider2D>();

        if (circle != null)
        {
            circle.offset = offset;

            float diameter = Mathf.Min(size.x, size.y);
            circle.radius = diameter / 2f;
        }

        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();

        if (capsule != null)
        {
            capsule.offset = offset;
            capsule.size = size;
        }

        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();

        if (polygon != null)
        {
            ResizePolygonCollider(polygon, isSliced, size);
        }
    }

    private void ResizePolygonCollider(PolygonCollider2D polygon, bool isSliced, Vector2 targetSize)
    {
        Sprite sprite = spriteRenderer.sprite;
        int pathCount = sprite.GetPhysicsShapeCount();

        if (pathCount == 0)
        {
            return;
        }

        polygon.pathCount = pathCount;

        Vector2 originalSize = sprite.bounds.size;
        float scaleX = targetSize.x / originalSize.x;
        float scaleY = targetSize.y / originalSize.y;

        for (int i = 0; i < pathCount; i++)
        {
            List<Vector2> points = new List<Vector2>();

            sprite.GetPhysicsShape(i, points);

            if (isSliced)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    points[j] = new Vector2(
                        points[j].x * scaleX,
                        points[j].y * scaleY
                    );
                }
            }

            polygon.SetPath(i, points);
        }
    }
}