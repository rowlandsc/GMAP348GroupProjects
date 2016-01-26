using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public SpriteRenderer Background;
    public float BackgroundFadeTimer = 2.0f;
    public float GameOverTextTimer = 1.0f;
    public UnityEngine.UI.Text GameOverText;
    public UnityEngine.UI.Text InstructionsText;


	void Start () {
        StartCoroutine(GameOverAnimation());
	}

    void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            Application.LoadLevel("MainMenu");
        }
    }
	
	
	IEnumerator GameOverAnimation() {
        float _timer = 0;
        while (_timer < BackgroundFadeTimer) {
            _timer += Time.deltaTime;

            Background.color = new Color(1, Background.color.g - Time.deltaTime / BackgroundFadeTimer, Background.color.b - Time.deltaTime / BackgroundFadeTimer);
            yield return null;
        }

        yield return new WaitForSeconds(GameOverTextTimer);

        GameOverText.gameObject.SetActive(true);
        InstructionsText.gameObject.SetActive(true);
    }
}
