using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Speed")]
    public float releaseSpeed = 50f;
    [Header("Rotation")]
    public float minRotation = 0f;
    public float maxRotation = 180f;

    private bool rotating = false;
    private bool movingTowardsMax = true;

    private float rotationProgress = 0f;
    public float rotationDuration = 1.25f;

    private float currentAngle;

    private PlayerController player;
    [HideInInspector] public bool playerInCannon = false;

    private CameraController mainCamera;
    public float cameraZoomFactor = 1.5f;

    void Start()
    {
        RebuildCollider();
        SetRandomRotation();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }


    void Update()
    {
        if (!rotating)
            return;

        RotateCannon();
    }


    void SetRandomRotation()
    {
        currentAngle = Random.Range(minRotation, maxRotation);

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                currentAngle
            );

        // Decide which direction to move based on current position
        float distanceToMin = Mathf.Abs(currentAngle - minRotation);
        float distanceToMax = Mathf.Abs(maxRotation - currentAngle);

        // If closer to min, move towards max.
        // If closer to max, move towards min.
        movingTowardsMax = distanceToMin < distanceToMax;
    }


    void RotateCannon()
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


        float easedProgress =
            Mathf.SmoothStep(0f, 1f, rotationProgress);


        currentAngle =
            Mathf.Lerp(
                startAngle,
                targetAngle,
                easedProgress
            );


        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                currentAngle
            );

        player.transform.rotation = transform.rotation;

        if (rotationProgress >= 1f)
        {
            rotationProgress = 0f;
            movingTowardsMax = !movingTowardsMax;
        }
    }


    public void ToggleCannon()
    {
        if (GameStateManager.Instance.IsGameplayFrozen())
        {
            Debug.Log("Cannot use cannon, game not in playing state.");
            return;
        }


        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();

            if (player == null)
            {
                Debug.LogWarning("PlayerController not found.");
                return;
            }
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
        }
    }

    void RebuildCollider()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();

        PolygonCollider2D poly =
            GetComponent<PolygonCollider2D>();

        if (sr != null && poly != null && sr.sprite != null)
        {
            poly.pathCount =
                sr.sprite.GetPhysicsShapeCount();

            for (int i = 0; i < poly.pathCount; i++)
            {
                List<Vector2> points = new();

                sr.sprite.GetPhysicsShape(i, points);

                poly.SetPath(i, points);
            }
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