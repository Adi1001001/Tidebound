using UnityEngine;
using System.Collections; 
using UnityEngine.UI;    
using TMPro;             

public class NewDiscovery : MonoBehaviour
{
    public int discoveryID;
    public string newHeaderStr;
    public string newBodyStr;
    public Color newBodyColor;
    public string description;
    private GameObject UIPanel;

    void Start()
    {
        UIPanel = FindAnyObjectByType<UIConnector>().racePanel;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DataCarrier.Instance.GetDiscoveryID() >= discoveryID) { return; }

        DataCarrier.Instance.SetDiscoveryID(discoveryID);
        StartCoroutine(PlayUISequence());
    }

    IEnumerator PlayUISequence()
    {
        GameObject speedText = UIPanel.transform.Find("Speed Text").gameObject;
        GameObject speedBar = UIPanel.transform.Find("Speed Bar").gameObject;
        
        TMP_Text headerText = UIPanel.transform.Find("New Header").GetComponent<TMP_Text>();
        TMP_Text bodyText = UIPanel.transform.Find("New Body").GetComponent<TMP_Text>();
        TMP_Text descText = UIPanel.transform.Find("New Description").GetComponent<TMP_Text>();

        TMP_Text sTextComp = speedText.GetComponent<TMP_Text>();
        Image sBarImage = speedBar.GetComponent<Image>();

        speedText.SetActive(false);
        speedBar.SetActive(false);

        headerText.text = newHeaderStr;
        bodyText.text = newBodyStr;
        bodyText.color = newBodyColor; 
        descText.text = description;

        TMP_Text[] discoveryTexts = new TMP_Text[] { headerText, bodyText, descText };

        headerText.gameObject.SetActive(true);
        bodyText.gameObject.SetActive(true);
        descText.gameObject.SetActive(true);

        yield return StartCoroutine(FadeTexts(discoveryTexts, 0.5f, true));

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(FadeTexts(discoveryTexts, 0.5f, false));

        headerText.gameObject.SetActive(false);
        bodyText.gameObject.SetActive(false);
        descText.gameObject.SetActive(false);

        speedText.SetActive(true);
        speedBar.SetActive(true);

        StartCoroutine(FadeTexts(new TMP_Text[] { sTextComp }, 0.5f, true));
        yield return StartCoroutine(FadeImages(new Image[] { sBarImage }, 0.5f, true));
    }


    IEnumerator FadeTexts(TMP_Text[] texts, float duration, bool fadeIn)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            float targetAlpha = fadeIn ? progress : 1f - progress;

            foreach (TMP_Text text in texts)
            {
                if (text != null)
                {
                    Color textColor = text.color;
                    textColor.a = targetAlpha;
                    text.color = textColor;

                    Color outlineColor = text.outlineColor;
                    outlineColor.a = targetAlpha;
                    text.outlineColor = outlineColor;
                }
            }
            yield return null;
        }
    }
    IEnumerator FadeImages(Image[] images, float duration, bool fadeIn)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            float targetAlpha = fadeIn ? progress : 1f - progress;

            foreach (Image image in images)
            {
                if (image != null)
                {
                    Color c = image.color;
                    c.a = targetAlpha;
                    image.color = c;
                }
            }
            yield return null;
        }
    }
}
