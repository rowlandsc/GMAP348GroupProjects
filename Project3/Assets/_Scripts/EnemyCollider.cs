using UnityEngine;
using System.Collections;

public class EnemyCollider : MonoBehaviour {

    public Enemy ParentEnemy;

    private Collider2D _collider;

    void Start() {
        _collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D coll) {
        ParentEnemy.RegisterCollision(coll, _collider);
    }
}
