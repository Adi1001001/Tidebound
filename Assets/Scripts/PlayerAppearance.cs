using UnityEngine;
using System.Collections.Generic;

public class PlayerAppearance : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public CharacterType character;
        public Sprite sprite;
    }

    [SerializeField] private CharacterData[] characters;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        ApplyCharacter();
    }

    public void ApplyCharacter()
    {
        CharacterType current = DataCarrier.Instance.currentCharacter;

        foreach (CharacterData character in characters)
        {
            if (character.character == current)
            {
                spriteRenderer.sprite = character.sprite;

                RebuildCollider(character.sprite);
                return;
            }
        }

        Debug.LogWarning($"No sprite assigned for character {current}.");
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