using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public enum State { IDLE, CHASE };
    public State CurrentState = State.IDLE;
    public Enemy _enemy;

    public float IdleSpeed = 4.0f;
    public float ChaseSpeed = 6.0f;
    public float ChaseTimer = 1.0f;

    private float _currentChaseTimer = 0;

    void Start() {
        _enemy = GetComponent<Enemy>();
    }

	void Update() {
        if (_enemy.Frozen) return;

        UpdateState();

        if (CurrentState == State.IDLE) {
            Idle();
        }
        else {
            Chase();
        }
    }

    protected virtual void Idle() {

    }

    protected virtual void Chase() {

    }

    void UpdateState() {
        if ((_enemy.CurrentFacing == Character.Facing.RIGHT && Player.Instance.transform.position.x > transform.position.x) ||
            (_enemy.CurrentFacing == Character.Facing.LEFT && Player.Instance.transform.position.x < transform.position.x) ) {

            int direction = 1;
            if (_enemy.CurrentFacing == Character.Facing.LEFT) {
                direction = -1;
            }
            RaycastHit2D hit = Physics2D.Linecast(transform.position + new Vector3(direction * 3, 1, 0), transform.position + (Player.Instance.transform.position - transform.position).normalized * Camera.main.orthographicSize * 0.8f);
            if (hit.collider && hit.collider.gameObject.tag == "Player") {
                _currentChaseTimer = ChaseTimer;
            }
            else {
                _currentChaseTimer -= Time.deltaTime;
            }
        }
        else {
            _currentChaseTimer -= Time.deltaTime;
        }

        if (_currentChaseTimer <= 0) {
            _currentChaseTimer = 0;
            CurrentState = State.IDLE;
        }
        else {
            CurrentState = State.CHASE;
        }
    }
}
