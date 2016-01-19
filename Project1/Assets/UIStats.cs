using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIStats : MonoBehaviour {

    public Text levelText;
    public Text playerXP;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        levelText.text = "Level : " + Player.Instance.GetComponent<Stats>().cLevel;
        playerXP.text = "Total XP : " + Player.Instance.xpEarned;
	}
}
