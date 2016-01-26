using UnityEngine;
using System.Collections.Generic;

public class Necromancer : Enemy {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override List<CombatSystem.AttackType> GetInputSeries(int length) {
        int maxattacks = GetComponent<Stats>().maxAttacks;
        List<CombatSystem.AttackType> attackList = new List<CombatSystem.AttackType>();
        for (int i = 0; i < maxattacks; i++) {
            if (i == 0 || attackList[i-1] != CombatSystem.AttackType.CHARGE) {
                if (Random.Range(0, 100) > 50) {
                    attackList.Add(CombatSystem.AttackType.QUICKATTACK);
                }
                else {
                    attackList.Add(CombatSystem.AttackType.CHARGE);
                }
            }
            else {
                if (Random.Range(0, 100) > 50) {
                    attackList.Add(CombatSystem.AttackType.CHARGE);
                }
                else {
                    attackList.Add(CombatSystem.AttackType.SPECIALATTACK);
                }
            }
        }
        return attackList;
    }
}
