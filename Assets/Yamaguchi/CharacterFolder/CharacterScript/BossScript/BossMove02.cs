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
        JumpAttack,
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

    private float currentTime;


    //強攻撃と弱攻撃に必要なオブジェクト
    [SerializeField] private GameObject slashAttackArea;
    [SerializeField] private GameObject quickAttackArea;
    [SerializeField]private GameObject jumpAttackArea;
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

        // スケール設定 (以前のディスカッションに基づき残す)
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    private void Update()
    {
        // BossManager.instance.currentBossHPはBossManagerが更新されている前提
        if (BossManager.instance.currentBossHP <= 0)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null) Destroy(body); // Rigidbodyが存在する場合のみDestroy
            ChangeState(Boss02ActionType.Death);
            return;
        }

        // JumpAttackの処理を追加する場合はここに記述
        if (currentState == Boss02ActionType.JumpAttack)
        {
            JumpAttack();
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
                DetermineNextAction();  // 行動決定を呼び出す
            }
        }
    }

    public void DetermineNextAction()
    {
        if (BossManager.instance == null)
        {
            Debug.LogError("BossManager.instance が初期化されていません。Script Execution Order を確認してください。");
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
        int actionIndex = BossManager.instance.GetActionIndex(weight);

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

    private void ChangeState(Boss02ActionType newAction)
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
            case Boss02ActionType.JumpAttack:
                LookAtPlayerHorizontal();
                StartCoroutine(JumpAttack()); // JumpAttackのコルーチンを呼び出す
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

    // JumpAttackのロジック (Tackleの代わりに新規作成)
    private IEnumerator JumpAttack()
    {
        float jumpAttackTime = 2.0f;
        yield return new WaitForSeconds(jumpAttackTime);
        ChangeState(Boss02ActionType.Idle);
    }

    private IEnumerator StrongAttack()
    {
        float strongAttackDuration = 1.5f;
        yield return new WaitForSeconds(strongAttackDuration);
        ChangeState(Boss02ActionType.Idle);
    }

    public void GenerateSlash() 
    {
        Instantiate(slashAttackArea, transform.position, Quaternion.identity);
    }

    public void GenerateJumpAttackArea()
    {
        Instantiate(jumpAttackArea, transform.position, Quaternion.identity);
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
