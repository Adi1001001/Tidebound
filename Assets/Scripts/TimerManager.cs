using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour {
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI countdownDisplay;
    public float timeLimit = 30f;
    private PlayerController playerController;
    private Ability ability;
    private float elapsedTime = 0f;
    [HideInInspector] public bool isTimerRunning = false;
    int countdown = 3;

    void Start() {
        playerController = FindAnyObjectByType<PlayerController>();
        ability = playerController.GetComponent<Ability>();
    }

    void Update() {
        if (isTimerRunning) {
            elapsedTime += Time.deltaTime;
            // if (ability.onAbility && DataCarrier.Instance.GetCharacter() == Character.Swordfish) {
            //     elapsedTime += Time.deltaTime * ability.swordfishAbilitySlowFactor;
            // } else {
            //     elapsedTime += Time.deltaTime;
            // }

            if (elapsedTime >= timeLimit) {
                isTimerRunning = false;
                timerDisplay.color = Color.red;
                timerDisplay.text = "Out of time! Run won't count towards completion.";
            } else {
                DisplayTime(timeLimit - elapsedTime);
            }
        }
    }

    void DisplayTime(float timeToDisplay) {
        timeToDisplay = Mathf.Max(0f, timeToDisplay);
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        int tenths = Mathf.FloorToInt(timeToDisplay % 1 * 10);

        float elapsed = elapsedTime / timeLimit;
        if (ability.onAbility && DataCarrier.Instance.GetCharacter() == Character.Swordfish) {
            timerDisplay.color = Color.white;
        } else if (elapsed >= 0.9f) {
            timerDisplay.color = Color.red;
        } else if (elapsed >= 0.75f) {
            timerDisplay.color = Color.yellow;
        } else {
            timerDisplay.color = Color.green;
        }

        if (minutes > 0) {
            timerDisplay.text = string.Format("{0}:{1:00}.{2}", minutes, seconds, tenths);
        } else {
            timerDisplay.text = string.Format("{0}.{1}", seconds, tenths);
        }
    }

    public void StartCountdown() {
        if (GameStateManager.Instance.GetGameState() != GameStateManager.GameStates.Countdown) {
            Debug.Log("Cannot show countdown timer when not in countdown state");
            return;
        }
        StartCoroutine(CountdownCoroutine());
    }

    public void StartRaceTimer() {
        timerDisplay.gameObject.SetActive(true);
        isTimerRunning = true;
    }

    public (float, float) GetTimerValues() {
        return (elapsedTime, timeLimit);
    }

    public void StopRaceTimer() {
        isTimerRunning = false;
        elapsedTime = 0f;
        timerDisplay.gameObject.SetActive(false);
    }

    IEnumerator CountdownCoroutine() {
        countdownDisplay.gameObject.SetActive(true);
        while (countdown > 0) {
            countdownDisplay.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countdownDisplay.text = "Go!";
        yield return new WaitForSeconds(1f);
        countdownDisplay.text = "";
        countdownDisplay.gameObject.SetActive(false);
        countdown = 3;

        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Racing);
        StartRaceTimer();
    }
}