using UnityEngine;

public class VineGate : MonoBehaviour
{
    public int requiredProgress;
    void Start()
    {
        CheckGate();
    }

    public void CheckGate()
    {
        if (DataCarrier.Instance.GetProgress() >= requiredProgress)
        {
            Destroy(gameObject);
        }
    }
}