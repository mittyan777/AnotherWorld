using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public Boss01ActionType currentState = Boss01ActionType.Idle;

    private BossAnimationManager animationManager;

    //考える時間(行動のインターバル)
    [SerializeField] private float thinkingTime;
    private float currentThinkingTime;

    [SerializeField] private GameObject playerObj;
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleTime; 
    private float currentTime;

    int TestCount=0;
    private void Start()
    {
        animationManager = GetComponent<BossAnimationManager>();
        DetermineNextAction();
    }
    private void Update()
    {
        if (currentState == Boss01ActionType.Tackle)
        {
            BossTackle();
        }

        if (currentState == Boss01ActionType.Idle)
        {
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
                return; // 行動が決まったら関数を終了
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
                break;
            case Boss01ActionType.StrongAttack:
                //攻撃アニメーションを開始し、攻撃中のロジックをコルーチンで実行
                break;
            case Boss01ActionType.Tackle:
                //Tackle固有のロジックを実行
                transform.LookAt(playerObj.transform);
                break;
            case Boss01ActionType.Walking:
                //Walking固有のロジックを実行
                break;
        }
    }

    //Bossの位置からプレイヤーがいる方向を取得し、その方向に向かって高速移動させる
    //壁に当たれば移動を辞める
    private void BossTackle() 
    {
        ChangeState(Boss01ActionType.Tackle);
        currentTime += Time.deltaTime;
        transform.position += transform.forward * tackleSpeed * Time.deltaTime;

        if (currentTime >= tackleTime)
        {
            ChangeState(Boss01ActionType.Idle);
            currentTime = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //壁に当たったら攻撃を中断(主にタックル)
        if (collision.gameObject.CompareTag("Wall"))
        {
            ChangeState(Boss01ActionType.Idle);
        }
    }
}
