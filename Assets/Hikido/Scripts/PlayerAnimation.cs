using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// ジョブタイプごとにセットするアニメーションコマンドを変更
public enum JobType
{
    NONE,         //使用しない枠 = 0
    SWORDSMAN,    //剣士  = 1
    ARCHER,       //弓使い(アーチャー) = 2
    WIZARD        //魔法使い = 3
}

public class PlayerAnimation : MonoBehaviour
{
    
    [SerializeField] private CommandoConfigSO _cmdCofigSO;
    [SerializeField] private AnimationFlagManagerSO _animFlgSO;

    [SerializeField] private  Animator animator;

    //ジョブタイプ
    public JobType jobType { get; private set; } = JobType.NONE;

    //職種に対応したコマンドセット
    private Dictionary<JobType, CommandoConfigSO.CommandSet> CommandMap;

    //ゲームマネージャー
    //[SerializeField] GameObject[] manager;

    private void Awake()
    {
        if (!animator)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。", this);
            return;
        }

        if (!_cmdCofigSO) 
        {
            Debug.LogError("コマンドコンフィグが設定されていない", this);            
            return;
        }

        CommandMap = _cmdCofigSO.commandSets.ToDictionary(set => set.JobType, set => set);

    }
    private void OnEnable()
    {
        //チェック
        if (_animFlgSO)
        {
            //各アニメーションの処理をActionに登録
            _animFlgSO.AttackNormal += AttackAnimation_Normal;
        }
        else
        {
            Debug.LogError("_animFlgSOが存在しない", this);
            return;
        }
    }

    private void OnDisable()
    {
        if (_animFlgSO)
        {
            //各アニメーション処理をActionから削除
            _animFlgSO.AttackNormal -= AttackAnimation_Normal;
        }
    }

    /// <summary> /// ジョブ設定(アニメーション用) /// </summary>
    /// <param name="_newJob"></param>
    public void SetJobType(JobType _newJob) 
    {
        jobType = _newJob;
    }

    /// <summary> /// 通常攻撃アニメーション /// </summary>
    private void AttackAnimation_Normal() 
    {
        //攻撃のアニメーション発火
        if(CommandMap.TryGetValue(jobType,out var commandSet)) 
        {
            AnimationBaseSO _currentCmd = commandSet.normalAttackCd;
            if (!_currentCmd) { _currentCmd.Execute(animator); }
            else { Debug.LogWarning($"ジョブ:{jobType} のコマンドが設定されていない。"); }
        }
        else { Debug.LogError($"ジョブ設定ミス{jobType}"); }
    }

}
