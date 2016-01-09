using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public GameManager.Control Control;
    public float Acceleration = 5;
    public float OnGoalInvulnerabilityTime = 1.0f;
    public bool IsInvulnerable = false;
    public float InvulnerableFlashSpeed = 5;

    private float _invulnerabilityTimer = 0;

    void Start () {
	
	}
	
	void Update () {
        if (_invulnerabilityTimer > 0) {
            IsInvulnerable = true;
            _invulnerabilityTimer -= Time.deltaTime;
        }
        else {
            IsInvulnerable = false;
        }

        if (IsInvulnerable) {
            float r = 1 - ((0.5f * Mathf.Sin(_invulnerabilityTimer * InvulnerableFlashSpeed)) + 0.5f);
            GetComponent<SpriteRenderer>().color = new Color(r, 1, 1, 1);
        }
        else {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Control == GameManager.Control.PLAYER_ONE) {
            Vector2 move = new Vector2(0, Input.GetAxisRaw("P1GoalVertical")).normalized * Acceleration;
            GetComponent<Rigidbody2D>().AddForce(move, ForceMode2D.Force);
        }
        else if (Control == GameManager.Control.AI) {
            GameManager.Quadrant q = GameManager.Instance.GetPuckQuadrant();
            if (q == GameManager.Quadrant.TOPLEFT || q == GameManager.Quadrant.TOPRIGHT) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1 * Acceleration), ForceMode2D.Force);
            }
            else {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Acceleration), ForceMode2D.Force);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (!IsInvulnerable) {
            if (c.collider.gameObject.tag == "Puck") {
                GameManager.Instance.EventGoalScored(this);
                Debug.Log("Scored on " + gameObject.name);
                _invulnerabilityTimer = OnGoalInvulnerabilityTime;
            }
        }
    }
}
