using UnityEngine;

public class UIConnector : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject racePanel;
    public GameObject volumePanel;
    public GameObject controlsPanel;

    void Start() {
        LevelManager.Instance.UpdateUIReferences(pausePanel, racePanel, volumePanel, controlsPanel);
    }
}

