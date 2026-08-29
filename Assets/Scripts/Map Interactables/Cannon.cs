using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Speed")]
    public float releaseSpeed = 50f;
    [Header("Rotation")]
    public float minRotation = 0f;
    public float maxRotation = 10f;

    [HideInInspector] public bool rotating = false;
    private bool movingTowardsMax = true;

    private float rotationProgress = 0f;
    public float rotationDuration = 1.25f;

    private float currentAngle;

    private PlayerController player;
    [HideInInspector] public bool playerInCannon = false;

    private CameraController mainCamera;
    public float cameraZoomFactor = 1.5f;
    private AudioSource sfxManager;
    void Start()
    {
        currentAngle = minRotation;
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        sfxManager = GameObject.Find("SFX Managers/Cannon").GetComponent<AudioSource>();
    }


    void Update()
    {
        if (!rotating) {return;}

        RotateCannon();
    }

    private void RotateCannon()
    {
        float startAngle;
        float targetAngle;

        if (movingTowardsMax)
        {
            startAngle = minRotation;
            targetAngle = maxRotation;
        }
        else
        {
            startAngle = maxRotation;
            targetAngle = minRotation;
        }

        rotationProgress += Time.deltaTime / rotationDuration;
        float easedProgress = Mathf.SmoothStep(0f, 1f, rotationProgress);


        currentAngle = Mathf.Lerp(startAngle, targetAngle, easedProgress);

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        player.transform.rotation = transform.rotation;

        if (rotationProgress >= 1f)
        {
            rotationProgress = 0f;
            movingTowardsMax = !movingTowardsMax;
        }
    }


    public void ToggleCannon()
    {
        if (GameStateManager.Instance.IsGameplayFrozen()) {return;}

        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
            if (player == null) {return;}
        }

        playerInCannon = !playerInCannon;
        rotating = playerInCannon;

        if (playerInCannon)
        {
            player.EnterCannon(transform.position);
            player.SetVisible(false);
            mainCamera.ZoomCamera(cameraZoomFactor);
        }
        else
        {
            player.FireFromCannon(releaseSpeed, transform.up);
            player.SetVisible(true);
            mainCamera.ZoomCamera(1/cameraZoomFactor);
            sfxManager.Play();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerActionManager>().SetNearbyCannon(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerActionManager>().SetNearbyCannon(null);
        }
    }

}