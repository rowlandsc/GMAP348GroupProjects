using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    public virtual List<CombatSystem.AttackType> GetInputSeries(int length) {
        return new List<CombatSystem.AttackType>(length);
    }

    public void Kill() {
        WaveManager.Instance.CurrentEnemies.Remove(gameObject);
        Destroy(this);
    }
}
