using UnityEngine;
using System.Collections;

public class BackWall : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D c) {
        if (c.collider.gameObject.tag == "Puck") {
            GameManager.Instance.EventGoalWallHit(this);
            Debug.Log("Hit back wall " + gameObject.name);
        }
    }
}
