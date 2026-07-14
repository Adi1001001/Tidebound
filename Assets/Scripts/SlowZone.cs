using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float speedLimit;

    [Range(0.75f, 1f)]
    public float accelerationFactor = 1f;

    public bool breakableByShark;
    public bool active = true;


    [Header("Movement")]
    public bool movable = false;
    public float moveSpeed = 2f;

    [Tooltip("Offsets from starting position.")]
    public List<Vector2> waypoints = new List<Vector2>()
    {
        Vector2.zero
    };


    // Movement
    private int currentTarget = 0;
    private Vector2 startPosition;


    // Path Line
    private LineRenderer pathRenderer;

    private static readonly Color PATH_COLOR =
        new Color32(37, 0, 255, 255);


    // Moving indicator
    private GameObject movementIndicator;
    public Sprite circleSprite;

    public float indicatorSize = 0.02f;

    private static readonly Color INDICATOR_COLOR =
        Color.red;


    void Start()
    {
        startPosition = transform.position;

        RebuildCollider();

        CreatePathLine();

        CreateMovementIndicator();
    }



    void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameplayFrozen()) {return;}

        if (movable && waypoints.Count > 1)
        {
            Vector2 target =
                startPosition + waypoints[currentTarget];


            transform.position =
                Vector2.MoveTowards(
                    transform.position,
                    target,
                    moveSpeed * Time.deltaTime
                );


            if (Vector2.Distance(transform.position, target) < 0.02f)
            {
                currentTarget = (currentTarget + 1) % waypoints.Count;
            }
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
                List<Vector2> points =
                    new List<Vector2>();


                sr.sprite.GetPhysicsShape(i, points);

                poly.SetPath(i, points);
            }
        }
    }



    void CreatePathLine()
    {
        if (waypoints.Count < 2)
            return;


        pathRenderer =
            GetComponent<LineRenderer>();


        if (pathRenderer == null)
        {
            pathRenderer =
                gameObject.AddComponent<LineRenderer>();
        }


        pathRenderer.useWorldSpace = true;


        pathRenderer.startWidth = 0.05f;
        pathRenderer.endWidth = 0.05f;


        pathRenderer.material =
            new Material(
                Shader.Find("Sprites/Default")
            );


        pathRenderer.startColor = PATH_COLOR;
        pathRenderer.endColor = PATH_COLOR;


        List<Vector3> points =
            new List<Vector3>();


        for (int i = 0; i < waypoints.Count; i++)
        {
            points.Add(
                startPosition + waypoints[i]
            );
        }


        // Return to first waypoint
        points.Add(points[0]);


        pathRenderer.positionCount =
            points.Count;


        pathRenderer.SetPositions(
            points.ToArray()
        );


        pathRenderer.sortingOrder = 1;
    }



    void CreateMovementIndicator()
    {
        if (!movable || waypoints.Count < 2)
            return;


        movementIndicator =
            new GameObject("Movement Indicator");


        movementIndicator.transform.parent = transform;


        movementIndicator.transform.localPosition =
            Vector2.zero;


        SpriteRenderer sr =
            movementIndicator.AddComponent<SpriteRenderer>();


        sr.sprite = circleSprite;


        sr.color =
            INDICATOR_COLOR;


        sr.sortingOrder = 2;


        movementIndicator.transform.localScale =
            Vector2.one * indicatorSize;
    }

    void UpdateMovementIndicator()
    {
        if (movementIndicator == null)
            return;


        movementIndicator.transform.position =
            transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;


        PlayerController player =
            collision.GetComponent<PlayerController>();


        AbilityManager ability =
            collision.GetComponent<AbilityManager>();


        if (!active)
            return;


        if (ability != null && ability.sharkOn)
        {
            if (breakableByShark)
                Destroy(gameObject);

            return;
        }


        if (player != null)
        {
            player.EnterSlowZone(
                speedLimit,
                accelerationFactor
            );
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;


        PlayerController player =
            collision.GetComponent<PlayerController>();


        if (player != null)
        {
            player.ExitSlowZone();
        }
    }



    private void OnDestroy()
    {
        if (movementIndicator != null)
            Destroy(movementIndicator);
    }
}