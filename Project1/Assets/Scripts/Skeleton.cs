using UnityEngine;
using System.Collections.Generic;

public class Skeleton : Enemy {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override List<CombatSystem.AttackType> GetInputSeries(int length)
    {
        return new List<CombatSystem.AttackType>(length);
    }
}
