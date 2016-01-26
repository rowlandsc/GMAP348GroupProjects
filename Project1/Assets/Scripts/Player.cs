using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player Instance = null;
    Stats playerStats;

    public float xpEarned = 0;
   
    public SpriteRenderer PlayerSprite;
    
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
        CombatSystem.OnCombatEnemyKilled += OnEnemyDeathXp;
        WaveManager.OnWaveEnd += RestoreHealth;
         
	}
	

    public void CalcLevel()
    {
        //playerStats.cLevel = ((int) xpEarned / 100) + 1;
        playerStats.cLevel = (int)(-.0000002f * Mathf.Pow(xpEarned, 2.0f) + 0.0039f * xpEarned + 1.7841f);

        playerStats.CalcStats();

    }

    public void OnEnemyDeathXp()
    {
        xpEarned += CombatSystem.Instance.CurrentEnemy.GetComponent<Stats>().GetXpReward();
        XpBoost(playerStats.maxAttacks - CombatSystem.Instance.CurrentAction);
        CalcLevel();
        
    }

    public void XpBoost(int boost)
    {
        float xpBoost = CombatSystem.Instance.CurrentEnemy.GetComponent<Stats>().GetXpReward() / CombatSystem.Instance.CurrentEnemy.GetComponent<Stats>().maxAttacks * boost;
        xpEarned += xpBoost;
       
                
    }

    public void OnDestroy()
    {
        CombatSystem.OnCombatEnemyKilled -= OnEnemyDeathXp;
    }

    public void RestoreHealth()
    {
        playerStats.RestoreToFullHealth();
    }

}
