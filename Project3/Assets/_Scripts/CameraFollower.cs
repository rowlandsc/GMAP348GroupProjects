using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    public Transform Leader;
    public float Speed = 8;
    public bool Frozen = false;

    void Start() {
        transform.position = new Vector3(Leader.position.x, Leader.position.y, transform.position.z);
    }
	
	void Update () {
        transform.position += (new Vector3(Leader.position.x, Leader.position.y, transform.position.z) - transform.position) * 8 * Time.deltaTime;
    }
}
