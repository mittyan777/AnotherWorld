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
    [SerializeField] private CommandoConfigSO _comconSO;
    [SerializeField] private AnimationFlagManagerSO _animFlgSO;
    Animator animator;

    //ジョブタイプ
    public JobType jobType { get; private set; } = JobType.NONE;

    //職種に対応したコマンドセット
    private Dictionary<JobType, CommandoConfigSO.CommandSet> CommandMap;

    //ゲームマネージャー
    //[SerializeField] GameObject[] manager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。", this);
            return;
        }

        if (!_comconSO) 
        {
            Debug.LogError("コマンドコンフィグが設定されていない", this);            
            return;
        }

        CommandMap = _comconSO.commandSets.ToDictionary(set => set.JobType, set => set);

    }

    private void OnEnable()
    {
        //チェック
        if (_animFlgSO) 
        {
            //_animFlgSO.AttackNormal += AttackAnimation_Normal()
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

        }
        else 
        {
            Debug.LogError("_animFlgSOが存在しない", this);
            return;
        }
    }

    private void AttackAnimation_Normal() 
    {

    }

}
