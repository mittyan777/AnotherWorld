using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

//行動と確率をセットで保持
[System.Serializable]
public class ActionWeightData
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
    [SerializeField] private List<ActionWeightData> actionWeightsList;

    [HideInInspector] public Boss01ActionType currentState = Boss01ActionType.Idle;

    private BossAnimationManager animationManager;

    //考える時間(行動のインターバル)
    [SerializeField] private float thinkingTime;
    private float currentThinkingTime;

    //タックルに必要な変数
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleTime;
    private Vector3 tackleDirection;
    private float currentTime;

    //強攻撃と弱攻撃に必要なオブジェクト
    [SerializeField] private GameObject strongAttackArea;
    [SerializeField] private GameObject quickAttackArea;
    [SerializeField] private float quickAttackRange;
    [SerializeField] private float strongAttacRange;

    //追尾速度
    [SerializeField] private float chaseSpeed;

    private void Start()
    {
        animationManager = GetComponent<BossAnimationManager>();
        quickAttackArea.SetActive(false);
    }
    private void Update()
    {
        if (currentState == Boss01ActionType.Tackle)
        {
            BossTackle();
        }

        if (currentState == Boss01ActionType.QuickAttack ||
            currentState == Boss01ActionType.StrongAttack)
        {
            
        }

        if (currentState == Boss01ActionType.Idle)
        {
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
        /*
        //actionWeightsListからProbabilityWeightの値だけ抜き出し、新しいFlort型のリストを「ToList」で作成する
        //Select(a=>a.ProbabilityWeight)：Selectで抜き出すデータを指定する。
        //「a」は貰ったリスト(actionWeightsList)の仮の名前で、「a」のProbabilityWeightの値を選択する。
        List<float> weights = actionWeightsList.Select(a=>a.ProbabilityWeight).ToList();

        //BossManagerの関数を呼び出し、インデックス（何番目か）を取得
        int actionIndex = BossManager.instance.GetActionIndex(weights);

        if (actionIndex != -1)
        {
            //受け取ったインデックスをEnumに変換し、ステートを変更
            Boss01ActionType nextAction = (Boss01ActionType)actionIndex;

            ChangeState(nextAction);
        }
        */
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
    }

    private void ChangeState(Boss01ActionType newAction)
    {
        currentState = newAction;
        if (animationManager != null)
        {
            animationManager.UpdateAnimation(currentState);
        }

        switch (newAction)
        {
            case Boss01ActionType.Idle:
                //アニメーターのisIdleをTrueにする
                currentThinkingTime = 0;
                break;
            case Boss01ActionType.QuickAttack:
                //NavMeshAgentを起動し、プレイヤーを追跡する処理を実行
                StartCoroutine(QuickAttack());
                break;
            case Boss01ActionType.StrongAttack:
                //攻撃アニメーションを開始し、攻撃中のロジックをコルーチンで実行
                StartCoroutine(StrongAttack());
                break;
            case Boss01ActionType.Tackle:
                //Tackle固有のロジックを実行
                currentTime = 0;
                transform.LookAt(playerObj.transform);
                tackleDirection = transform.forward;
                break;
            case Boss01ActionType.Walking:
                //Walking固有のロジックを実行
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

        if (currentTime >= tackleTime)
        {
            ChangeState(Boss01ActionType.Idle);
            currentTime = 0;
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


    private void OnCollisionEnter(Collision collision)
    {
        //壁に当たったら攻撃を中断(主にタックル)
        if (collision.gameObject.CompareTag("syounin"))
        {
            ChangeState(Boss01ActionType.Idle);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("syounin"))
        {
            ChangeState(Boss01ActionType.Idle);
        }
    }
}
