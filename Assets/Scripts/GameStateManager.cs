using UnityEngine;
// this is just an extra level of protection so we don't encounter any issues where certain methods are called when they aren't supposed to be
public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }
    private GameStates currentGameState;

    public enum GameStates {
        MainMenu,
        Playing,
        Paused,
        GameOver // death screen
    }
    public void SetGameState(GameStates newState) {
        currentGameState = newState;
    }
    public GameStates CheckGameState() {
        return currentGameState;
    }
    void Awake() {
        if (Instance != null && Instance != this) { // making it a singleton
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentGameState = GameStates.MainMenu; // default state
    }
}
