using UnityEngine;
using TMPro;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI addedTimeDisplay;
    public TextMeshProUGUI countdownDisplay;
    public float initialTimer = 30f;

    private float totalTimer;
    private GameObject player;
    private PlayerController playerController;
    private Ability ability;
    private float elapsedTime = 0f;

    [HideInInspector] public float slowFactor = 1;
    [HideInInspector] public bool isTimerRunning = false;

    int countdown = 3;

    private float addedTimeAnimationTime = 2f;
    private float addedTimeMoveDistance = 50f;

    private Coroutine addedTimeCoroutine;
    private Vector2 addedTimeOriginalPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        ability = playerController.GetComponent<Ability>();
        totalTimer = initialTimer;

        addedTimeOriginalPos = addedTimeDisplay.rectTransform.anchoredPosition;

        ResetAddedTimeDisplay();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime * slowFactor;
        }

        if (elapsedTime >= totalTimer)
        {
            timerDisplay.color = Color.red;
            timerDisplay.text = "Out of time!";
        }
        else
        {
            DisplayTime(totalTimer - elapsedTime);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(0f, timeToDisplay);

        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        int tenths = Mathf.FloorToInt(timeToDisplay % 1 * 10);

        float elapsed = (totalTimer - elapsedTime) / initialTimer;

        if (ability.onAbility && DataCarrier.Instance.GetCharacter() == Character.Swordfish)
        {
            timerDisplay.color = Color.white;
        }
        else if (elapsed <= 0.1f)
        {
            timerDisplay.color = Color.red;
        }
        else if (elapsed <= 0.25f)
        {
            timerDisplay.color = Color.yellow;
        }
        else
        {
            timerDisplay.color = Color.green;
        }

        if (minutes > 0)
        {
            timerDisplay.text = string.Format("{0}:{1:00}.{2}", minutes, seconds, tenths);
        }
        else
        {
            timerDisplay.text = string.Format("{0}.{1}", seconds, tenths);
        }
    }

    public void StartCountdown()
    {
        if (GameStateManager.Instance.GetGameState() != GameStateManager.GameStates.Countdown)
        {
            Debug.Log("Cannot show countdown timer when not in countdown state");
            return;
        }

        StartCoroutine(CountdownCoroutine());
    }

    public void StartRaceTimer()
    {
        timerDisplay.gameObject.SetActive(true);
        isTimerRunning = true;
    }

    public (float, float) GetTimerValues()
    {
        return (elapsedTime, totalTimer);
    }

    public void StopRaceTimer()
    {
        isTimerRunning = false;
        elapsedTime = 0f;
        timerDisplay.gameObject.SetActive(false);
    }

    public void AddTime(int extraTime)
    {
        if (elapsedTime < totalTimer)
        {
            totalTimer += extraTime;

            // Stop the previous animation if one is running
            if (addedTimeCoroutine != null)
            {
                StopCoroutine(addedTimeCoroutine);
                addedTimeCoroutine = null;
            }

            ResetAddedTimeDisplay();
            addedTimeCoroutine = StartCoroutine(ShowAddedTime(extraTime));
        }
        else
        {
            isTimerRunning = false;
        }
    }

    void ResetAddedTimeDisplay()
    {
        RectTransform rectTransform = addedTimeDisplay.rectTransform;

        rectTransform.anchoredPosition = addedTimeOriginalPos;

        Color color = addedTimeDisplay.color;
        color.a = 0f;
        addedTimeDisplay.color = color;

        addedTimeDisplay.gameObject.SetActive(false);
    }

    IEnumerator ShowAddedTime(int timeAdded)
    {
        RectTransform rectTransform = addedTimeDisplay.rectTransform;

        addedTimeDisplay.gameObject.SetActive(true);
        addedTimeDisplay.text = "+" + timeAdded;

        float elapsed = 0f;

        while (elapsed < addedTimeAnimationTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / addedTimeAnimationTime);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector2 finalPos = new Vector2(addedTimeOriginalPos.x+addedTimeMoveDistance, 
                                            addedTimeOriginalPos.y+addedTimeMoveDistance);

            rectTransform.anchoredPosition = Vector2.Lerp(addedTimeOriginalPos, finalPos, smoothT);
            Color color = addedTimeDisplay.color;

            if (t < 0.2f)
            {
                color.a = t / 0.2f;
            }
            else
            {
                color.a = 1f - ((t - 0.2f) / 0.8f);
            }

            addedTimeDisplay.color = color;

            yield return null;
        }

        ResetAddedTimeDisplay();
        addedTimeCoroutine = null;
    }

    IEnumerator CountdownCoroutine()
    {
        countdownDisplay.gameObject.SetActive(true);

        while (countdown > 0)
        {
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