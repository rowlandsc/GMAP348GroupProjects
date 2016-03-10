using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScoreText : MonoBehaviour {

    public ScoreDisplay Destination;
    public int Value;
    public UnityEngine.UI.Text UIText;
    

    public Vector2 Velocity;

    private bool _initialized = false;
    private RectTransform _rect;
    private RectTransform _destRect;

	public void Initialize(ScoreDisplay dest, int value) {
        Destination = dest;
        Value = value;

        _rect = GetComponent<RectTransform>();
        _destRect = Destination.GetComponent<RectTransform>();
        
        UIText.text = (value >= 0) ? "+" + value.ToString() : value.ToString();

        UIText.DOFade(0, ScoreManager.Instance.ScoreTextFadeDuration);

        _initialized = true;
    }
}
