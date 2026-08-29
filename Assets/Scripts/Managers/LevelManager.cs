using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }
    public GameObject pauseMenuUI;
    public GameObject raceUI;
    public GameObject volumeUI;
    public GameObject controlsUI;
    private float prevTimeScale;
    
    void Awake() {
        if (Instance != null && Instance != this) { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateUIReferences(GameObject pausePanel, GameObject racePanel, GameObject volumePanel, 
    GameObject controlsPanel) {
        pauseMenuUI = pausePanel;
        volumeUI = volumePanel;
        controlsUI = controlsPanel;
        raceUI = racePanel;
    }
    public void QuitGame() 
    { 
        Application.Quit();
    }
    public void RestartRace() 
    {
        Time.timeScale = 1f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Racing);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ToOverworld()
    {
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
        Time.timeScale = 1f;
        switch (DataCarrier.Instance.GetBiomeNum())
        {
            case 1:
                SceneManager.LoadScene("Overworld 1");
                break;
            case 2:
                SceneManager.LoadScene("Overworld 2");
                break;
            case 3:
                SceneManager.LoadScene("Overworld 3");
                break;
        }
    }
    public void ToCharacterSelect() 
    {
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.CharacterSelect);
        SceneManager.LoadScene("Character Select");
    }
    public void ToMainMenu() 
    { 
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.MainMenu);
        SceneManager.LoadScene("Main Menu");
    }
    public void ToRaceScene() 
    {
        string raceTag = DataCarrier.Instance.nextRaceTag;
        SceneManager.LoadScene(raceTag);
    }
    public void LoadSceneByName(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }
    public void PauseGame() 
    {
        if (GameStateManager.Instance.GetGameState() == GameStateManager.GameStates.MainMenu ||
        GameStateManager.Instance.GetGameState() == GameStateManager.GameStates.Countdown) {return;}

        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Paused);
        pauseMenuUI.SetActive(true);
        raceUI.SetActive(false);
    }
    public void ResumeGame() 
    {
        // Accounting for if game time was running slower than normal when paused.
        Time.timeScale = prevTimeScale;
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing);
        pauseMenuUI.SetActive(false);
        if (volumeUI != null) 
        {
            volumeUI.SetActive(false);
        }
        if (controlsUI != null)
        {
            controlsUI.SetActive(false);
        }
        raceUI.SetActive(true);
    }
    public void OpenVolumeSettings() 
    {
        volumeUI.SetActive(true);
        Slider masterSlider = volumeUI.transform.Find("Master Slider").GetComponent<Slider>();
        Slider bgSlider = volumeUI.transform.Find("BG Slider").GetComponent<Slider>();
        Slider sfxSlider = volumeUI.transform.Find("SFX Slider").GetComponent<Slider>();
        masterSlider.value = DataCarrier.Instance.GetVolume(Volume.Master);
        bgSlider.value = DataCarrier.Instance.GetVolume(Volume.BG);
        sfxSlider.value = DataCarrier.Instance.GetVolume(Volume.SFX);
        pauseMenuUI.SetActive(false);
    }

    public void ChangeVolumeSetings(Volume volType, float newVol)
    {
        DataCarrier.Instance.SetVolume(volType, newVol);
        GameObject bgAudioManager = GameObject.Find("BG_AudioManager");
        GameObject SFXManager = GameObject.Find("SFX Managers");
        if (bgAudioManager != null)
        {
            bgAudioManager.GetComponent<BGTrackPlayer>().UpdateVolume();
        }
        if (SFXManager != null)
        {
            SFXManager.GetComponent<SFXPlayer>().UpdateVolume();
        }
    }
    public void CloseVolumeSettings() 
    {
        volumeUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
    public void OpenControls() 
    {
        controlsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void CloseControls() 
    {
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}