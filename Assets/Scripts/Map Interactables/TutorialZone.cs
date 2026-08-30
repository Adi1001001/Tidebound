using UnityEngine;
using UnityEngine.UI;

public class TutorialZone : MonoBehaviour
{
    public Sprite topSprite;
    public Sprite bottomSprite;
    public float timeScale = 1f;
    public int tutorialID;

    private bool hasEntered = false;
    private bool hasExited = false;

    private Image tutorialTop;
    private Image tutorialBottom;

    void Start()
    {
        GameObject topObj = GameObject.Find("Tutorial Top");
        if (topObj != null)
        {
            tutorialTop = topObj.GetComponent<Image>();
        }

        GameObject bottomObj = GameObject.Find("Tutorial Bottom");
        if (bottomObj != null)
        {
            tutorialBottom = bottomObj.GetComponent<Image>();
        }

        HideTutorial();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TryStartTutorial();
        }
    }

    void TryStartTutorial()
    {
        if (GameStateManager.Instance.GetGameState() == GameStateManager.GameStates.NPC)
        {
            HideTutorial();
            return;
        }

        if (hasEntered || hasExited)
        {
            return;
        }

        if (DataCarrier.Instance.GetTutorialID() > tutorialID)
        {
            return;
        }

        ShowTutorial();
    }

    void ShowTutorial()
    {
        hasEntered = true;
        DataCarrier.Instance.SetTutorialID(tutorialID);

        if (topSprite != null && tutorialTop != null)
        {
            tutorialTop.sprite = topSprite;
            SetAlpha(tutorialTop, 1f);
        }
        else if (tutorialTop != null)
        {
            SetAlpha(tutorialTop, 0f);
        }

        if (bottomSprite != null && tutorialBottom != null)
        {
            tutorialBottom.sprite = bottomSprite;
            SetAlpha(tutorialBottom, 1f);
        }
        else if (tutorialBottom != null)
        {
            SetAlpha(tutorialBottom, 0f);
        }

        Time.timeScale = timeScale;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (!hasEntered || hasExited)
        {
            return;
        }

        hasEntered = false;
        hasExited = true;

        HideTutorial();
    }

    void HideTutorial()
    {
        if (tutorialTop != null)
        {
            SetAlpha(tutorialTop, 0f);
        }

        if (tutorialBottom != null)
        {
            SetAlpha(tutorialBottom, 0f);
        }

        Time.timeScale = 1f;
    }

    void SetAlpha(Image image, float alpha)
    {
        if (image == null)
        {
            return;
        }

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    void OnDisable()
    {
        HideTutorial();
    }
}