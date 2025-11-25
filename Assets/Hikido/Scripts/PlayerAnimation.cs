using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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

    [SerializeField] private Animator animator;

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
            UnityEngine.Debug.LogError("Animatorコンポーネントが見つかりません。", this);
            return;
        }

        if (!_cmdCofigSO)
        {
            UnityEngine.Debug.LogError("コマンドコンフィグが設定されていない", this);
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
            _animFlgSO.AttackArcherSkills += AttackAnimation_Archer;

            //回避用イベント登録
            _animFlgSO.AvoidanceEvents += AvoidAnim;
        }
        else
        {
            UnityEngine.Debug.LogError("_animFlgSOが存在しない", this);
            return;
        }
    }

    private void OnDisable()
    {
        if (_animFlgSO)
        {
            //各アニメーション処理をActionから削除
            _animFlgSO.AttackNormal -= AttackAnimation_Normal;
            _animFlgSO.AttackArcherSkills -= AttackAnimation_Archer;

            //回避用イベント解除
            _animFlgSO.AvoidanceEvents -= AvoidAnim;
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
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _currentCmd = commandSet.normalAttackCd;
            if (_currentCmd != null) { _currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"ジョブ:{jobType} のコマンドが設定されていない。"); }
        }
        else { UnityEngine.Debug.LogError($"ジョブ設定ミス{jobType}"); }
    }

    /// <summary> /// 攻撃アニメーション終了用関数 /// </summary>
    public void AttackAnimation_NormalEnd()
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _endCmd = commandSet.normalAttackEndCd;
            if (_endCmd != null) { _endCmd.Execute(animator); }
            else { UnityEngine.Debug.LogError("失敗"); }
        }
    }


    /// <summary> /// 回避アニメーション /// </summary>
    public void AvoidAnim() 
    {
        if(CommandMap.TryGetValue(jobType,out var commandSet)) 
        {
            AnimationBaseSO _avoidCmd = commandSet.avoidCd;
            if(_avoidCmd != null) { _avoidCmd.Execute(animator); }
            else { UnityEngine.Debug.Log("回避アニメーション失敗。"); }
        }
    }

    /// <summary> /// 回避アニメーション終了コマンド /// </summary>
    public void AvodAnimationEnd() 
    {
        if(CommandMap.TryGetValue(jobType,out var commandSet)) 
        {
            AnimationBaseSO _avoidEndCmd = commandSet.avoidAnimEndCd;
            if(_avoidEndCmd != null) { _avoidEndCmd.Execute(animator); }
            else { UnityEngine.Debug.Log("回避アニメーション終了失敗"); }
        }
    }

    //TODO:アーチャースキルのスクリプトが存在しないのでマージ後テスト
    //      ->現状はプレイヤーのソードスキルとして考えてテストする。
    /// <summary> /// アーチャースキルアニメーション /// </summary>
    public void AttackAnimation_Archer() 
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _currentCmd = commandSet.archerSkilsCd;
            if (_currentCmd != null) { _currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"ジョブ:{jobType} のコマンドが設定されていない。"); }
        }
        else { UnityEngine.Debug.LogError("ジョブ設定ミス"); }
    }


}
