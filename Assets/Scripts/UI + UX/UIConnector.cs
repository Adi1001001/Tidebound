using UnityEngine;

public class UIConnector : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject racePanel;
    public GameObject volumePanel;
    public GameObject controlsPanel;

    void Awake()
    {
        if (LevelManager.Instance == null)
        {
            GameObject obj = new GameObject("LevelManager");
            obj.AddComponent<LevelManager>();
        }

        LevelManager.Instance.UpdateUIReferences(pausePanel, racePanel, volumePanel, controlsPanel);
    }
}

