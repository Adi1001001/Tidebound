using UnityEngine;

[System.Serializable]
public struct Frame
{
    public string text;
    public Vector2 cameraCoords; // Let (0, 0) = No camera movement required
    public int cameraMoveTime;
}

public class Cutscenes : MonoBehaviour
{
    public int cutsceneID;
    public Frame[] frames;
    private GameObject cutsceneTextbox;
    private GameObject speedBar;
    private GameObject speedText;
    private GameObject interactButton;

    void Start()
    {
        GameObject raceUI = GameObject.Find("Canvas").transform.Find("RaceUI").gameObject;
        cutsceneTextbox = raceUI.transform.Find("Cutscene Textbox").gameObject;
        speedBar = raceUI.transform.Find("Speed Bar").gameObject;
        speedText = raceUI.transform.Find("Speed Text").gameObject;
        interactButton = raceUI.transform.Find("Interact Button").gameObject;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (DataCarrier.Instance.GetCutsceneID() >= cutsceneID) return;

        DataCarrier.Instance.SetCutsceneID(cutsceneID); 
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.NPC);
        ChangeToCutscene(true);
        CutsceneManager.Instance.StartNewCutscene(frames, this);
    } 

    private void ChangeToCutscene(bool starting)
    {
        speedBar.SetActive(!starting);
        speedText.SetActive(!starting);
        interactButton.SetActive(!starting);
        cutsceneTextbox.SetActive(starting);
    }

    public void OnCutsceneFinish()
    {
        ChangeToCutscene(false);
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
    }
}
