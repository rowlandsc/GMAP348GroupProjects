﻿using UnityEngine;
using System.Collections.Generic;

public class Goblin : Enemy {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override List<CombatSystem.AttackType> GetInputSeries(int length)
    {
        int temp = GetComponent<Stats>().maxAttacks;
        bool s = false;
        List<CombatSystem.AttackType> list1 = new List<CombatSystem.AttackType>();
        for (int i = 0; i < temp; i++)
        {
            if (s == false)
            {
                list1.Add(CombatSystem.AttackType.QUICKATTACK);
                s = true;
            }
            else
            {
                list1.Add(CombatSystem.AttackType.ATTACK);
                s = false;
            }
        }
        return list1;
    }
}
