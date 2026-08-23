using UnityEngine;
using TMPro;
using System;

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
    public int raceID;
    
    void Start() {
        timerManager = FindAnyObjectByType<TimerManager>();
        maxSpeedManager = FindAnyObjectByType<MaxSpeedManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        StartRace();
    }

    public void StartRace() { 
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Countdown);
        timerManager.StartCountdown();
    }
    public void FinishRace() { 
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.GameOver);
        float elapsedTime = (float)Math.Round(timerManager.GetTimerValues().Item1, 1);
        float requiredTime = timerManager.GetTimerValues().Item2;
        float topSpeed = (float)Math.Round(maxSpeedManager.GetCurrentMaxSpeed());
        topSpeed *= playerController.speedMultiplier;
        timerManager.StopRaceTimer();

        RaceEndUI.SetActive(true);
        RaceUI.SetActive(false);

        if (!timerManager.failed) { // Can fail due to not reaching time gates fast enough
            successRaceResultText.gameObject.SetActive(true);
            failedRaceResultText.gameObject.SetActive(false);
            DataCarrier.Instance.UnlockProgress(raceID);
            DataCarrier.Instance.SetBestTime(raceID, elapsedTime);
        } else {
            successRaceResultText.gameObject.SetActive(false);
            failedRaceResultText.gameObject.SetActive(true);
        }

        if (elapsedTime <= requiredTime && timerManager.failed)
        {
            elapsedTimeText.text = elapsedTime.ToString()+" (Failed at time gate)";
        }
        else
        {
            elapsedTimeText.text = elapsedTime.ToString();
        }
        requiredTimeText.text = requiredTime.ToString();
        timeDifferenceText.text = Math.Round(Math.Abs(elapsedTime - requiredTime), 1).ToString();
        topSpeedText.text = topSpeed.ToString();

        float bestTime = DataCarrier.Instance.GetBestTime(raceID);
        if (bestTime == 0.0f)
        {
            bestTimeText.text = "No best time.";
        }
        else
        {
            bestTimeText.text = bestTime.ToString();
            if (elapsedTime == bestTime)
            {
                bestTimeText.text += " (NEW BEST!)";
            }
        }
    }
}
