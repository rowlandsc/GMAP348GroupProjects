using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIStats : MonoBehaviour {

    public Text playerLevelText;
    public Text playerXPText;
    public Text enemyLevelText;
    public Slider playerHealth;
    public Slider enemyHealth;
	// Use this for initialization
	void Start ()
    {
	
	}
    
    void onGUI()
    {

    }

	// Update is called once per frame
	void Update ()
    {
        playerLevelText.text = "Level : " + Player.Instance.GetComponent<Stats>().cLevel;
        playerXPText.text = "Total XP : " + Player.Instance.xpEarned;
        if (CombatSystem.Instance.CurrentEnemy) {
            enemyLevelText.enabled = true;
            enemyLevelText.text = "Level: " + CombatSystem.Instance.CurrentEnemy.GetComponent<Stats>().cLevel;
        }
        else {
            enemyLevelText.enabled = false;
        }
	}
}
