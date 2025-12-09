using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Windows;

/// ジョブタイプごとにセットするアニメーションコマンドを変更
public enum JobType
{
    SWORDSMAN,    //剣士  = 0
    ARCHER,       //弓使い(アーチャー) = 1
    WIZARD,        //魔法使い = 2

    NONE
}

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField] private CommandoConfigSO _cmdCofigSO;
    [SerializeField] private AnimationFlagManagerSO _animFlgSO;

    //TODO；テスト時のみhikido使用
    [SerializeField] GameManager_hikido _gameManager;
    //[SerializeField] GameManager _gameManager;

    [SerializeField] private Animator animator;
    [SerializeField] Player_hikido1 _player;  //プレイヤーhikido
    [SerializeField] private Transform _cameraTransform;
    private Transform _playerTransform;
    private bool _isCurrentlyAiming = false;
    private bool _shootInputConfirmed = false;

    [SerializeField] private SwordSkillAnime _swordSkillCmd;
    [SerializeField] Player_Swoad _plSwardLogic;

    //ジョブタイプ
    public JobType jobType { get; private set; } = JobType.NONE;

    //職種に対応したコマンドセット
    private Dictionary<JobType, CommandoConfigSO.CommandSet> CommandMap;

    private int _currentjobNum = -1;
    private void Start()
    {
        if (_player != null) { _playerTransform = _player.transform; }

        if (_gameManager == null)
        {
            UnityEngine.Debug.LogError("GameManagerが設定されていません。ジョブ設定ができません。", this);
            return;
        }

        _currentjobNum = (int)_gameManager.job;
        SetJobType((JobType)_currentjobNum);
        _plSwardLogic.GetComponent<Player_Swoad>();

        UnityEngine.Debug.Log($"初期ジョブタイプ設定完了: {jobType}");
    }

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

        if (_gameManager != null)
        {
            int jobNum = (int)_gameManager.job;
            SetJobType((JobType)jobNum);

            UnityEngine.Debug.Log($"初期ジョブタイプ: {jobType}");
        }

        CommandMap = _cmdCofigSO.commandSets.ToDictionary(set => set.JobType, set => set);

    }

    private void Update()
    {
        if (_gameManager == null) return;

        int jobFromManager = (int)_gameManager.job;
        if (jobFromManager != _currentjobNum)
        {
            _currentjobNum = jobFromManager;
            SetJobType((JobType)_currentjobNum);

            UnityEngine.Debug.Log($"ジョブが更新されました！ JobType: {jobType}");
        }

        if(_player.isAiming && UnityEngine.Input.GetMouseButtonUp(0)) 
        {
            _shootInputConfirmed = true;
        }
    }


    private void FixedUpdate()
    {
        Vector2 moveInput = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));

        bool isAiming = _player.isAiming;
        bool isArcher = (jobType == JobType.ARCHER);

        //ADSが始まった最初のフレーム値を検出
        bool isAimingStartFrame = (isAiming && isArcher && !_isCurrentlyAiming);

        //マウスボタン離しで発射
        bool isShootRequested = _shootInputConfirmed;

        int adsLayerIndex = animator.GetLayerIndex("ADS Layer");

        //ADS時の処理
        if (isAiming && isArcher)
        {
            _isCurrentlyAiming = true;
            if (adsLayerIndex != -1) { animator.SetLayerWeight(adsLayerIndex, 1f); }

            if (_playerTransform != null && _cameraTransform != null)
            {
                float targetYAngle = _cameraTransform.eulerAngles.y - 90f;

                Quaternion targetRotation = Quaternion.Euler(
                    0f,
                    targetYAngle,
                    0f
                );
                _playerTransform.rotation = Quaternion.Slerp(
                    _playerTransform.rotation,
                    targetRotation,
                    Time.fixedDeltaTime * 10f
                );
            }

            if (CommandMap.TryGetValue(jobType, out var commandSet) && commandSet.archerAimCd != null)
            {
                if (commandSet.archerAimCd is ArcherAimWalkSO aimWalkBoolCmd)
                {
                    aimWalkBoolCmd.ExecuteMoveMent(
                        animator,
                        _playerTransform, 
                        moveInput,
                        isAimingStartFrame,
                        isShootRequested);
                }
            }

            if (isShootRequested) { _shootInputConfirmed = false; }

            UnityEngine.Debug.Log($"Input: X={moveInput.x}, Y={moveInput.y}");
        }
        else
        {
            _isCurrentlyAiming = false;
            animator.ResetTrigger("StartADS");
            animator.ResetTrigger("Recoil");
            _player.HandleStanderdMovement(_playerTransform, animator);
            if (adsLayerIndex != -1) { animator.SetLayerWeight(adsLayerIndex, 0f); }
        }

    }

    private void OnEnable()
    {
        //チェック
        if (_animFlgSO)
        {
            //各アニメーションの処理をActionに登録
            _animFlgSO.AttackNormal += AttackAnimation_Normal;
            _animFlgSO.AttackArcherSkills += AttackAnimation_Archer;
            _animFlgSO.ArcherRecoil += ArcherRecoilAnim;

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
            _animFlgSO.ArcherRecoil -= ArcherRecoilAnim;

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
        jobType = (JobType)_gameManager.job;
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _avoidCmd = commandSet.avoidCd;
            if (_avoidCmd != null) { _avoidCmd.Execute(animator); }
            else { UnityEngine.Debug.Log("回避アニメーション失敗。"); }
        }
    }

    /// <summary> /// 回避アニメーション終了コマンド /// </summary>
    public void AvoidAnimationEnd()
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _avoidEndCmd = commandSet.avoidAnimEndCd;
            if (_avoidEndCmd != null)
            {
                _avoidEndCmd.Execute(animator);
                _animFlgSO.Avoidflg = false;
            }

            else { UnityEngine.Debug.Log("回避アニメーション終了失敗"); }
        }
    }

    /// <summary>/// アーチャー発射アニメーション /// </summary>
    public void ArcherRecoilAnim() 
    {
        if(CommandMap.TryGetValue(jobType,out var commandSet)) 
        {
            AnimationBaseSO _currentCmd = commandSet.ArcherrecoilCd;
            if(_currentCmd != null) {_currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"ジョブ:{jobType} のコマンドが設定されていない"); }
        }
    }

    /// <summary> /// 終了用コマンド /// </summary>
    public void ArcherRecoilEndAnim() 
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _archerrecoilEndCmd = commandSet.ArcherrecoilEndCd;
            if (_archerrecoilEndCmd != null)
            {
                _archerrecoilEndCmd.Execute(animator);
                _animFlgSO.ArcherRecoileFlg = false;
            }
            else { UnityEngine.Debug.LogWarning($"ジョブ:{jobType} のコマンドが設定されていない。"); }
        }
    }

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

    //剣士アニメーション
    public void AttackAnimation_Swordman(int comboCount) 
    {
        if (_swordSkillCmd != null)
        { 
            _swordSkillCmd.ExecuteCombo(animator, comboCount);
            OnComboSlash();
        }
        else { UnityEngine.Debug.LogWarning($"SwordSkillAnimSO が設定されていません。"); }
    }

    public void OnComboSlash()  { _plSwardLogic.TryAttackCombo(); }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_player.isAiming && jobType == JobType.ARCHER)
        {
            if (_cameraTransform == null) return;

            float lookAtWeight = 1.0f;
            Vector3 targetPosition = _cameraTransform.position + _cameraTransform.forward * 10f; // カメラの見ている先

            animator.SetLookAtWeight(lookAtWeight);
            animator.SetLookAtPosition(targetPosition);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }
}
