using UnityEngine;

public class BoundaryManager : MonoBehaviour {
    public float currentCentre = 0f;
    public float currentWidth = 20f;
    public float leftEdge;
    public float rightEdge;

    void CalculateEdges() {
        leftEdge = currentCentre - (currentWidth / 2);
        rightEdge = currentCentre + (currentWidth / 2);
    }

    void Update() {
        CalculateEdges();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        // Drawing the left edge
        Vector3 left = new Vector3(leftEdge, 0, 0);
        Gizmos.DrawRay(left + Vector3.up * 10, Vector3.down * 20);

        // Drawing the right edge
        Vector3 right = new Vector3(rightEdge, 0, 0);
        Gizmos.DrawRay(right + Vector3.up * 10, Vector3.down * 20);
    }
}