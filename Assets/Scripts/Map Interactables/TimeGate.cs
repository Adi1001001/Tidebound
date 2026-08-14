using UnityEngine;

public class TimeGate : MonoBehaviour
{
    public int extraTime = 5;
    private bool claimedTime = false;
    TimerManager timerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerManager = FindAnyObjectByType<TimerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!claimedTime)
        {
            timerManager.AddTime(extraTime);
            claimedTime = true;
        }
    }
}
