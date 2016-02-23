using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float Coffee = 50;
    public static Player Instance = null;

    public float ExplosionLength = 5.0f;
    public float ExplosionZoomSpeed = 1.0f;
    public float ExplosionDensityBegin = 1f;
    public float ExplosionDensityEnd = 5f;
    public GameObject Explosion;
    
    private Character _playerCharacter;
    private bool _exploded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this.gameObject);
    }
    void Start() {
        _playerCharacter = GetComponent<Character>();
    }
	
	void Update () {
	    if (Coffee >= 100 && !_exploded) {
            StartCoroutine(CoffeeExplosion());
            _exploded = true;
        }
	}
    public void PlayerHit()
    {
        Debug.Log("coffee");
        Coffee -= 5f;
    }

    IEnumerator CoffeeExplosion() {
        _playerCharacter.Frozen = true;
        _playerCharacter.Invulnerable = true;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);
        Enemy[] chars = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in chars) {
            if (!enemy.gameObject.activeSelf) continue;
            enemy.Frozen = true;
        }

        float timer = 0;
        float explosionDensity = ExplosionDensityBegin;
        while (timer < ExplosionLength) {
            timer += Time.deltaTime;

            explosionDensity += Time.deltaTime / (ExplosionDensityEnd - ExplosionDensityBegin);
            Camera.main.orthographicSize += ExplosionZoomSpeed * Time.deltaTime;
            for (int i=0; i<explosionDensity; i++) { 
                float x = Random.Range(0.0f, 1.0f);
                float y = Random.Range(0.0f, 1.0f);

                Vector3 worldpoint = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
                Instantiate(Explosion, worldpoint, Explosion.transform.rotation);
            }

            yield return null;
        }

        foreach (Enemy enemy in chars) {
            if (!enemy.gameObject.activeSelf) continue;
            StartCoroutine(enemy.Knock(true, enemy.KnockDuration, enemy.KnockSpeed, true));
        }

    }
}
