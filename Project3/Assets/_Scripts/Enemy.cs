using UnityEngine;
using System.Collections;

public class Enemy : Character {

    public Collider2D FrontCollider;
    public Collider2D BackCollider;
    public Collider2D WeaponCollider;
    public GameObject CoffeePrefab;
    public int MinCoffee = 1;
    public int MaxCoffee = 3;

    public float OnHitPlayerFreezeLength = 0.5f;

    
    void Start () {
	    
	}

    protected override void Update () {
        base.Update();
	}


    public void RegisterCollision(Collision2D collision, Collider2D thisCollider) {
        if (collision.collider.gameObject.tag != "Player") return;

        Character player = collision.collider.GetComponent<Character>();

        if (thisCollider == FrontCollider || thisCollider == WeaponCollider) {
            Debug.Log("Hit front");
            if (!player.Frozen) {
                Player.Instance.GetComponent<Player>().PlayerHit();
                StartCoroutine(player.Knock(false, player.KnockDuration, player.KnockSpeed, false));
            }
            StartCoroutine(FreezeForSeconds(OnHitPlayerFreezeLength));
        }
        if (thisCollider == BackCollider) {
            Debug.Log("Hit back");
            if (!Frozen) {
                CreateCoffee();
                StartCoroutine(Knock(true, KnockDuration, KnockSpeed, true));
            }
        }

    }

    public void CreateCoffee() {
        int rand = Random.Range(MinCoffee, MaxCoffee);
        for (int i=0; i< rand; i++) {
            GameObject coffee = GameObject.Instantiate(CoffeePrefab, transform.position, transform.rotation) as GameObject;
            coffee.GetComponent<Coffee>().Shoot();
        }
    }
}
