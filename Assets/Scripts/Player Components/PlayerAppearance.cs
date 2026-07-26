using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAppearance : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public Character character;
        public Sprite sprite;
    }

    [SerializeField] private CharacterData[] characters;
    [SerializeField] private float minShadowScale = 0.4f;
    [SerializeField] private float shadowOffsetScale = 2f;

    private GameObject airShadow;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Vector3 originalScale;
    private Coroutine airborneCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        airShadow = transform.Find("Air Shadow").gameObject;

        originalScale = transform.localScale;
    }

    private void Start()
    {
        ApplySize();
        ApplyCharacter();
        airShadow.SetActive(false);
    }

    private void ApplySize()
    {
        Character current = DataCarrier.Instance.currentCharacter;
        switch (current)
        {
            case Character.Anglerfish:
                transform.localScale = new Vector3(0.75f, 0.65f, 1f);
                break;
            case Character.Swordfish:
                transform.localScale = new Vector3(1.1f, 0.7f, 1f);
                break;
            case Character.Turtle:
                transform.localScale = new Vector3(0.85f, 0.85f, 1f);
                break;
        }
    }

    private void ApplyCharacter()
    {
        Character current = DataCarrier.Instance.currentCharacter;
        SpriteRenderer shadowRenderer = airShadow.GetComponent<SpriteRenderer>();

        foreach (CharacterData character in characters)
        {
            if (character.character == current)
            {
                spriteRenderer.sprite = character.sprite;
                shadowRenderer.sprite = character.sprite;

                shadowRenderer.color = new Color(0, 0, 0, 0.35f);

                RebuildCollider(character.sprite);
                return;
            }
        }

        Debug.LogWarning($"No sprite assigned for character {current}.");
    }

    public void StartAirborneEffect(float duration, Vector2 launchDirection, float launchStrength)
    {
        if (airborneCoroutine != null)
        {
            StopCoroutine(airborneCoroutine);
        }
        airborneCoroutine = StartCoroutine(PlayAirborneEffect(duration, launchDirection, launchStrength));
    }

    public IEnumerator PlayAirborneEffect(float duration, Vector2 launchDirection, float launchStrength)
    {
        airShadow.SetActive(true);
        float timer = 0f;

        float jumpIntensity = launchStrength / 20f;
        float currentJumpScale = 1f + (0.2f * jumpIntensity);
        float currentShadowOffset = shadowOffsetScale * jumpIntensity;

        Vector3 shadowOffset = -(Vector3)launchDirection.normalized * currentShadowOffset;
        float currentMinShadowScale = Mathf.Lerp(1f, minShadowScale, Mathf.Clamp01(jumpIntensity/2.5f));
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float height = Mathf.Sin(timer / duration * Mathf.PI);
            // Shadow Effect
            transform.localScale = Vector3.Lerp(originalScale, originalScale * currentJumpScale, height);
            float shadowScale = Mathf.Lerp(1f, currentMinShadowScale, height);
            airShadow.transform.localScale = new Vector3(shadowScale, shadowScale * 0.7f, 1f);
            airShadow.transform.position = transform.position + shadowOffset * height;
            yield return null;
        }
        transform.localScale = originalScale;
        airShadow.transform.localScale = Vector3.one;
        airShadow.transform.localPosition = Vector3.zero;
        airShadow.SetActive(false);
    }

    public void StopAirborneEffect()
    {
        StopAllCoroutines();

        transform.localScale = originalScale;
        airShadow.transform.localScale = Vector3.one;
        airShadow.SetActive(false);
    }

    private void RebuildCollider(Sprite sprite)
    {
        if (polygonCollider == null || sprite == null) return;

        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            List<Vector2> points = new List<Vector2>();
            sprite.GetPhysicsShape(i, points);
            polygonCollider.SetPath(i, points);
        }
    }
}