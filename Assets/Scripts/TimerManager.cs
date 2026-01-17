// in order to activate the timer, when the game enters a racing level it will call the countdown and then the timer will auto start (make sure to change states once countdown timer is done)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour {
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI countdownDisplay;
    Dictionary<string, float> timeValues = new Dictionary<string, float>
    {{"Race1", 20.0f}, {"Race2", 24.5f}, {"Race3", 35.5f}, {"Race4", 20.5f}, {"Race5", 40.0f}};
    int countdown = 3;

    void Update() {
        if (GameStateManager.Instance.CheckGameState() != GameStateManager.GameStates.Racing) {
            // Debug.Log("Cannot show timer when not in racing state");
            return;
        }
    }
    public void StartCountdown() {
        if (GameStateManager.Instance.CheckGameState() != GameStateManager.GameStates.Countdown) {
            Debug.Log("Cannot show countdown timer when not in countdown state");
            return;
        }
        StartCoroutine(CountdownCoroutine());
    }
    public void StartRaceTimer() {
        timerDisplay.gameObject.SetActive(true);
        string currentRace = SceneManager.GetActiveScene().name.ToString();
        float raceTime = timeValues[currentRace];
    }
    System.Collections.IEnumerator CountdownCoroutine() {
        countdownDisplay.gameObject.SetActive(true);
        while (countdown > 0) {
            countdownDisplay.text = countdown.ToString(); // displaying the countdown number
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countdownDisplay.text = "Go!";
        yield return new WaitForSeconds(1f);
        countdownDisplay.text = "";
        countdownDisplay.gameObject.SetActive(false); // getting rid of the countdown text afterwards
        countdown = 3;

        // Start the timer here
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Racing);
        StartRaceTimer();
    }
}