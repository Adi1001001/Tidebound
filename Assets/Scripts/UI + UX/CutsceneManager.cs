using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }
    public TextMeshProUGUI dialogueText; 
    public TextMeshProUGUI skipText;
    private float charactersPerSecond = 20f;
    private Frame[] activeFrames;
    private Cutscenes activeTriggerSource;
    private int currentFrameIndex = 0;
    
    private Coroutine typingCoroutine;
    private Coroutine cameraCoroutine;
    private bool isTyping = false;
    private bool isCameraMoving = false; 
    private bool isCutsceneRunning = false;

    private Camera mainCamera;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Camera.main != null)
        {
            mainCamera = Camera.main;
        }
    } 

    void Update()
    {
        if (!isCutsceneRunning) {return;}

        if (isCameraMoving) {return;}

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleInput();
        }
    }

    public void StartNewCutscene(Frame[] newFrames, Cutscenes sourceScript)
    {
        if (newFrames == null || newFrames.Length == 0) {return;}

        activeFrames = newFrames;
        activeTriggerSource = sourceScript;
        currentFrameIndex = 0;
        isCutsceneRunning = true;
        ShowFrame();
    }

    void ShowFrame()
    {
        if (currentFrameIndex >= activeFrames.Length)
        {
            FinishCutscene();
            return;
        }

        if (typingCoroutine != null) {StopCoroutine(typingCoroutine);}
        if (cameraCoroutine != null) {StopCoroutine(cameraCoroutine);}

        Frame currentFrame = activeFrames[currentFrameIndex];

        if (currentFrame.cameraCoords != Vector2.zero && currentFrame.cameraMoveTime > 0)
        {
            mainCamera.GetComponent<CameraController>().stickToPlayer = false;
            cameraCoroutine = StartCoroutine(MoveCamera(currentFrame));
        }
        else
        {
            mainCamera.GetComponent<CameraController>().stickToPlayer = true;
            isCameraMoving = false; 
            if (skipText != null) skipText.text = "Left click to continue >>>";
        }
        typingCoroutine = StartCoroutine(TypeText(currentFrame.text));
    }

    IEnumerator MoveCamera(Frame frame)
    {
        isCameraMoving = true;
        skipText.text = "Can't skip right now!";
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = new Vector3(frame.cameraCoords.x, frame.cameraCoords.y, startPosition.z);
        
        float elapsedTime = 0f;
        float duration = frame.cameraMoveTime;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        isCameraMoving = false;
        skipText.text = "Left click to continue >>>";
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";
        
        float timeBetweenChars = 1f / charactersPerSecond; 

        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timeBetweenChars);
        }

        isTyping = false;
    }

    void HandleInput()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = activeFrames[currentFrameIndex].text;
            isTyping = false;
        }
        else
        {
            currentFrameIndex++;
            ShowFrame();
        }
    }

    void FinishCutscene()
    {
        isCutsceneRunning = false;
        if (skipText != null) skipText.text = "";
        if (activeTriggerSource != null)
        {
            activeTriggerSource.OnCutsceneFinish();
        }
    }
}
