// in order to activate the timer, when the game enters a racing level it will call the countdown and then the timer will auto start (make sure to change states once countdown timer is done)
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour {
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI countdownDisplay;
    private float elapsedTime = 0f;
    [HideInInspector] public bool isTimerRunning = false;
    Dictionary<string, float> timeValues = new Dictionary<string, float>
    {{"SampleRaceScene", 10.0f}, {"Race1", 20.0f}, {"Race2", 24.5f}, {"Race3", 35.5f}, {"Race4", 20.5f}, {"Race5", 40.0f}};
    int countdown = 3;
    private float raceTime;

    void Update() {
        if (isTimerRunning) {
            Debug.Log("Timer Running");
            elapsedTime += Time.deltaTime;
            DisplayTime(elapsedTime);
        }
    }
    void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = timeToDisplay % 1 * 100;

        if (elapsedTime <= raceTime) {
            timerDisplay.color = Color.green;
        } else {
            timerDisplay.color = Color.red;
        }
        timerDisplay.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds); // "00:00:00" format
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
        raceTime = timeValues[currentRace];
        elapsedTime = 0f;
        isTimerRunning = true;
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