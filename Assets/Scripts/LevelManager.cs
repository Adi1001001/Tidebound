using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public void PlayGame() { // going from the main menu to the game
        Debug.Log("PLAY GAME");
        SceneManager.LoadScene("SampleScene");
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.Playing); // Change the game state to 'InGame' (triggers the event)
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
    public void ResumeCurrentScene() { // restarting the level. make it resume the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame() { // restarting the game to main menu. but later on you also have to delete the save data.
        SceneManager.LoadScene("Main Menu");
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.MainMenu);
    }

    public void ToMainMenu() { // going back to the main menu without losing all of the save data.
        SceneManager.LoadScene("Main Menu");
        GameStateManager.Instance.SetGameState(GameStateManager.GameStates.MainMenu);
    }
}