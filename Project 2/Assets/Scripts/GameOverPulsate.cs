using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameOverPulsate : MonoBehaviour {

    public float Variance = 0.2f;
    public float RotateAngle = 15;
    public float RotateTime = 2.0f;

	void OnEnable() {
        StartCoroutine(RotationCoroutine());

    }

    IEnumerator RotationCoroutine() {

        yield return transform.DORotate(new Vector3(0, 0, -1 * RotateAngle * (1 + Random.Range(-1 * Variance, Variance))), RotateTime / 2 * (1 + Random.Range(-1 * Variance, Variance))).WaitForCompletion();
        while (enabled) {
            yield return transform.DORotate(new Vector3(0, 0, RotateAngle * (1 + Random.Range(-1 * Variance, Variance))), RotateTime * (1 + Random.Range(-1 * Variance, Variance))).WaitForCompletion();
            yield return transform.DORotate(new Vector3(0, 0, -1 * RotateAngle * (1 + Random.Range(-1 * Variance, Variance))), RotateTime * (1 + Random.Range(-1 * Variance, Variance))).WaitForCompletion();
        }
    }
}
