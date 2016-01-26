using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatAnimation : MonoBehaviour {

    public float Lifetime = 0.0f;
    public bool RiseOverTime = false;

    public float _timer = 0;
    public Text _text;
    public Outline _outline;
    float _ypos;

    void Start() {
        _text = GetComponent<Text>();
        _outline = GetComponent<Outline>();
        _ypos = transform.position.y;
    }

    void Update () {
        if (_timer >= Lifetime) Destroy(gameObject);
        _timer += Time.deltaTime;

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - (Time.deltaTime / Lifetime));
        _outline.effectColor = new Color(_outline.effectColor.r, _outline.effectColor.g, _outline.effectColor.b, _outline.effectColor.a - (Time.deltaTime / Lifetime));

        if (RiseOverTime) {
            transform.position = new Vector2(transform.position.x, _ypos + Mathf.Sin(_timer / Lifetime) * CombatAnimationManager.Instance.RiseDistance);
        }
	}
}
