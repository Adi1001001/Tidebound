using UnityEngine;
using System.Collections.Generic;

public class BoundaryGeneration : MonoBehaviour {
    public BoundaryManager boundaries;
    public int nodesOnScreen = 20; // How many "steps" ahead we calculate
    public float stepDistance = 2f; // Vertical distance between each node
    
    public Queue<float> centreQueue = new Queue<float>(); // using a queue because the FIFO principle is effective here
    public Queue<float> widthQueue = new Queue<float>();

    void Start() {
        // Filling the initial river with a straight path
        for (int i = 0; i < nodesOnScreen; i++) {
            centreQueue.Enqueue(boundaries.currentCentre);
            widthQueue.Enqueue(boundaries.currentWidth);
        }
    }

    // Call this whenever the player moves "stepDistance" forward
    public void AdvanceRiver(float targetCentre, float targetWidth) {
        // 1. Remove the point the player just passed
        centreQueue.Dequeue();
        widthQueue.Dequeue();
        
        centreQueue.Enqueue(targetCentre);
        widthQueue.Enqueue(targetWidth);
        
        // UpdateVisuals(); // got to implement this to update the river visuals
    }

    float GetLastElement(Queue<float> q) {
        float[] array = q.ToArray();
        return array[array.Length - 1];
    }
}