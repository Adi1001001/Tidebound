using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }
    public GameObject pauseMenuUI;
    public GameObject raceUI;
    public GameObject volumeUI;
    public GameObject controlsUI;
    // private RaceManager raceManager;
    void Awake() {
        if (Instance != null && Instance != this) { // making it a singleton
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void UpdateUIReferences(GameObject pausePanel, GameObject racePanel, GameObject volumePanel, GameObject controlsPanel) {
        pauseMenuUI = pausePanel;
        volumeUI = volumePanel;
        controlsUI = controlsPanel;
        raceUI = racePanel;
    }
    // public void NextScene() { // going to the next level.
    //     Debug.Log("NEXT SCENE");
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    // public void PreviousScene() { // going to the previous level.
    //     Debug.Log("PREVIOUS SCENE");
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    // }
    public void QuitGame() { // closing the game
        Debug.Log("END GAME");
        Application.Quit();
    }
    // public void RestartCurrentScene() { // restarting the level. make it resume the level
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }
    public void RestartRace() {
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Racing);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ToOverworld() {
        Debug.Log(GameStateManager.Instance.CheckGameState());
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
        Debug.Log(GameStateManager.Instance.CheckGameState());
        SceneManager.LoadScene("Overworld");
    }
    public void ToCharacterSelect() {
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.CharacterSelect);
        SceneManager.LoadScene("Character Select");
    }
    public void ToMainMenu() { // going back to the main menu without losing all of the save data.
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.MainMenu);
        SceneManager.LoadScene("Main Menu");
    }
    public void ToRaceScene() {
        string raceTag = DataCarrier.Instance.nextRaceTag;
        SceneManager.LoadScene(raceTag);
        // raceManager = FindFirstObjectByType<RaceManager>();
        // raceManager.StartRace();
    }
    public void LoadSceneByName(string sceneName) {
        Debug.Log("LOADING SCENE: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void PauseGame() {
        if (GameStateManager.Instance.CheckGameState() == GameStateManager.GameStates.MainMenu
        || GameStateManager.Instance.CheckGameState() == GameStateManager.GameStates.Countdown) {
            return;
        }
        Time.timeScale = 0f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Paused);
        pauseMenuUI.SetActive(true);
        raceUI.SetActive(false);
    }
    public void ResumeGame() {
        Time.timeScale = 1f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
        pauseMenuUI.SetActive(false);
        if (volumeUI != null) volumeUI.SetActive(false);
        if (controlsUI != null) controlsUI.SetActive(false);
        raceUI.SetActive(true);
    }
    public void OpenVolumeSettings() {
        volumeUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void CloseVolumeSettings() {
        volumeUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
    public void OpenControls() {
        controlsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void CloseControls() {
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}