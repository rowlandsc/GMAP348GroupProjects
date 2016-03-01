using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScreenShake : MonoBehaviour {

    public static ScreenShake Instance = null;

    public float Duration = 1.0f;
    public float Strength = 1.0f;
    public int Vibrato = 1;
    public float Randomness = 1.0f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }
	
    public void ShakeCamera() {
        ShakeCamera(Duration, Strength, Vibrato, Randomness);
    }

    public void ShakeCamera(float duration, float strength, int vibrato, float randomness) {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }
}
