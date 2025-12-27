using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ScoreManager : MonoBehaviour {
    public TextMeshProUGUI scoreDisplay; // ive got to assign it in inspector
    private float score = 0f;

    void Update() {
        score += Time.deltaTime * 2; // The score is just time-based for now (we could add more points for coins or other actions)
        scoreDisplay.text = "Score: " + Mathf.FloorToInt(score).ToString(); // Displaying the score as a whole number
    }
    public int GetScore() { // for the leaderboard
        return Mathf.FloorToInt(score);
    }
}