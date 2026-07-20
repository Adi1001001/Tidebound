using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    [System.Serializable]
    public class BiomeSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }

    [SerializeField] private BiomeSprite[] sprites;

    private void Awake()
    {
        StartCoroutine(TrySetSprite());
    }

    private System.Collections.IEnumerator TrySetSprite()
    {
        while (DataCarrier.Instance == null)
        {
            yield return null;
        }

        SetSprite();
    }
    private void SetSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int currentBiome = DataCarrier.Instance.GetBiomeNum();

        foreach (BiomeSprite biomeSprite in sprites)
        {
            if (biomeSprite.biomeNum == currentBiome)
            {
                sr.sprite = biomeSprite.sprite;
                return;
            }
        }
    }
}
