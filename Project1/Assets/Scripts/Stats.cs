using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
    
    public float bAttackMin = 1.0f; //base
    public float bAttackMax = 2.0f; //base
    public float bHealth = 10.0f; //base

    public float levelDifMod = 1.0f;
    public int bXpReward = 5;
    public int maxAttacks = 5; //Talk about how we want to scale max attacks
    public int cLevel = 1; //Current level

    private float _cHealth; //Current
    private float _cAttack; //Current
    private float _cXp; //Current

    
    public CombatSystem.AttackType[] possibleAttacks;
    
    public Stats (float bAttackMin, float bAttackMax, float bHealth, float levelDifMod, int bXpReward, int maxAttacks, int cLevel)
    {
        this.bAttackMin = bAttackMin;
        this.bAttackMax = bAttackMax;
        this.bHealth = bHealth;
        this.levelDifMod = levelDifMod;
        this.bXpReward = bXpReward;
        this.maxAttacks = maxAttacks;
        this.cLevel = cLevel;
    }

    public Stats()
    {
        
    }

    void Start()
    {
        CalcStats();
                
    }

    public void CalcStats()
    {
        //All linear scaling based on dif modifier. Change to logrithmic scaling at some point possibly
        _cHealth = bHealth + (cLevel * levelDifMod * bHealth);
        _cAttack = bAttackMin + (cLevel * levelDifMod * (Random.Range(bAttackMin * 100, bAttackMax * 100) * .01f));
        _cXp = bXpReward + (cLevel * levelDifMod * (Random.Range((bXpReward - 1) * 100, (bXpReward + 1) * 100) * .01f));
    }

    public float GetHealth() {
        return _cHealth;
    }

    public float GetAttack() {
        return _cAttack;
    }

    public bool GetIsAlive() {
        return (_cHealth > 0);
    }
   
}
