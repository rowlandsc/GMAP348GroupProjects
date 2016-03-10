using UnityEngine;
using System.Collections;
using DG.Tweening;

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
    public float Player1ApparentScore;
    public float Player2ApparentScore;
    
    public ScoreDisplay Player1ScoreText;
    public ScoreDisplay Player2ScoreText;

    public float ScoreChangeRate = 50;
    public float ScoreShakeDuration = 0.5f;
    public float ScoreShakeStrength = 0.5f;
    public int ScoreShakeVibrato = 10;
    public float ScoreShakeRandomness = 5f;
    public bool Player1ScoreChanging = false;
    public bool Player2ScoreChanging = false;

    public Transform ScoreTextParent;
    public GameObject ScoreTextPrefab;
    public float ScoreTextFadeDuration = 1.5f;

    void Start() {
        Player1ApparentScore = Player1.Score;
        Player2ApparentScore = Player2.Score;
        StartCoroutine(UpdatePlayer1Score());
        StartCoroutine(UpdatePlayer2Score());
    }

    void Update() {
        Player1ScoreText.DisplayText.text = ((int) Player1ApparentScore).ToString();
        Player2ScoreText.DisplayText.text = ((int) Player2ApparentScore).ToString();
	}

    IEnumerator UpdatePlayer1Score() {
        while (true) {
            if (Mathf.Abs(Player1ApparentScore - Player1.Score) < float.Epsilon) {
                yield return null;
                continue;
            }

            Player1ScoreChanging = true;
            float diff = Mathf.Abs(Player1.Score - Player1ApparentScore);
            ShakeScoreDisplay(Player1ScoreText, (int)(diff));

            float changeRate = (diff <= 10) ? ScoreChangeRate : ScoreChangeRate * (diff / 10f); 

            float targetScore = Player1.Score;
            int direction = (targetScore > Player1ApparentScore) ? 1 : -1;
            while ((direction > 0 && (targetScore - Player1ApparentScore) > float.Epsilon) || (direction < 0 && (Player1ApparentScore - targetScore) > float.Epsilon)) {
                yield return null;
                Player1ApparentScore += direction * changeRate * Time.deltaTime;
            }
            Player1ApparentScore = targetScore;
            Player1ScoreChanging = false;
        }
    }

    IEnumerator UpdatePlayer2Score() {
        while (true) {
            if (Mathf.Abs(Player2ApparentScore - Player2.Score) < float.Epsilon) {
                yield return null;
                continue;
            }

            Player2ScoreChanging = true;
            float diff = Mathf.Abs(Player1.Score - Player1ApparentScore);
            ShakeScoreDisplay(Player2ScoreText, (int)(diff));

            float changeRate = (diff <= 10) ? ScoreChangeRate : ScoreChangeRate * (diff / 10f);

            float targetScore = Player2.Score;
            int direction = (targetScore > Player2ApparentScore) ? 1 : -1;
            while ((direction > 0 && (targetScore - Player2ApparentScore) > float.Epsilon) || (direction < 0 && (Player2ApparentScore - targetScore) > float.Epsilon)) {
                yield return null;
                Player2ApparentScore += direction * changeRate * Time.deltaTime;
            }
            Player2ApparentScore = targetScore;
            Player2ScoreChanging = false;
        }
    }

    void ShakeScoreDisplay(ScoreDisplay display, int scoreDiff) {
        display.Shake(ScoreShakeDuration, ScoreShakeStrength * (scoreDiff / 100f), (int) (ScoreShakeVibrato * (scoreDiff / 100f)), ScoreShakeRandomness * (scoreDiff / 100f));
    }

    public void CreateScoreText(ScoreDisplay display, Vector3 position, int value) {
        Debug.Log("Making score text");
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);

        GameObject clone = Instantiate(ScoreTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        ScoreText scoreText = clone.GetComponent<ScoreText>();
        RectTransform rect = clone.GetComponent<RectTransform>();
        rect.SetParent(ScoreTextParent);
        rect.anchorMin = new Vector2(viewportPoint.x, viewportPoint.y);
        rect.anchorMax = new Vector2(viewportPoint.x, viewportPoint.y);
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one;

        scoreText.Initialize(display, value);
    }
}
