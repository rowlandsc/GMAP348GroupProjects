using UnityEngine;
using System.Collections;

public class DDOL : MonoBehaviour
{
    public static DDOL Instance = null;
	// Use this for initialization
	void Awake ()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
            Destroy(this.gameObject);
        
	}
	
}
