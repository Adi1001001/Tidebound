using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float speedLimit;

    [Range(0.75f, 1f)]
    public float accelerationFactor = 1f;

    [Header("Movement")]
    public bool movable = false;
    public float moveSpeed = 2f;
    [Tooltip("Offsets from starting position.")]
    public List<Vector2> waypoints = new List<Vector2>() { Vector2.zero };

    private int currentTarget = 0;
    private Vector2 startPosition;
    private LineRenderer pathRenderer;

    private static readonly Color PATH_COLOR = new Color32(90, 70, 45, 255);

    void Start()
    {
        startPosition = transform.position;
        CreatePathLine();
    }

    void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameplayFrozen()) return;

        if (movable && waypoints.Count > 1)
        {
            Vector2 target = startPosition + waypoints[currentTarget];
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 0.02f)
                currentTarget = (currentTarget + 1) % waypoints.Count;
        }
    }

    void CreatePathLine()
    {
        if (waypoints.Count < 2) return;

        pathRenderer = GetComponent<LineRenderer>();

        if (pathRenderer == null)
            pathRenderer = gameObject.AddComponent<LineRenderer>();

        pathRenderer.useWorldSpace = true;
        pathRenderer.startWidth = 0.05f;
        pathRenderer.endWidth = 0.05f;
        pathRenderer.material = new Material(Shader.Find("Sprites/Default"));
        pathRenderer.startColor = PATH_COLOR;
        pathRenderer.endColor = PATH_COLOR;
        pathRenderer.sortingOrder = 0;

        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < waypoints.Count; i++)
            points.Add(startPosition + waypoints[i]);

        points.Add(points[0]);
        pathRenderer.positionCount = points.Count;
        pathRenderer.SetPositions(points.ToArray());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
            player.EnterSlowZone(speedLimit, accelerationFactor);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
            player.ExitSlowZone();
    }
}