using UnityEngine;

public class Sponge : MonoBehaviour
{
    [System.Serializable]
    public class SpongeSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }
    public float pushForce = 10f;
    public float airTime = 0.75f;
    PlayerController playerController;
    PlayerAppearance appearance;
    private bool spongeActivated = false;
    public bool initialised = false;
    [SerializeField] private SpongeSprite[] sprites;
        
    void Start() {
        SetSprite();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        appearance = GameObject.FindWithTag("Player").GetComponent<PlayerAppearance>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        CircleCollider2D cc = GetComponent<CircleCollider2D>();

        Bounds bounds = sr.sprite.bounds; // Local-space bounds of the sprite

        // Radius that encloses the entire sprite
        cc.radius = Mathf.Max(bounds.extents.x, bounds.extents.y);

        // Center the collider on the sprite
        cc.offset = bounds.center;
    }

    private void SetSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int currentBiome = DataCarrier.Instance.GetBiomeNum();
        foreach (SpongeSprite spongeSprite in sprites)
        {
            if (spongeSprite.biomeNum == currentBiome)
            {
                sr.sprite = spongeSprite.sprite;
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        TryBeginningAirTime(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        TryBeginningAirTime(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!initialised) {return;}
        spongeActivated = false;
    }

    private void TryBeginningAirTime(Collider2D collision)
    {
        if (GameStateManager.Instance.GetPlayerState() == GameStateManager.PlayerStates.Sponge || spongeActivated) {return;}
        spongeActivated = true;
        BeginAirTime(collision);
    }
    private void BeginAirTime(Collider2D collision)
    {
        GameStateManager.Instance.SetPlayerState(GameStateManager.PlayerStates.Sponge);
        Rigidbody2D rb = collision.attachedRigidbody;
        Vector2 launchDirection = rb.linearVelocity.normalized;
        rb.angularVelocity = 0f;
        rb.AddForce(launchDirection * pushForce, ForceMode2D.Impulse);

        playerController.StartAirTime(airTime);
        appearance.StartAirborneEffect(airTime, launchDirection, pushForce);
    }
}
