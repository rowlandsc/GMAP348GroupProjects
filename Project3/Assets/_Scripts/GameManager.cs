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
}
