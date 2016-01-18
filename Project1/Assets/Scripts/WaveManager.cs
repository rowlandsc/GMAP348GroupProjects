using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

    public static WaveManager Instance = null;
    public float EnemySpeed = 1;
    public float CombatStartDistance = 1.5f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start () {
	
	}
	
	void Update () {
       
        if (!CombatSystem.Instance.IsInCombat) {
            Enemy[] enemiesInGame = GameObject.FindObjectsOfType<Enemy>();
            foreach (Enemy e in enemiesInGame) {
                if (Vector3.Distance(e.transform.position, Player.Instance.transform.position) < CombatStartDistance) {
                    CombatSystem.Instance.StartCombat(e);
                    break;
                }
            }

            foreach (Enemy e in enemiesInGame) {
                e.transform.position += new Vector3(-1 * EnemySpeed * Time.deltaTime, 0, 0);
            }
        }
	}
}
