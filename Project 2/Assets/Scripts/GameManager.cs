using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public Map GameMap;
    public int GridWidth = 45;
    public int GridHeight = 20;
    public float PercentBombs = 0.15f;
    public int NumberOfPlayers = 2;

    public GameObject p1;
    public GameObject p2;

    public bool IsGameOver = false;
    public GameObject GameOverScreen;
    public GameObject Player1Wins;
    public GameObject Player2Wins;
    public GameObject Tie;

    public float TileRevealInterval = 0.1f;

    void Start () {
        GameMap.Initialize(GridWidth, GridHeight, PercentBombs, NumberOfPlayers);
        p1.gameObject.SetActive(true);
        p2.gameObject.SetActive(true);

    }

    public void Update() {
        if (IsGameOver && Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel(0);
        } 
    }
	
	public IEnumerator GameOver() {
        IsGameOver = true;

        PlayerMovement player1 = p1.GetComponent<PlayerMovement>();
        PlayerMovement player2 = p2.GetComponent<PlayerMovement>();

        yield return StartCoroutine(RevealMap());
        while (ScoreManager.Instance.Player1ScoreChanging || ScoreManager.Instance.Player2ScoreChanging) {
            yield return null;
        }

        GameOverScreen.SetActive(true);
        if (player1.Score > player2.Score) {
            Player1Wins.SetActive(true);
        }
        else if (player1.Score < player2.Score) {
            Player2Wins.SetActive(true);
        }
        else {
            Tie.SetActive(true);
        }

        ScreenShake.Instance.ShakeCamera(ScreenShake.Instance.Duration, ScreenShake.Instance.Strength * 2, ScreenShake.Instance.Vibrato, ScreenShake.Instance.Randomness);
    }

    IEnumerator RevealMap() {

        PlayerMovement player1 = p1.GetComponent<PlayerMovement>();
        PlayerMovement player2 = p2.GetComponent<PlayerMovement>();

        for (int i = Map.Instance.TileMap.Count - 1; i >= 0;  i--) {
            for (int j = 0; j < Map.Instance.TileMap[i].Count / 2.0f; j++) {
                MapTile tileDown = Map.Instance.TileMap[i][Mathf.FloorToInt(Map.Instance.TileMap[i].Count / 2.0f) - j - 1];
                if (tileDown.HasBomb) {
                    tileDown.IsVisible = true;
                    if (tileDown.P1Marked) {
                        player1.Score += ScoreManager.Instance.SCORE_CORRECT_MARK;
                        //ScoreManager.Instance.CreateScoreText(ScoreManager.Instance.Player1ScoreText, tileDown.transform.position, ScoreManager.Instance.SCORE_CORRECT_MARK);
                    }
                    if (tileDown.P2Marked) {
                        player2.Score += ScoreManager.Instance.SCORE_CORRECT_MARK;
                    }
                }
                else {
                    tileDown.IsExplored = true;
                    if (tileDown.P1Marked) {
                        player1.Score += ScoreManager.Instance.SCORE_INCORRECT_MARK;
                    }
                    if (tileDown.P2Marked) {
                        player2.Score += ScoreManager.Instance.SCORE_INCORRECT_MARK;
                    }
                }

                MapTile tileUp = Map.Instance.TileMap[i][Mathf.FloorToInt(Map.Instance.TileMap[i].Count / 2.0f) + j];
                if (tileUp.HasBomb) {
                    tileUp.IsVisible = true;
                    if (tileUp.P1Marked) {
                        player1.Score += ScoreManager.Instance.SCORE_CORRECT_MARK;
                    }
                    if (tileUp.P2Marked) {
                        player2.Score += ScoreManager.Instance.SCORE_CORRECT_MARK;
                    }
                }
                else {
                    tileUp.IsExplored = true;
                    if (tileUp.P1Marked) {
                        player1.Score += ScoreManager.Instance.SCORE_INCORRECT_MARK;
                    }
                    if (tileUp.P2Marked) {
                        player2.Score += ScoreManager.Instance.SCORE_INCORRECT_MARK;
                    }
                }

                yield return new WaitForSeconds(TileRevealInterval);
            } 
        }
    }

    
}
