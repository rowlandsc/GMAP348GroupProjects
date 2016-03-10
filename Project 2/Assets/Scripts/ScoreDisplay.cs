using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class ScoreDisplay : MonoBehaviour {

    public int Player = 1;
    public float AbsorbDistance = 1;

    public UnityEngine.UI.Text DisplayText;

    public List<ScoreText> IncomingScores = new List<ScoreText>();

    private RectTransform _rect;

    void Start() {
        _rect = GetComponent<RectTransform>();
    }

    void Update() {
        foreach (ScoreText st in IncomingScores) {
            if (Vector3.Distance(_rect.anchoredPosition, st.GetComponent<RectTransform>().anchoredPosition) <= AbsorbDistance) {
                switch (Player) {
                    case 1:
                        ScoreManager.Instance.Player1.Score += st.Value;
                        break;
                    case 2:
                        ScoreManager.Instance.Player2.Score += st.Value;
                        break;
                }
                Debug.Log("Destroying " + st.gameObject.name);
                IncomingScores.Remove(st);
                Destroy(st);
            }
        }
    }
	
    public void Shake(float duration, float strength, int vibrato, float randomness) {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }
}
