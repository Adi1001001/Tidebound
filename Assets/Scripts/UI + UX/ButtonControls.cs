using UnityEngine;
// must add this script because levelmanager is a singleton and hence we can't directly call its functions from the button UI
public class ButtonControls : MonoBehaviour {
    // public void NextScene() {
    //     LevelManager.Instance.NextScene();
    // }
    // public void PreviousScene() {
    //     LevelManager.Instance.PreviousScene();
    // }
    public void QuitGame() {
        LevelManager.Instance.QuitGame();
    }
    // public void RestartCurrentScene() {
    //     LevelManager.Instance.RestartCurrentScene();
    // }
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
    public void OpenControls() {
        LevelManager.Instance.OpenControls();
    }
    public void CloseControls() {
        LevelManager.Instance.CloseControls();
    }
}
