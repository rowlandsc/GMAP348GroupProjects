using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    public void Awake() {
        if (!Instance) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public bool GameOver = false;
    public float CurrentTime = 0;
    public Text TimeText;
    public Text coffee;
    public RectTransform GameOverRect;
    public float GameOverScaleIncreaseSpeed = 1.1f;
    public Text GameOverTime;

    void Update() {
        if (!GameOver) CurrentTime += Time.deltaTime;
        coffee.text = "Coffee: " + Player.Instance.Coffee.ToString();
        if (Input.GetKeyUp("r")) {
            Application.LoadLevel(0);
            
        };

        UpdateTimeText();
    }

    void UpdateTimeText() {
        int time = Mathf.FloorToInt(CurrentTime);
        int min = time / 60;
        int sec = time % 60;

        string minString = min.ToString();
        string secString = (sec >= 10) ? sec.ToString() : "0" + sec.ToString();

        TimeText.text = minString + ":" + secString;
    }

    public void EndGame() {
        GameOver = true;
        GameOverTime.text = TimeText.text;
        GameOverRect.gameObject.SetActive(true);

        StartCoroutine(EndGameCoroutine(Player.Instance.ExplosionLength * 0.9f));
    }

    IEnumerator EndGameCoroutine(float time) {
        float timer = 0;
        while (timer <= time) {
            timer += Time.deltaTime;
            GameOverRect.localScale *= GameOverScaleIncreaseSpeed;
            yield return null;
        }
    }
}
