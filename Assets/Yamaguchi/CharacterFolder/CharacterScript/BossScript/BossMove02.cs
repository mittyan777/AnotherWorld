using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Boss02ActionWeightData
{
    public BossMove02.Boss02ActionType ActionType;
    public float ProbabilityWeight;
}


public class BossMove02 : MonoBehaviour
{
    public enum Boss02ActionType
    {
        Idle,
        QuickAttack,
        StrongAttack,
        TwoStepAttack,
        Walking,
        Death,
        None
    }

    [SerializeField] private List<Boss02ActionWeightData> attackRangeActionList;
    [SerializeField] private List<Boss02ActionWeightData> farRangeActionList;

    [HideInInspector] public Boss02ActionType currentState = Boss02ActionType.Idle;

    private BossAnimationManager animationManager;

    //考える時間(行動のインターバル)
    [SerializeField] private float thinkingTime;
    private float currentThinkingTime;

    //不要な変数を削除: private float currentTime;

    //強攻撃と弱攻撃に必要なオブジェクト
    [SerializeField] private GameObject twoStepSlashAttackArea;
    [SerializeField] private GameObject strongSlashAttackArea;
    [SerializeField] private GameObject quickAttackArea;
    [SerializeField] private float attackRange;

    //追尾速度
    [SerializeField] private float chaseSpeed;

    // プレイヤーオブジェクト (BossMove01からコピー)
    [SerializeField] private GameObject playerObj;

