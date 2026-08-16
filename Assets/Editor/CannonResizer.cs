using UnityEngine;
using UnityEditor;

public class CannonResizer
{
    [MenuItem("Tools/Convert Cannon Sprites To Sliced")]
    static void Convert()
    {
        Cannon[] cannons = Object.FindObjectsByType<Cannon>();

        int converted = 0;

        foreach (Cannon cannon in cannons)
        {
            SpriteRenderer sr = cannon.GetComponent<SpriteRenderer>();

            if (sr == null || sr.sprite == null)
                continue;

            if (sr.drawMode != SpriteDrawMode.Simple)
                continue;

            Undo.RecordObject(sr, "Convert Cannon Sprite");
            Undo.RecordObject(sr.transform, "Convert Cannon Scale");

            // Current visual size before conversion
            Vector2 spriteSize = sr.sprite.bounds.size;
            Vector3 scale = sr.transform.localScale;

            Vector2 worldSize = new Vector2(
                spriteSize.x * scale.x,
                spriteSize.y * scale.y
            );

            // Convert Simple -> Sliced
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = worldSize;

            // Preserve the appearance, move sizing responsibility to SpriteRenderer
            sr.transform.localScale = Vector3.one;

            EditorUtility.SetDirty(sr);
            EditorUtility.SetDirty(sr.transform);

            converted++;
        }

        Debug.Log($"Converted {converted} Cannons to Sliced mode.");
    }
}