using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public enum Facing { RIGHT, LEFT };

    public bool Frozen = false;
    public bool Invulnerable = false;
    public Facing CurrentFacing = Facing.RIGHT;

    public float KnockDuration = 1.0f;
    public float KnockSpeed = 1.0f;
    public float InvulnerableFlashSpeed = 1.0f;

    public static float DeathHeight = -200;

    protected Rigidbody2D _rigidbody;
    protected SpriteRenderer _spriteRenderer;

	void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected virtual void Update () {
        UpdateFacing();
	}

    protected virtual void UpdateFacing() {
        if (transform.localScale.x < 1) {
            CurrentFacing = Facing.LEFT;
        }
        else {
            CurrentFacing = Facing.RIGHT;
        }
    }

    public virtual IEnumerator Knock(bool forward, float duration, float driftSpeed, bool killOnFinish) {
        int currentMask = gameObject.layer;
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Knockback"));
        Frozen = true;

        if ((CurrentFacing == Facing.RIGHT && forward) || (CurrentFacing == Facing.LEFT && !forward)) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }

        float startY = transform.position.y;
        float gravity = 0;
        if (_rigidbody) {
            gravity = _rigidbody.gravityScale;
            _rigidbody.gravityScale = 0;
        }

        transform.position += new Vector3(0, 1, 0);

        int direction = 1;
        if ((CurrentFacing == Facing.RIGHT && forward) || (CurrentFacing == Facing.LEFT && !forward)) {
            direction = 1;
        }
        else {
            direction = -1;
        }

        float timer = 0;
        if (_rigidbody != null) {
            _rigidbody.AddForce(transform.right * direction * 10000);
        }
        while (timer < duration) {
            timer += Time.deltaTime;
            if (!_rigidbody) transform.position += new Vector3(direction * driftSpeed * Time.deltaTime, 0, 0);

            if (timer >= duration / 4.0f && timer < duration / 2.0f) {
                if (forward) {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, -56));
                }
                else {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 56));
                }
            }
            if (timer >= duration / 2.0f && timer < (3 * duration) / 2.0f) {
                if (forward) {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, -67));
                }
                else {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 67));
                }
            }
            if (timer >= (3 * duration) / 2.0f) {
                if (forward) {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, -78));
                }
                else {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 78));
                }
            }

            yield return null;
        }

        if (killOnFinish) {
            yield return StartCoroutine(Kill(forward, driftSpeed));
        }
        else {
            //transform.position = new Vector3(transform.position.x, startY, transform.position.z);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Frozen = false;
            SetLayerRecursively(gameObject, currentMask);
            if (_rigidbody) {
                _rigidbody.gravityScale = gravity;
            }
        }

    }

    public IEnumerator Kill(bool forward, float forwardSpeed) {
        if (forward) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -85));
        }
        else {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 85));
        }

        OnKill();

        float accleration = 1f;
        float fallSpeed = 0;
        int direction = 1;
        if ((CurrentFacing == Facing.RIGHT && forward) || (CurrentFacing == Facing.LEFT && !forward)) {
            direction = 1;
        }
        else {
            direction = -1;
        }

        while (transform.position.y > DeathHeight) {
            transform.position += new Vector3(direction * forwardSpeed * Time.deltaTime, -1 * fallSpeed * Time.deltaTime, 0);
            fallSpeed += accleration;

            yield return null;
        }

        OnDeath();
    }

    public IEnumerator FreezeForSeconds(float time) {
        Frozen = true;
        yield return new WaitForSeconds(time);
        Frozen = false;
    }

    public IEnumerator InvulnerableForSeconds(float time) {
        Invulnerable = true;
        float timer = 0;
        Color realColor = _spriteRenderer.color;
        while (timer < time) {
            timer += Time.deltaTime;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, Mathf.Sin(timer * InvulnerableFlashSpeed)/2.0f + 0.5f, Mathf.Sin(timer * InvulnerableFlashSpeed) / 2.0f + 0.5f, 1);
            yield return null;
        }
        _spriteRenderer.color = realColor;
        Invulnerable = false;
    }

    public virtual void OnKill() {

    }

    public virtual void OnDeath() {
        gameObject.SetActive(false);
    }

    public void SetLayerRecursively(GameObject go, int layer) {
        go.layer = layer;
        foreach (Transform child in go.transform) {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
