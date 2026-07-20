using UnityEngine;

public class Current : MonoBehaviour {
    
    [System.Serializable]
    public class CurrentSprite
    {
        public int biomeNum;
        public Sprite sprite;
    }
    public float pushForce = 10f;
    private Rigidbody2D playerRb;
    private CameraController cameraController;
    private PlayerController playerController;
    [SerializeField] private CurrentSprite[] sprites;

    void Start()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int currentBiome = DataCarrier.Instance.GetBiomeNum();
        foreach (CurrentSprite currentSprite in sprites)
        {
            if (currentSprite.biomeNum == currentBiome)
            {
                sr.sprite = currentSprite.sprite;
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            cameraController = FindAnyObjectByType<CameraController>();
            
            playerController.inCurrent = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (playerRb != null && GameStateManager.Instance.GetPlayerState() != GameStateManager.PlayerStates.Sponge) {
                playerRb.AddForce(transform.up * pushForce); // you have to adjust the direction of the current in unity
                cameraController.CameraShake();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerController.inCurrent = false;
            playerRb = null;
            cameraController = null;
            playerController = null;
        }
    }
}
