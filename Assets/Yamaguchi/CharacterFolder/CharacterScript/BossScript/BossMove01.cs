using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//行動と確率をセットで保持
[System.Serializable]
public class Boss01ActionWeightData
{
    //どの行動を行うか）
    public BossMove01.Boss01ActionType ActionType;
    //確率
    public float ProbabilityWeight;
}

public class BossMove01 : MonoBehaviour
{
    public enum Boss01ActionType
    {
        Idle,
        QuickAttack,
        StrongAttack,
        Tackle,
        Walking,
        Death,
        None
    }

    //Bossの行動と、その行動確率を設定するデータリスト
    [SerializeField] private List<Boss01ActionWeightData> attackRangeActionList;
    [SerializeField] private List<Boss01ActionWeightData> farRangeActionList;

    [HideInInspector] public Boss01ActionType currentState = Boss01ActionType.Idle;

    private BossAnimationManager animationManager;

    //考える時間(行動のインターバル)
    [SerializeField] private float thinkingTime;
    private float currentThinkingTime;

    //タックルに必要な変数
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject tackleObj;
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleTime;
    private Vector3 tackleDirection;
    private float currentTime;

    //強攻撃と弱攻撃に必要なオブジェクト
    [SerializeField] private GameObject strongAttackArea;
    [SerializeField] private GameObject quickAttackArea;
    [SerializeField] private float attackRange;
    //追尾速度
    [SerializeField] private float chaseSpeed;



    private void Start()
    {
    }

