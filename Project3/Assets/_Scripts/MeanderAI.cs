using UnityEngine;
using System.Collections;

public class MeanderAI : EnemyAI {

    public LayerMask TurnAroundAt;

    protected override void Idle() {
        if (_enemy.CurrentFacing == Character.Facing.RIGHT) {
            transform.position += new Vector3(IdleSpeed * Time.deltaTime, 0, 0);
            Collider2D col = Physics2D.OverlapArea(_enemy.WeaponCollider.bounds.min, _enemy.WeaponCollider.bounds.max, TurnAroundAt.value);
            if (col != null) {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else {
            transform.position -= new Vector3(IdleSpeed * Time.deltaTime, 0, 0);
            Collider2D col = Physics2D.OverlapArea(_enemy.WeaponCollider.bounds.min, _enemy.WeaponCollider.bounds.max, TurnAroundAt.value);
            if (col != null) {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    protected override void Chase() {
        if (_enemy.CurrentFacing == Character.Facing.RIGHT) {
            Collider2D col = Physics2D.OverlapArea(_enemy.WeaponCollider.bounds.min, _enemy.WeaponCollider.bounds.max, TurnAroundAt.value);
            if (col == null) {
                transform.position += new Vector3(ChaseSpeed * Time.deltaTime, 0, 0);
            }
        }
        else {   
            Collider2D col = Physics2D.OverlapArea(_enemy.WeaponCollider.bounds.min, _enemy.WeaponCollider.bounds.max, TurnAroundAt.value);
            if (col == null) {
                transform.position -= new Vector3(ChaseSpeed * Time.deltaTime, 0, 0);
            }
        }

        if ((Player.Instance.transform.position.x > transform.position.x && _enemy.CurrentFacing == Character.Facing.LEFT) ||
            (Player.Instance.transform.position.x < transform.position.x && _enemy.CurrentFacing == Character.Facing.RIGHT)) {

            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
