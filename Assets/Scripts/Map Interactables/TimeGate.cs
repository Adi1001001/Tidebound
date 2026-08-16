using UnityEngine;

public class TimeGate : MonoBehaviour
{
    public int extraTime = 5;
    private bool claimedTime = false;
    private GameObject checkpointNum;
    TimerManager timerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerManager = FindAnyObjectByType<TimerManager>();
        checkpointNum = transform.Find("Checkpoint Num").gameObject;
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
            GetComponent<SpriteRenderer>().color = Color.grey;
            checkpointNum.GetComponent<SpriteRenderer>().color = Color.white;
            claimedTime = true;
        }
    }
}
