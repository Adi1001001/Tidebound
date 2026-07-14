using UnityEngine;

public class BouncyArea : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(":)");
            collision.GetComponent<PlayerStateManager>().SetNearbyBouncyArea(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(":(");
            collision.GetComponent<PlayerStateManager>().SetNearbyBouncyArea(null);
        }
    }
}
