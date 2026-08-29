using UnityEngine;
using UnityEngine.UI;
// must add this script because levelmanager is a singleton and hence we can't directly call its functions from the button UI
public class ButtonControls : MonoBehaviour {
    // public void NextScene() {
    //     LevelManager.Instance.NextScene();
    // }
    // public void PreviousScene() {
    //     LevelManager.Instance.PreviousScene();
    // }
    private Slider masterSlider;

    private Slider bgSlider;
    private Slider sfxSlider;

    void Start()
    {
        Transform volumeUI = GameObject.Find("Canvas").transform.Find("VolumeUI");
        if (volumeUI != null)
        {
            masterSlider = volumeUI.transform.Find("Master Slider").GetComponent<Slider>();
            bgSlider = volumeUI.transform.Find("BG Slider").GetComponent<Slider>();
            sfxSlider = volumeUI.transform.Find("SFX Slider").GetComponent<Slider>();
        }
    }

    public void QuitGame() {
        LevelManager.Instance.QuitGame();
    }
 
    public void RestartRace() {
        LevelManager.Instance.RestartRace();
    }
    public void ToOverworld() {
        LevelManager.Instance.ToOverworld();
    }
    public void ToMainMenu() {
        LevelManager.Instance.ToMainMenu();
    }
    public void ToCharacterSelect() {
        LevelManager.Instance.ToCharacterSelect();
    }
    public void ToRaceScene() {
        LevelManager.Instance.ToRaceScene();
    }
    public void LoadSceneByName(string sceneName) {
        LevelManager.Instance.LoadSceneByName(sceneName);
    }
    public void PauseGame() {
        LevelManager.Instance.PauseGame();
    }
    public void ResumeGame() {
        LevelManager.Instance.ResumeGame();
    }
    public void OpenVolumeSettings() {
        LevelManager.Instance.OpenVolumeSettings();
    }
    public void CloseVolumeSettings() {
        LevelManager.Instance.CloseVolumeSettings();
    }
    public void ChangeMasterVolume()
    {
        LevelManager.Instance.ChangeVolumeSetings(Volume.Master, masterSlider.value);
    }
    public void ChangeBGVolume()
    {
        LevelManager.Instance.ChangeVolumeSetings(Volume.BG, bgSlider.value);
    }
    public void ChangeSFXVolume()
    {
        LevelManager.Instance.ChangeVolumeSetings(Volume.SFX, sfxSlider.value);
    }
    public void OpenControls() {
        LevelManager.Instance.OpenControls();
    }
    public void CloseControls() {
        LevelManager.Instance.CloseControls();
    }
}