    private void Start()
    {
        // BossAnimationManagerはBossMove01から流用している前提
        animationManager = GetComponent<BossAnimationManager>();
        quickAttackArea.SetActive(false);
        // PlayerObjの検索
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        //確実な初期化処理をここに記述
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

    private void Update()
    {
        /*
        // BossMoveManager.instance.currentBossHPはBossManagerが更新されている前提
        if (BossMoveManager.instance.currentBossHP <= 0)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null) Destroy(body); // Rigidbodyが存在する場合のみDestroy
            ChangeState(Boss02ActionType.Death);
            return;
        }
        */
        if (currentState == Boss02ActionType.Death) return;
        if (BossMoveManager.instance != null && BossMoveManager.instance.IsStunned)
        {
            return;
        }

        // JumpAttackの処理を追加する場合はここに記述
        if (currentState == Boss02ActionType.TwoStepAttack)
        {
            // TwoStepAttackの処理がまだ空
        }
        else if (currentState == Boss02ActionType.Walking)
        {
            Walking();
        }

        if (currentState == Boss02ActionType.Idle)
        {
            //傾きを修正
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            quickAttackArea.SetActive(false);
            currentThinkingTime += Time.deltaTime; // 考える時間を計測

            if (currentThinkingTime >= thinkingTime)
            {
                currentThinkingTime = 0f; // 時間リセット
                DetermineNextAction();  // 行動決定を呼び出す
            }
        }
    }

    public void DetermineNextAction()
    {
        if (BossMoveManager.instance == null)
        {
            Debug.LogError("BossMoveManager.instance が初期化されていません。Script Execution Order を確認してください。");
            ChangeState(Boss02ActionType.Idle);
            return;
        }

        if (playerObj == null)
        {
            Debug.LogError("playerObj が設定されていません。");
            ChangeState(Boss02ActionType.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, playerObj.transform.position);
        List<Boss02ActionWeightData> targetAction;

        // 距離に応じて抽選するリストを変える
        if (distance <= attackRange)
        {
            targetAction = attackRangeActionList;
        }
        else
        {
            targetAction = farRangeActionList;
        }

        if (targetAction == null || targetAction.Count == 0)
        {
            Debug.LogWarning($"抽選リストが空または未設定です。距離: {distance}。Idleに戻ります。");
            ChangeState(Boss02ActionType.Idle);
            return;
        }

        List<float> weight = targetAction.Select(a => a.ProbabilityWeight).ToList();

        // BossManagerの抽選関数を使用
        int actionIndex = BossMoveManager.instance.GetActionIndex(weight);

        if (actionIndex != -1)
        {
            // 抽選されたActionWeightDataからActionTypeを取得
            Boss02ActionType nextAction = targetAction[actionIndex].ActionType;

            ChangeState(nextAction);
            return;
        }

        // 抽選に失敗した場合
        ChangeState(Boss02ActionType.Idle);
    }

    //外部からもアニメーションを変えれるようにする
    public void ChangeState(Boss02ActionType newAction)
    {
        currentState = newAction;

        // BossAnimationManagerがBoss02ActionTypeに対応している必要があります
        if (animationManager != null)
        {
            animationManager.Boss02UpdateAnimation((BossMove02.Boss02ActionType)(int)currentState);
        }

        switch (newAction)
        {
            case Boss02ActionType.Idle:
                currentThinkingTime = 0;
                break;
            case Boss02ActionType.QuickAttack:
                LookAtPlayerHorizontal();
                StartCoroutine(QuickAttack());
                break;
            case Boss02ActionType.StrongAttack:
                LookAtPlayerHorizontal();
                StartCoroutine(StrongAttack());
                break;
            case Boss02ActionType.TwoStepAttack:
                LookAtPlayerHorizontal();
                StartCoroutine(TwoStepAttack());

                break;
            case Boss02ActionType.Walking:
                break;
        }
    }

    private IEnumerator QuickAttack()
    {
        float quickAttackDuration = 0.5f;
        float waitTime = 0.2f;
        yield return new WaitForSeconds(waitTime);
        quickAttackArea.SetActive(true);
        yield return new WaitForSeconds(quickAttackDuration);
        ChangeState(Boss02ActionType.Idle);
    }

    private IEnumerator StrongAttack()
    {
        //float strongAttackDuration = 1.5f;
        float strongAttackDuration = 1.5f;
        yield return new WaitForSeconds(strongAttackDuration);
        ChangeState(Boss02ActionType.Idle);
    }

    private IEnumerator TwoStepAttack()
    {
        //float strongAttackDuration = 0.5f;
        float strongAttackDuration = 2.0f;
        yield return new WaitForSeconds(strongAttackDuration);
        ChangeState(Boss02ActionType.Idle);
    }

    public void TwoStepSlash()
    {
        float yOffset = 1.5f;
        float rotationZ = 30f;
        Vector3 spawnPos = transform.position;
        // オフセットとして使いたい場合は spawnPos.y += yOffset; に修正が必要です。
        spawnPos.y += yOffset;
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, rotationZ);
        Instantiate(twoStepSlashAttackArea, spawnPos, rotation);
    }
    public void StrongSlash() 
    {
        float yOffset = 1.75f;
        float rotationZ = 30f;
        Vector3 spawnPos = transform.position;
        spawnPos.y += yOffset;
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, rotationZ);
        Instantiate(strongSlashAttackArea, spawnPos, rotation);
    }

    private void Walking()
    {
        //プレイヤーの方向を向く
        LookAtPlayerHorizontal();

        //設定された速度で移動
        transform.position += transform.forward * chaseSpeed * Time.deltaTime;

        //攻撃範囲に入ったかチェック
        float distance = Vector3.Distance(transform.position, playerObj.transform.position);
        if (distance <= attackRange)
        {
            // 攻撃範囲内に入ったらIdleに戻り、次の行動抽選を促す
            ChangeState(Boss02ActionType.Idle);
        }
    }

    private void LookAtPlayerHorizontal()
    {
        if (playerObj == null) return;

        //プレイヤーの水平位置を計算
        Vector3 targetPosition = new Vector3(
            playerObj.transform.position.x,
            transform.position.y, //ボスのY座標を維持する
            playerObj.transform.position.z
        );

        //その水平位置に向かって回転
        transform.LookAt(targetPosition);
    }
}