using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    private TimerManager timerManager;
    private MaxSpeedManager maxSpeedManager;
    private PlayerController playerController;
    public GameObject RaceEndUI;
    public GameObject RaceUI;
    public TMP_Text elapsedTimeText;
    public TMP_Text requiredTimeText;
    public TMP_Text timeDifferenceText;
    public TMP_Text topSpeedText;
    public TMP_Text bestTimeText;
    public TMP_Text successRaceResultText;
    public TMP_Text failedRaceResultText;
    
    void Start() {
        timerManager = FindAnyObjectByType<TimerManager>();
        maxSpeedManager = FindAnyObjectByType<MaxSpeedManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        StartRace();
    }

    public void StartRace() { // add the feature to restart a race by pressing a button laters
        Debug.Log("STARTING RACE");
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Countdown);
        timerManager.StartCountdown();
    }
    public void FinishRace(int raceID) { // also add the best time feature later when you have the saves ready
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.GameOver);
        float elapsedTime = timerManager.GetTimerValues().Item1;
        float requiredTime = timerManager.GetTimerValues().Item2;
        float topSpeed = maxSpeedManager.GetCurrentMaxSpeed();
        topSpeed *= playerController.speedMultiplier;
        timerManager.StopRaceTimer();

        RaceEndUI.SetActive(true);
        RaceUI.SetActive(false);

        if (elapsedTime <= requiredTime) {
            successRaceResultText.gameObject.SetActive(true);
            failedRaceResultText.gameObject.SetActive(false);
            DataCarrier.Instance.UnlockProgress(raceID);
        } else {
            successRaceResultText.gameObject.SetActive(false);
            failedRaceResultText.gameObject.SetActive(true);
        }

        elapsedTimeText.text = elapsedTime.ToString();
        requiredTimeText.text = requiredTime.ToString();
        timeDifferenceText.text = (elapsedTime - requiredTime).ToString();
        topSpeedText.text = topSpeed.ToString();
    }
}
