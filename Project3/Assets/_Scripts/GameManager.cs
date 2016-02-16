using UnityEngine;
using System.Collections;

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


    void Update() {
        while (!GameOver) CurrentTime += Time.deltaTime;

        if (Input.GetKeyUp("r")) {
            Application.LoadLevel(0);
        };
    }
}
