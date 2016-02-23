using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public enum State { IDLE, CHASE };
    public State CurrentState = State.IDLE;
    public Enemy _enemy;

    public float IdleSpeed = 4.0f;
    public float ChaseSpeed = 6.0f;

    void Start() {
        _enemy = GetComponent<Enemy>();
    }

	void Update() {
        if (_enemy.Frozen) return;

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
}
