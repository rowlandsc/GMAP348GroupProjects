using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player Instance = null;
    Stats playerStats;

    public float xpEarned = 0;
   
    
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

	void Start ()
    {
            
        playerStats = Player.Instance.GetComponent<Stats>();
	    
	}
	
	void Update ()
    {
        
    }

    public void calcLevel()
    {
        playerStats.cLevel = (int)xpEarned / 100;
        


    }


}