    private void OnEnable() 
    {
        animationManager = GetComponent<BossAnimationManager>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        quickAttackArea.SetActive(false);
        tackleObj.SetActive(false);
        //確実な初期化処理をここに記述
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    private void Update()
    {
        /*
        if (BossMoveManager.instance.currentBossHP <= 0)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            Destroy(body);
            ChangeState(Boss01ActionType.Death);
            return;
        }
         */

        if (currentState == Boss01ActionType.Tackle)
        {
            BossTackle();
        }
        else if (currentState == Boss01ActionType.Walking)
        {
            Walking();
        }

        if (currentState == Boss01ActionType.Idle)
        {
            //傾きを修正
            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
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
        //【重要】BossManagerのインスタンスがnullでないことを最初に確認する
        if (BossMoveManager.instance == null)
        {
            Debug.LogError("BossMoveManager.instance が初期化されていません。Script Execution Order を確認してください。");
            ChangeState(Boss01ActionType.Idle);
            return;
        }

        // playerObjが未設定だと、ここでエラーになる可能性もあるためチェック（任意）
        if (playerObj == null)
        {
            Debug.LogError("playerObj が設定されていません。");
            ChangeState(Boss01ActionType.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, playerObj.transform.position);
        List<Boss01ActionWeightData> targetAction;

        // 距離に応じて抽選するリストを変える
        if (distance <= attackRange)
        {
            targetAction = attackRangeActionList;
        }
        else
        {
            targetAction = farRangeActionList;
        }

        //targetAction が null か Count が 0 の場合をチェック
        if (targetAction == null || targetAction.Count == 0)
        {
            // どちらかのリストが設定されていないか、要素がない場合
            Debug.LogWarning($"抽選リストが空または未設定です。距離: {distance}。Idleに戻ります。");
            ChangeState(Boss01ActionType.Idle);
            return;
        }

        List<float> weight = targetAction.Select(a => a.ProbabilityWeight).ToList();

        // ここで BossMoveManager.instance が null の場合、エラーが出る
        int actionIndex = BossMoveManager.instance.GetActionIndex(weight);

        if (actionIndex != -1)
        {
            // 抽選されたActionWeightDataからActionTypeを取得
            Boss01ActionType nextAction = targetAction[actionIndex].ActionType;

            ChangeState(nextAction);
            return;
        }

        // 抽選に失敗した場合
        ChangeState(Boss01ActionType.Idle);

        /*
        //各行動の確率を合算する
        float totalWeight = actionWeightsList.Sum(a => a.ProbabilityWeight);

        if (totalWeight <= 0)
        {
            // 重みが無い場合はIdleに戻るか、何もしない
            ChangeState(Boss01ActionType.Idle);
            return;
        }

        //ランダムで行動を選抜
        float drawValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        //actionWeightsListを直接ループする
        for (int i = 0; i < actionWeightsList.Count; i++)
        {
            //actionWeightsListのProbabilityWeight を使用
            currentWeight += actionWeightsList[i].ProbabilityWeight;

            if (drawValue < currentWeight)
            {
                //抽選されたActionTypeを取得
                Boss01ActionType nextAction = actionWeightsList[i].ActionType;

                ChangeState(nextAction);
                return; //行動が決まったら関数を終了
            }
        }

        // 全ての重みをチェックしても決まらない（通常は起こらない）場合は、Idleに戻す
        ChangeState(Boss01ActionType.Idle);
        */
    }

    public void ChangeState(Boss01ActionType newAction)
    {
        currentState = newAction;
        if (animationManager != null)
        {
            animationManager.Boss01UpdateAnimation(currentState);
        }

        switch (newAction)
        {
            case Boss01ActionType.Idle:
                //アニメーターのisIdleをTrueにする
                currentThinkingTime = 0;
                break;
            case Boss01ActionType.QuickAttack:
                //NavMeshAgentを起動し、プレイヤーを追跡する処理を実行
                LookAtPlayerHorizontal();
                StartCoroutine(QuickAttack());
                break;
            case Boss01ActionType.StrongAttack:
                //攻撃アニメーションを開始し、攻撃中のロジックをコルーチンで実行
                LookAtPlayerHorizontal();
                StartCoroutine(StrongAttack());
                break;
            case Boss01ActionType.Tackle:
                //Tackle固有のロジックを実行
                currentTime = 0;
                LookAtPlayerHorizontal();
                tackleDirection = transform.forward;
                break;
            case Boss01ActionType.Walking:
                //Walking固有のロジックを実行
                break;
            case Boss01ActionType.Death:
                tackleObj.SetActive(false);
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
        ChangeState(Boss01ActionType.Idle);
    }

    //Bossの位置からプレイヤーがいる方向を取得し、その方向に向かって高速移動させる
    //壁に当たれば移動を辞める
    private void BossTackle()
    {
        //ChangeState(Boss01ActionType.Tackle);
        currentTime += Time.deltaTime;
        transform.position += tackleDirection * tackleSpeed * Time.deltaTime;
        tackleObj.SetActive(true);
        if (currentTime >= tackleTime)
        {
            ChangeState(Boss01ActionType.Idle);
            currentTime = 0;
            tackleObj.SetActive(false);
        }
    }

    //強攻撃状態からidol状態に戻すのを待つ処理
    private IEnumerator StrongAttack()
    {
        //状態を戻すまでの時間指定
        float strongAttackDuration = 1.5f;
        yield return new WaitForSeconds(strongAttackDuration);
        ChangeState(Boss01ActionType.Idle);
    }

    public void ExecuteStrongAttackHit()
    {
        //アニメーションのタイミングで生成する
        Instantiate(strongAttackArea, transform.position, Quaternion.identity);
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
            ChangeState(Boss01ActionType.Idle);
        }
    }

    /*
    //タックル以外の攻撃が出た場合プレイヤーを追尾し、攻撃範囲内に入れば攻撃を行う
    private void Chase()
    {
        //プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, playerObj.transform.position);

        if (currentState == Boss01ActionType.QuickAttack)
        {
            //攻撃範囲内に入ったかチェック
            if (distance <= quickAttackRange)
            {
                return;
            }
            //範囲外ならプレイヤーに向き、移動する
            transform.LookAt(playerObj.transform.position);
            transform.position += transform.forward * chaseSpeed * Time.deltaTime;
        }
        else if (currentState == Boss01ActionType.StrongAttack) 
        {
            if (distance <= strongAttacRange)
            {
                return;
            }
            transform.LookAt(playerObj.transform.position);
            transform.position += transform.forward * chaseSpeed * Time.deltaTime;
        }

    }
    */

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

    private void OnCollisionEnter(Collision collision)
    {
        //壁に当たったら攻撃を中断(主にタックル)
        if (collision.gameObject.CompareTag("syounin"))
        {
            ChangeState(Boss01ActionType.Idle);
            tackleObj.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("syounin"))
        {
            ChangeState(Boss01ActionType.Idle);
            tackleObj.SetActive(false);
        }
    }
}
