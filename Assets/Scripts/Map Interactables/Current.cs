using UnityEngine;

public class Current : MonoBehaviour {
    
    public float pushForce = 10f;
    private Rigidbody2D playerRb;
    private CameraController cameraController;
    private PlayerController playerController;
    private AudioSource sfxManager;
    public AudioClip currentSFX;
    public AudioClip reversedSFX;

    void Start()
    {
        sfxManager = GameObject.Find("SFX Managers/Current").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            cameraController = FindAnyObjectByType<CameraController>();
            playerController.inCurrent = true;
            if (GameStateManager.Instance.GetPlayerState() != GameStateManager.PlayerStates.Lilypad)
            {
                if (Vector2.Dot(playerRb.linearVelocity, transform.up) > 0)
                {
                    sfxManager.clip = currentSFX;
                }
                else
                {
                    sfxManager.clip = reversedSFX;
                }
                sfxManager.Play();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (playerRb != null && GameStateManager.Instance.GetPlayerState() != GameStateManager.PlayerStates.Lilypad) {
                playerRb.AddForce(transform.up * pushForce); 
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
