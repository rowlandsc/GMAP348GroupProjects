using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player Instance = null;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

	void Start () {
	    
	}
	
	void Update () {
	    
	}
}
