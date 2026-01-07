// in order to activate the timer, when the game enters a racing level it will call the countdown and then the timer will auto start (make sure to change states once countdown timer is done)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour {
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI countdownDisplay;
    List<float> timer = new List<float> {20.0f, 24.5f, 35.5f, 20.5f, 40.0f};
    int countdown = 3;
    private GameStateManager.GameStates currentGameState;

    void Update() {
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState != GameStateManager.GameStates.Racing) {
            Debug.Log("Cannot show timer when not in racing state");
            return;
        }
    }
    public void StartCountdown() {
        currentGameState = GameStateManager.Instance.CheckGameState();
        if (currentGameState != GameStateManager.GameStates.Countdown) {
            Debug.Log("Cannot show countdown timer when not in countdown state");
            return;
        }
        StartCoroutine(CountdownCoroutine());
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
        // Start the timer here
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Racing);
    }
}