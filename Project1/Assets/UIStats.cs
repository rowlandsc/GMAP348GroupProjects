using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIStats : MonoBehaviour {

    public Text playerLevelText;
    public Text playerXPText;
    public Text enemyLevelText;
    public Text playerHealth;
    public Text enemyHealth;
	// Use this for initialization
	void Start ()
    {
	
	}
    

	// Update is called once per frame
	void Update ()
    {

        playerLevelText.text = "Level: " + Player.Instance.GetComponent<Stats>().cLevel;
        playerXPText.text = "Total XP: " + Player.Instance.xpEarned;
        playerHealth.text = "Health: " + Player.Instance.GetComponent<Stats>().GetHealth();

        if (CombatSystem.Instance.CurrentEnemy) {
            enemyLevelText.enabled = true;
            enemyHealth.enabled = true;
            enemyLevelText.text = "Level: " + CombatSystem.Instance.CurrentEnemy.GetComponent<Stats>().cLevel;
            enemyHealth.text = "Health: " + CombatSystem.Instance.CurrentEnemy.GetComponent<Stats>().GetHealth();
        }
        else {
            enemyLevelText.enabled = false;
            enemyHealth.enabled = false;
        }
	}
}
