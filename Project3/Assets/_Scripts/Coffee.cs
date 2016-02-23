using UnityEngine;
using System.Collections;

public class Coffee : MonoBehaviour {

    public float CoffeeBonus = 5;
    public float MaxSpeed = 1.0f;
    public GameObject Explosion;
	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.collider.gameObject.tag == "Player") {
            coll.collider.gameObject.GetComponent<Player>().Coffee += CoffeeBonus;
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    public void Shoot() {
        Vector2 velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized * MaxSpeed;

        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
