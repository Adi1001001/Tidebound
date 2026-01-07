using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }
    public GameObject pauseMenuUI;
    public GameObject raceUI;
    public GameObject volumeUI;
    public GameObject controlsUI;
    void Awake() {
        if (Instance != null && Instance != this) { // making it a singleton
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void NextScene() { // going to the next level.
        Debug.Log("NEXT SCENE");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PreviousScene() { // going to the previous level.
        Debug.Log("PREVIOUS SCENE");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame() { // closing the game
        Debug.Log("END GAME");
        Application.Quit();
    }
    public void RestartCurrentScene() { // restarting the level. make it resume the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ToMainMenu() { // going back to the main menu without losing all of the save data.
        SceneManager.LoadScene("Main Menu");
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.MainMenu);
    }
    public void LoadSceneByName(string sceneName) {
        Debug.Log("LOADING SCENE: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void PauseGame() {
        Time.timeScale = 0f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Paused);
        pauseMenuUI.SetActive(true);
        raceUI.SetActive(false);
    }
    public void ResumeGame() {
        Time.timeScale = 1f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
        pauseMenuUI.SetActive(false);
        raceUI.SetActive(true);
    }
    public void VolumeButton() {
        volumeUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void VolumeBackButton() {
        volumeUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
    public void ControlsButton() {
        controlsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void ControlsBackButton() {
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}