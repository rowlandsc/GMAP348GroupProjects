using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour {

    public static CombatSystem Instance = null;
    public enum AttackType { ATTACK, QUICKATTACK, BLOCK, CHARGE, SPECIALATTACK };

    public delegate void dCombatStart();
    public delegate void dCombatRoundStart();
    public delegate void dCombatRoundEnd();
    public delegate void dCombatInputStart();
    public delegate void dCombatInputEnd();
    public delegate void dCombatPlayerKilled();
    public delegate void dCombatEnemyKilled();

    public static event dCombatStart OnCombatStart;
    public static event dCombatRoundStart OnCombatRoundStart;
    public static event dCombatRoundEnd OnCombatRoundEnd;
    public static event dCombatInputStart OnCombatInputStart;
    public static event dCombatInputEnd OnCombatInputEnd;
    public static event dCombatPlayerKilled OnCombatPlayerKilled;
    public static event dCombatEnemyKilled OnCombatEnemyKilled;

    public InputSeriesUI InputUI = null;
    public bool IsInCombat = false;
    public Enemy CurrentEnemy = null;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start () {
        InputManager.OnStartInputSeries += OnInputSeriesStart;
        InputManager.OnStartInputSeriesWindow += OnInputWindowStart;
        InputManager.OnEndInputSeriesWindow += OnInputWindowEnd;
        InputManager.OnEndInputSeries += OnInputSeriesEnd;

        OnCombatStart += CombatStart;
        OnCombatRoundStart += CombatRoundStart;
        OnCombatRoundEnd += CombatRoundEnd;
        OnCombatInputStart += CombatInputStart;
        OnCombatInputEnd += CombatInputEnd;
        OnCombatPlayerKilled += CombatPlayerKilled;
        OnCombatEnemyKilled += CombatEnemyKilled;
    }
	
	void Update () {
        if (Input.GetKeyUp(KeyCode.RightShift) && !InputManager.Instance.IsGettingInputString) {
            StartCoroutine(InputManager.Instance.GenerateNewInputSeries(5));
        }
    }

    void OnInputSeriesStart() {
        
    }
    void OnInputWindowStart() {

    }
    void OnInputWindowEnd() {
        int i = InputManager.Instance.InputString.Count - 1;
        string text = "";
        switch(InputManager.Instance.InputString[i]) {
            case InputManager.InputType.DOUBLETAP:
                text = "Quick Attack";
                break;
            case InputManager.InputType.HOLD:
                text = "Charge";
                break;
            case InputManager.InputType.NONE:
                text = "Block";
                break;
            case InputManager.InputType.PRESS:
                text = "Attack";
                break;
            case InputManager.InputType.RELEASE:
                text = "Special Attack";
                break;
        }

        InputUI.SetWindowText(InputManager.Instance.InputString.Count - 1, text);
    }
    void OnInputSeriesEnd() {
        
    }

    void CombatStart() {
        StartCoroutine(CombatFlow());

        
    }
    void CombatRoundStart() {

    }
    void CombatRoundEnd() {

    }
    void CombatInputStart() {

    }
    void CombatInputEnd() {

    }
    void CombatPlayerKilled() {

    }
    void CombatEnemyKilled() {

    }

    public void StartCombat(Enemy enemy) {
        CurrentEnemy = enemy;
        OnCombatStart();
    }

    IEnumerator CombatFlow() {
        IsInCombat = true;

        yield return StartCoroutine(InputManager.Instance.GenerateNewInputSeries(5));
        List<InputManager.InputType> playerInput = InputManager.Instance.InputString;
        List<AttackType> playerSeries = new List<AttackType>();
        for (int i=0; i < playerInput.Count; i++) {
            switch(playerInput[i]) {
                case InputManager.InputType.DOUBLETAP:
                    playerSeries.Add(AttackType.QUICKATTACK);
                    break;
                case InputManager.InputType.HOLD:
                    playerSeries.Add(AttackType.CHARGE);
                    break;
                case InputManager.InputType.NONE:
                    playerSeries.Add(AttackType.BLOCK);
                    break;
                case InputManager.InputType.PRESS:
                    playerSeries.Add(AttackType.ATTACK);
                    break;
                case InputManager.InputType.RELEASE:
                    playerSeries.Add(AttackType.SPECIALATTACK);
                    break;
            }
        }

        List<AttackType> enemySeries = CurrentEnemy.GetInputSeries(5);
    }
}
