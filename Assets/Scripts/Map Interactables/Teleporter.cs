using System;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public string teleportTag;
    void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        CircleCollider2D cc = GetComponent<CircleCollider2D>();

        Vector2 size = sr.size;
        float radius = Math.Min(size.x, size.y)/2;

        cc.radius = Mathf.Max(radius, radius);

        cc.offset = new Vector2(0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerActionManager>().SetNearbyTeleporter(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerActionManager>().SetNearbyTeleporter(null);
        }
    }
    public void OnTeleportClick() {
        Debug.Log("Teleport triggered");
        if (teleportTag == "Overworld 2")
        {
            DataCarrier.Instance.SetBiomeNum(2);
            LevelManager.Instance.ToOverworld();
        }
        else if (teleportTag == "Overworld 3")
        {
            DataCarrier.Instance.SetBiomeNum(3);
            LevelManager.Instance.ToOverworld();
        }
        else
        {
            DataCarrier.Instance.UpdateTag(teleportTag);
            LevelManager.Instance.ToCharacterSelect();
        }
    }
}
