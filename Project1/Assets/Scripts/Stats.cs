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

    public float _cHealth; //Current
    private float _cAttack; //Current
    private float _cXpReward; //Current
    private float _mHealth; //Max Health


    public CombatSystem.AttackType[] possibleAttacks;


    void Start()
    {
        CalcStats();
        _cHealth = _mHealth;
        GetXpReward();
    }

    public void CalcStats()
    {
        //All linear scaling based on dif modifier. Change to logrithmic scaling at some point possibly
        _mHealth = bHealth + (cLevel * levelDifMod * bHealth);

        _cAttack = bAttackMin + (cLevel * levelDifMod * (Random.Range(bAttackMin * 100, bAttackMax * 100) * .01f));
        
    }

    public float GetXpReward()
    {
        _cXpReward = bXpReward + (cLevel * levelDifMod * (Random.Range((bXpReward - 1) * 100, (bXpReward + 1) * 100) * .01f));
        return _cXpReward;
    }

    public float GetHealth()
    {
        return _cHealth;
    }

    public float GetAttack()
    {
        return _cAttack;
    }

    public bool GetIsAlive()
    {
        return (_cHealth > 0);
    }

    public void DealDamage(float damage)
    {
        _cHealth = _cHealth - damage;
    }

    public void RestoreToFullHealth()
    {
        _cHealth = _mHealth;
    }

    public float GetAttackDamage()
    {
        
        return Mathf.Round(_cAttack);
    }

    public float GetQuickAttackDamage()
    {
        return Mathf.Round(_cAttack * .75f) ;
    }

    public float GetSpecialAttackDamage(int turnsCharged)
    {   
        return Mathf.Round(_cAttack * (turnsCharged + 1));
    }
   
}
