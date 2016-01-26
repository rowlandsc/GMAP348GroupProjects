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

    public float CombatInputWaitTime = 0.5f;
    public float CombatInputResultsTime = 1.0f;

    public float PlayerDeathFadeRedTime = 2.0f;
    public float PlayerDeathFadeSkeletonTime = 2.0f;
    public float GameOverWaitTime = 1.0f;
    public Sprite SkeletonSprite;

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
        PlayerCombatText.text = "";
        EnemyCombatText.text = "";
    }
    void CombatPlayerKilled() {
        StartCoroutine(PlayerDeath());
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
        yield return new WaitForSeconds(CombatInputWaitTime);

        AttackType playerAttack = _playerSeries[input];
        AttackType enemyAttack = _enemySeries[input];

        float playerReceivedQuickDamage = 0;
        float enemyReceivedQuickDamage = 0;
        float playerReceivedDamage = 0;
        float enemyReceivedDamage = 0;
        bool playerReceivedStun = false;
        bool enemyReceivedStun = false;
        bool playerAttacked = false;
        bool enemyAttacked = false;

        switch (playerAttack) {
            case AttackType.ATTACK:
                switch (enemyAttack) {
                    case AttackType.ATTACK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetAttackDamage();
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetAttackDamage();
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.BLOCK:
                        playerAttacked = true;
                        break;
                    case AttackType.CHARGE:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetAttackDamage();
                        playerAttacked = true;
                        break;
                    case AttackType.QUICKATTACK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetAttackDamage();
                        playerReceivedQuickDamage = CurrentEnemy.GetComponent<Stats>().GetQuickAttackDamage();
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.SPECIALATTACK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetAttackDamage();
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetSpecialAttackDamage(GetSpecialAttackCharge(false, input));
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    default:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetAttackDamage();
                        playerAttacked = true;
                        break;
                }
                break;
            case AttackType.BLOCK:
                switch (enemyAttack) {
                    case AttackType.ATTACK:
                        enemyAttacked = true;
                        break;
                    case AttackType.BLOCK:
                        break;
                    case AttackType.CHARGE:
                        break;
                    case AttackType.QUICKATTACK:
                        enemyReceivedStun = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.SPECIALATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetSpecialAttackDamage(GetSpecialAttackCharge(false, input));
                        enemyAttacked = true;
                        break;
                    default:
                        break;
                }
                break;
            case AttackType.CHARGE:
                switch (enemyAttack) {
                    case AttackType.ATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetAttackDamage();
                        enemyAttacked = true;
                        break;
                    case AttackType.BLOCK:
                        break;
                    case AttackType.CHARGE:
                        break;
                    case AttackType.QUICKATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetQuickAttackDamage();
                        enemyAttacked = true;
                        break;
                    case AttackType.SPECIALATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetSpecialAttackDamage(GetSpecialAttackCharge(false, input));
                        enemyAttacked = true;
                        break;
                    default:
                        break;
                }
                break;
            case AttackType.QUICKATTACK:
                switch (enemyAttack) {
                    case AttackType.ATTACK:
                        enemyReceivedQuickDamage = Player.Instance.GetComponent<Stats>().GetQuickAttackDamage();
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetAttackDamage();
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.BLOCK:
                        playerReceivedStun = true;
                        playerAttacked = true;
                        break;
                    case AttackType.CHARGE:
                        enemyReceivedQuickDamage = Player.Instance.GetComponent<Stats>().GetQuickAttackDamage();
                        playerAttacked = true;
                        break;
                    case AttackType.QUICKATTACK:
                        enemyReceivedQuickDamage = Player.Instance.GetComponent<Stats>().GetQuickAttackDamage();
                        playerReceivedQuickDamage = CurrentEnemy.GetComponent<Stats>().GetQuickAttackDamage();
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.SPECIALATTACK:
                        enemyReceivedQuickDamage = Player.Instance.GetComponent<Stats>().GetQuickAttackDamage();
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetSpecialAttackDamage(GetSpecialAttackCharge(false, input));
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    default:
                        enemyReceivedQuickDamage = Player.Instance.GetComponent<Stats>().GetQuickAttackDamage();
                        playerAttacked = true;
                        break;
                }
                break;
            case AttackType.SPECIALATTACK:
                int playerSpecialCharge = GetSpecialAttackCharge(true, input);
                switch (enemyAttack) {
                    case AttackType.ATTACK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetSpecialAttackDamage(playerSpecialCharge);
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetAttackDamage();
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.BLOCK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetSpecialAttackDamage(playerSpecialCharge); ;
                        playerAttacked = true;
                        break;
                    case AttackType.CHARGE:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetSpecialAttackDamage(playerSpecialCharge); ;
                        playerAttacked = true;
                        break;
                    case AttackType.QUICKATTACK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetSpecialAttackDamage(playerSpecialCharge); ;
                        playerReceivedQuickDamage = CurrentEnemy.GetComponent<Stats>().GetQuickAttackDamage();
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    case AttackType.SPECIALATTACK:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetSpecialAttackDamage(playerSpecialCharge); ;
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetSpecialAttackDamage(GetSpecialAttackCharge(false, input));
                        playerAttacked = true;
                        enemyAttacked = true;
                        break;
                    default:
                        enemyReceivedDamage = Player.Instance.GetComponent<Stats>().GetSpecialAttackDamage(playerSpecialCharge); ;
                        playerAttacked = true;
                        break;
                }
                break;
            default:
                switch (enemyAttack) {
                    case AttackType.ATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetAttackDamage();
                        enemyAttacked = true;
                        break;
                    case AttackType.BLOCK:
                        break;
                    case AttackType.CHARGE:
                        break;
                    case AttackType.QUICKATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetQuickAttackDamage();
                        enemyAttacked = true;
                        break;
                    case AttackType.SPECIALATTACK:
                        playerReceivedDamage = CurrentEnemy.GetComponent<Stats>().GetSpecialAttackDamage(GetSpecialAttackCharge(false, input));
                        enemyAttacked = true;
                        break;
                    default:
                        break;
                }
                break;
        }

        bool wait = false;
        if (playerAttack == AttackType.QUICKATTACK) {
            CombatAnimationManager.Instance.CreateCombatAnimationObject(CombatInputResultsTime, enemyReceivedQuickDamage.ToString(), false, Color.red, true);
            CurrentEnemy.GetComponent<Stats>().DealDamage(enemyReceivedQuickDamage);
            wait = true;
        }
        if (enemyAttack == AttackType.QUICKATTACK) {
            CombatAnimationManager.Instance.CreateCombatAnimationObject(CombatInputResultsTime, playerReceivedQuickDamage.ToString(), false, Color.red, true);
            Player.Instance.GetComponent<Stats>().DealDamage(playerReceivedQuickDamage);
            wait = true;
        }

        if (!Player.Instance.GetComponent<Stats>().GetIsAlive() || !CurrentEnemy.GetComponent<Stats>().GetIsAlive()) {
            yield return new WaitForSeconds(CombatInputResultsTime);
            yield break;
        }

        if (wait && ((playerAttack == AttackType.QUICKATTACK && enemyAttacked && enemyAttack != AttackType.QUICKATTACK) || (playerAttack != AttackType.QUICKATTACK && playerAttacked && enemyAttack == AttackType.QUICKATTACK))) {
            yield return new WaitForSeconds(CombatInputResultsTime / 2);
        }

        wait = false;
        if (playerAttacked && playerAttack != AttackType.QUICKATTACK) {
            CombatAnimationManager.Instance.CreateCombatAnimationObject(CombatInputResultsTime, enemyReceivedDamage.ToString(), false, Color.red, true);
            CurrentEnemy.GetComponent<Stats>().DealDamage(enemyReceivedDamage);
            wait = true;
        }
        if (enemyAttacked && enemyAttack != AttackType.QUICKATTACK) {
            CombatAnimationManager.Instance.CreateCombatAnimationObject(CombatInputResultsTime, playerReceivedDamage.ToString(), true, Color.red, true);
            Player.Instance.GetComponent<Stats>().DealDamage(playerReceivedDamage);
            wait = true;
        }

        if (wait && (playerReceivedStun || enemyReceivedStun)) {
            yield return new WaitForSeconds(CombatInputResultsTime / 2);
        }

        if (playerReceivedStun) {
            CombatAnimationManager.Instance.CreateCombatAnimationObject(CombatInputResultsTime, "Stun", true, Color.red, true);
            Stun(true, input + 1);            
        }
        if (enemyReceivedStun) {
            CombatAnimationManager.Instance.CreateCombatAnimationObject(CombatInputResultsTime, "Stun", false, Color.red, true);
            Stun(false, input + 1);
        }

        yield return new WaitForSeconds(CombatInputResultsTime);
    }


    int GetSpecialAttackCharge(bool player, int specialAttackInput) {
        if (player) {
            int charge = 0;
            for (int i=specialAttackInput - 1; i>=0; i--) {
                if (_playerSeries[i] == AttackType.CHARGE) charge++;
                else break;
            }

            return charge;
        }
        else {
            int charge = 0;
            for (int i = specialAttackInput - 1; i >= 0; i--) {
                if (_enemySeries[i] == AttackType.CHARGE) charge++;
                else break;
            }

            return charge;
        }
    }
   
    void Stun(bool player, int inputToStun) {
        if ((player && inputToStun >= _playerSeries.Count) || (!player && inputToStun >= _enemySeries.Count)) {
            return;
        }

        if (player) {
            _playerSeries[inputToStun] = AttackType.NONE;

            string text = GetAttackName(_playerSeries[inputToStun]);

            InputUI.SetWindowText(inputToStun, text);
        }
        else {
            _enemySeries[inputToStun] = AttackType.NONE;
        }
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

    IEnumerator PlayerDeath() {
        SpriteRenderer sr = Player.Instance.PlayerSprite;

        float _timer = 0;
        while (_timer < PlayerDeathFadeRedTime) {
            _timer += Time.deltaTime;
            sr.color = new Color(1, sr.color.g - Time.deltaTime / PlayerDeathFadeRedTime, sr.color.b - Time.deltaTime / PlayerDeathFadeRedTime);
            yield return null;
        }

        _timer = 0;
        GameObject deathSkeletonGO = new GameObject("Death Skeleton");
        deathSkeletonGO.transform.SetParent(Player.Instance.transform);
        deathSkeletonGO.transform.localPosition = Vector3.zero;
        deathSkeletonGO.transform.localScale = new Vector3(4, 2, 1);
        SpriteRenderer skeletonSR = deathSkeletonGO.AddComponent<SpriteRenderer>();
        skeletonSR.sprite = SkeletonSprite;
        skeletonSR.color = new Color(1, 1, 1, 0);
        while (_timer < PlayerDeathFadeSkeletonTime) {
            _timer += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - Time.deltaTime / PlayerDeathFadeSkeletonTime);
            skeletonSR.color = new Color(1, 1, 1, skeletonSR.color.a + Time.deltaTime / PlayerDeathFadeSkeletonTime);
            yield return null;
        }

        yield return new WaitForSeconds(GameOverWaitTime);
        Application.LoadLevel("GameOver");
    }

    void OnDestroy() {
        OnCombatStart -= CombatStart;
        OnCombatRoundStart -= CombatRoundStart;
        OnCombatRoundEnd -= CombatRoundEnd;
        OnCombatInputStart -= CombatInputStart;
        OnCombatInputEnd -= CombatInputEnd;
        OnCombatPlayerKilled -= CombatPlayerKilled;
        OnCombatEnemyKilled -= CombatEnemyKilled;
    }
}
