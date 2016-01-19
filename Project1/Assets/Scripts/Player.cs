using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player Instance = null;
    //Stats playerStats = Player.Instance.GetComponent<Stats>();

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
        
        
	    
	}
	
	void Update ()
    {
        
    }

    //public void calcLevel()
    //{
    //    if(xpEarned >= 100)
    //    {
            
    //        playerStats.cLevel = 2;
    //    }
    //    else if (xpEarned >= 200)
    //    {
    //        playerStats.cLevel = 3;
    //    }
    //    else if (xpEarned >= 300)
    //    {
    //        playerStats.cLevel = 4;
    //    }
    //    else if (xpEarned >= 400)
    //    {
    //        playerStats.cLevel = 5;
    //    }
    //    else
    //    {
    //        playerStats.cLevel = 1;
    //    }


    //}

    
}
