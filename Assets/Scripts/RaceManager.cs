using UnityEngine;

public class FinishRace : MonoBehaviour
{
    private TimerManager timerManager;
    private MaxSpeedManager maxSpeedManager;
    
    void Start() {
        timerManager = FindFirstObjectByType<TimerManager>();
        maxSpeedManager = FindFirstObjectByType<MaxSpeedManager>();
    }

    void StartRace() {
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Countdown);
        timerManager.StartCountdown();
    }
    // void FinishRace()
    // {

    //     topSpeed = maxSpeedManager.GetCurrentMaxSpeed();
    // }
}
