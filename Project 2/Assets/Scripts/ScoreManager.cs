using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance = null;
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public int SCORE_EXPLORE;
    public int SCORE_BOMB;
    public int SCORE_CORRECT_MARK;
    public int SCORE_INCORRECT_MARK;
    public int SCORE_GOAL;

    public PlayerMovement Player1;
    public PlayerMovement Player2;
    public UnityEngine.UI.Text Player1ScoreText;
    public UnityEngine.UI.Text Player2ScoreText;

    void Update() {
        Player1ScoreText.text = Player1.Score.ToString();
        Player2ScoreText.text = Player2.Score.ToString();
	}
}
