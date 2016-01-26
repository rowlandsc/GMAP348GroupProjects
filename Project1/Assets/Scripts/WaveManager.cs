using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    public delegate void dWaveStart();
    public delegate void dWaveEnd();
    public static event dWaveStart OnWaveStart;
    public static event dWaveEnd OnWaveEnd;

    public static WaveManager Instance = null;
    public int CurrentWave = 0;
    public float EnemySpeed = 1;
    public float CombatStartDistance = 1.5f;
    public float WaveDifficultyStart = 3;
    public float WaveDifficultyIncrease = 5;
    public float CurrentDifficulty = 0;
    public float EnemyXPositionStart = 400;
    public float EnemyXPositionIncrease = 50;
    public float EnemyXPositionVariance = 25;
    public float EnemyYPosition = -3.5f;

    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    public List<int> EnemyWeights = new List<int>();

    public List<GameObject> CurrentEnemies = new List<GameObject>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start () {
        OnWaveStart += WaveStart;
        OnWaveEnd += WaveEnd;

        CurrentDifficulty = WaveDifficultyStart;

        OnWaveStart();
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

    void WaveStart() {
        CurrentWave++;
        if (CurrentWave == 1) CurrentDifficulty = WaveDifficultyStart;
        else CurrentDifficulty += WaveDifficultyIncrease;
        GenerateWave();
    }

    void WaveEnd() {

    }

    public void StartWave() {
        OnWaveStart();
    }

    public void GenerateWave() {
        int currentDiff = 0;
        List<GameObject> enemies = new List<GameObject>();
        while (currentDiff < CurrentDifficulty) {
            bool added = false;
            List<int> tried = new List<int>();

            while (tried.Count < EnemyPrefabs.Count) {
                int r = Random.Range(0, EnemyPrefabs.Count - 1);
                if (currentDiff + EnemyWeights[r] <= CurrentDifficulty) {
                    enemies.Add(EnemyPrefabs[r]);
                    added = true;
                    currentDiff += EnemyWeights[r];
                }
                else {
                    if (!tried.Contains(r)) tried.Add(r);
                }

                if (added) break;
            }

            if (added) continue;
            break;
        }

        CurrentEnemies.Clear();
        float previousX = EnemyXPositionStart;
        for (int i=0; i<enemies.Count; i++) {
            float x = previousX + EnemyXPositionIncrease + Random.Range(-1 * EnemyXPositionVariance, EnemyXPositionVariance);
            previousX = x;

            CurrentEnemies.Add((GameObject)GameObject.Instantiate(enemies[i], new Vector3(x, EnemyYPosition), enemies[i].transform.rotation));
        }
    }
}
