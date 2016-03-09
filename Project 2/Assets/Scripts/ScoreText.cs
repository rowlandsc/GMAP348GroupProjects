using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour {

    public ScoreDisplay Destination;
    public int Value;
    public UnityEngine.UI.Text UIText;

    public Vector2 Velocity;

    private bool _initialized = false;

	public void Initialize(ScoreDisplay dest, int value, float minSpeed, float maxSpeed) {
        Destination = dest;
        Value = value;

        int xDirection = (transform.position.x <= Destination.transform.position.x) ? 1 : -1;
        int yDirection = (transform.position.y <= Destination.transform.position.y) ? 1 : -1;
        Velocity = new Vector2(xDirection * (Random.Range(0, maxSpeed - minSpeed) + minSpeed), yDirection * (Random.Range(0, maxSpeed - minSpeed) + minSpeed));
    }
	
	void Update () {
	    if (!_initialized) return;

        Vector3 gravity = (Destination.transform.position - transform.position).normalized * ScoreManager.Instance.ScoreTextGravity * Time.deltaTime / Mathf.Pow(Vector3.Distance(transform.position, Destination.transform.position), 2f);
        Velocity += new Vector2(gravity.x, gravity.y);
	}
}
