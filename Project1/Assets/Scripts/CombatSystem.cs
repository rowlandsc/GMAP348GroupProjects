using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour {

    public static CombatSystem Instance = null;
    public enum AttackType { NONE, ATTACK, QUICKATTACK, BLOCK, CHARGE, SPECIALATTACK };

    public delegate void dCombatStart();
    public delegate void dCombatRoundStart();
    public delegate void dCombatRoundEnd();
    public delegate void dCombatInputStart(int inputNumber);
    public delegate void dCombatInputEnd(int inputNumber);
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
    public Text PlayerCombatText;
    public Text EnemyCombatText;

    List<AttackType> _playerSeries = new List<AttackType>();
    List<AttackType> _enemySeries = new List<AttackType>();

    bool _inRound = false;

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
        string text = GetAttackName(GetAttackType(InputManager.Instance.InputString[i]));

        InputUI.SetWindowText(InputManager.Instance.InputString.Count - 1, text);
    }
    void OnInputSeriesEnd() {
        
    }

    void CombatStart() {
        StartCoroutine(CombatFlow());

        
    }
    void CombatRoundStart() {
        StartCoroutine(CombatRoundFlow());
    }
    void CombatRoundEnd() {

    }
    void CombatInputStart(int inputNumber) {
        PlayerCombatText.text = GetAttackName(_playerSeries[inputNumber]);
        EnemyCombatText.text = GetAttackName(_enemySeries[inputNumber]);
    }
    void CombatInputEnd(int inputNumber) {

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

        while (Player.Instance.GetComponent<Stats>().GetIsAlive() && CurrentEnemy.GetComponent<Stats>().GetIsAlive()) {
            yield return StartCoroutine(InputManager.Instance.GenerateNewInputSeries(5));
            List<InputManager.InputType> playerInput = InputManager.Instance.InputString;
            _playerSeries.Clear();
            for (int i = 0; i < playerInput.Count; i++) {
                _playerSeries.Add(GetAttackType(playerInput[i]));
            }

            Debug.Log("Got input string");
            _enemySeries = CurrentEnemy.GetInputSeries(5);

            OnCombatRoundStart();
            while (_inRound) {
                yield return null;
            }
            OnCombatRoundEnd(); 
        }

        if (!Player.Instance.GetComponent<Stats>().GetIsAlive()) {
            OnCombatPlayerKilled();
        }
        else {
            OnCombatEnemyKilled();
        }
    }

    IEnumerator CombatRoundFlow() {
        _inRound = true;

        int max = Mathf.Max(_playerSeries.Count, _enemySeries.Count);
        for (int i=_playerSeries.Count; i<max; i++) {
            _playerSeries.Add(AttackType.NONE);
        }
        for (int i=_enemySeries.Count; i<max; i++) {
            _enemySeries.Add(AttackType.NONE);
        }

        int input = 0;
        while (input < _playerSeries.Count) {
            OnCombatInputStart(input);
            yield return StartCoroutine(CombatInputFlow(input));
            OnCombatInputEnd(input);
            input++;
        }

        _inRound = false;
    }

    IEnumerator CombatInputFlow(int input) {
        yield return new WaitForSeconds(1.0f);
    }

    AttackType GetAttackType(InputManager.InputType type) {
        switch (type) {
            case InputManager.InputType.DOUBLETAP:
                return AttackType.QUICKATTACK;
            case InputManager.InputType.HOLD:
                return AttackType.CHARGE;
            case InputManager.InputType.NONE:
                return AttackType.BLOCK;
            case InputManager.InputType.PRESS:
                return AttackType.ATTACK;
            case InputManager.InputType.RELEASE:
                return AttackType.SPECIALATTACK;
            default:
                return AttackType.NONE;
        }
    }

    string GetAttackName(AttackType attack) {
        switch (attack) {
            case AttackType.QUICKATTACK:
                return "Quick Attack";
            case AttackType.CHARGE:
                return "Charge";
            case AttackType.BLOCK:
                return "Block";
            case AttackType.ATTACK:
                return "Attack";
            case AttackType.SPECIALATTACK:
                return "Special Attack";
            default:
                return "None";
        }
    }
}
