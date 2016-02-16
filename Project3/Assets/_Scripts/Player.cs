using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float Coffee = 50;
    public static Player Instance = null;
    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this.gameObject);
    }
	
	void Update () {
	
	}
    public void PlayerHit()
    {
        Debug.Log("coffee");
        Coffee -= 5f;
    }
}
