using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//ジョブ列挙
public enum JobType
{
    SWORDSMAN,   // 剣士 = 0
    ARCHER,      // アーチャー = 1  
    WIZARD,      // 魔法使い = 2 

    NONE
}

public class PlayerAnimation_main : MonoBehaviour
{
    public  static PlayerAnimation_main Instance {  get; private set; } 

    [SerializeField] private CommandoConfigSO _cmdCofigSO;
    [SerializeField] private AnimationFlagManagerSO _animFlgSO;
    [SerializeField] GameManager _gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] Player_main _player;  
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private SwordSkillAnime _swordSkillCmd;
    [SerializeField] Player_Swoad_main _plSwardLogic;

    private Transform _spineBorn;

    private Transform _playerTransform;
    private int _atkLayerIndex = -1;
    private bool _isCurrentlyAiming = false;
    private bool _shootInputConfirmed = false;
    public bool _swordattackanimFlg = false;

    //初期ジョブNone
    public JobType jobType { get; private set; } = JobType.NONE;

    private Dictionary<JobType, CommandoConfigSO.CommandSet> CommandMap;
    private int _currentjobNum = -1;

    private void Awake()
    {
        //シングルトン
        if(Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        if (!animator) { animator = GetComponent<Animator>();  }

        SetSceneCompornent();

        if (_cmdCofigSO != null) { CommandMap = _cmdCofigSO.commandSets.ToDictionary(set => set.JobType, set => set); }

    }

    private void Start()
    {
        if (_player != null) { _playerTransform = _player.transform; }

        if (_gameManager == null)
        {
            UnityEngine.Debug.LogError("GameManagerなし", this);
            return;
        }

        _currentjobNum = (int)_gameManager.job;
        SetJobType((JobType)_currentjobNum);

        if (animator != null)
        {
            _atkLayerIndex = animator.GetLayerIndex("UpperBodyLayer");
        }

        UnityEngine.Debug.Log($"セット: {jobType}");
    }

    private void Update()
    {

        if (SceneManager.GetActiveScene().name == "start_main")
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "EndRoll")
        {
            Destroy(gameObject);
        }
        if (_gameManager == null) { _gameManager = GameObject.FindAnyObjectByType<GameManager>(); }
        if (_gameManager == null) return;

        int jobFromManager = (int)_gameManager.job;
        if (jobFromManager != _currentjobNum)
        {
            _currentjobNum = jobFromManager;
            SetJobType((JobType)_currentjobNum);

            UnityEngine.Debug.Log($"ジョブ更新 JobType: {jobType}");
        }

        if(_player != null && _player.isAiming && UnityEngine.Input.GetMouseButtonUp(0)) 
        {
            _shootInputConfirmed = true;
        }
    }


    private void FixedUpdate()
    {
        if(_player == null || animator == null) { return; }

        Vector2 moveInput = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));

        bool isAiming = _player.isAiming;
        bool isArcher = (jobType == JobType.ARCHER);

        //ADS時　ー＞　アーチャー
        bool isAimingStartFrame = (isAiming && isArcher && !_isCurrentlyAiming);
        bool isShootRequested = _shootInputConfirmed;
        int adsLayerIndex = animator.GetLayerIndex("ADS Layer");
        int atkLayerIndex = animator.GetLayerIndex("UpperBodyLayer");

        if (isArcher)
        {
            if (isAiming) 
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
                            isShootRequested
                         );
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

                if(atkLayerIndex != -1) 
                {
                    AnimatorStateInfo atkState = animator.GetCurrentAnimatorStateInfo(atkLayerIndex);
                    float targetWeight = (atkState.IsTag("Attack") && atkState.normalizedTime < 1.0f) ? 1.0f : 0.0f;
                    animator.SetLayerWeight(atkLayerIndex, targetWeight);
                }
            }
        }
        else 
        {
            _isCurrentlyAiming = false;
            if (adsLayerIndex != -1) { animator.SetLayerWeight(adsLayerIndex, 0f); }
            if (atkLayerIndex != -1) { animator.SetLayerWeight(atkLayerIndex, 0f); }
            _player.HandleStanderdMovement(_playerTransform, animator);
        }

    }


    private void OnEnable()
    {
        //各イベント登録
        if (_animFlgSO)
        {
            _animFlgSO.NormalMajicAttack += AttackAnimation_Normal;
            _animFlgSO.AttackArcherSkills += AttackAnimation_Archer;
            _animFlgSO.ArcherRecoil += ArcherRecoilAnim;
            _animFlgSO.MajicSkil += AttackAnimation_Wizard;

            _animFlgSO.AvoidanceEvents += AvoidAnim;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            UnityEngine.Debug.LogError("_animFlgSOがない", this);
            return;
        }
    }



    private void OnDisable()
    {
        //イベント解除
        if (_animFlgSO)
        {
            _animFlgSO.NormalMajicAttack -= AttackAnimation_Normal;
            _animFlgSO.AttackArcherSkills -= AttackAnimation_Archer;
            _animFlgSO.ArcherRecoil -= ArcherRecoilAnim;
            _animFlgSO.MajicSkil -= AttackAnimation_Wizard;

            _animFlgSO.AvoidanceEvents -= AvoidAnim;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    //シーン切り替え時に情報検索
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode) 
    {
        SetSceneCompornent();
    }

    /// <summary> /// コンポーネント取得 /// </summary>
    private void SetSceneCompornent() 
    {
        _gameManager = GameObject.FindAnyObjectByType<GameManager>();
        _player = GameObject.FindAnyObjectByType<Player_main>();
        
        if(_player != null) 
        {
            _playerTransform = _player.transform;
            _plSwardLogic = _player.GetComponent<Player_Swoad_main>();
            animator = _player.GetComponent<Animator>();
            Camera childCam = _player.GetComponentInChildren<Camera>();
            _cameraTransform = (childCam != null) ? childCam.transform : Camera.main?.transform;
        }

        _spineBorn = null;
        if (animator != null)
        {
            _spineBorn = animator.GetBoneTransform(HumanBodyBones.Spine);
        }

        if (_gameManager != null)
        {
            int jobNum = (int)_gameManager.job;
            SetJobType((JobType)jobNum);
            UnityEngine.Debug.Log($"ジョブ: {jobType}");
        }
    }

    /// <summary> ///　jobセット /// </summary>
    /// <param name="_newJob"></param>
    public void SetJobType(JobType _newJob)
    {
        jobType = _newJob;
    }

    /// <summary> /// 通常攻撃アニメーション /// </summary>
    private void AttackAnimation_Normal()
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _currentCmd = commandSet.normalAttackCd;
            if (_currentCmd != null) { _currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺�"); }
        }
        else { UnityEngine.Debug.LogError($"繧ｸ繝ｧ繝冶ｨｭ螳壹Α繧ｹ{jobType}"); }
    }

    /// <summary> /// 通常攻撃終了 /// </summary>
    public void AttackAnimation_NormalEnd()
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _endCmd = commandSet.normalAttackEndCd;
            if (_endCmd != null) { _endCmd.Execute(animator); }
            else { UnityEngine.Debug.LogError("螟ｱ謨"); }
        }
    }

    /// <summary> /// 回避アニメーション/// </summary>
    public void AvoidAnim()
    {
        jobType = (JobType)_gameManager.job;
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _avoidCmd = commandSet.avoidCd;
            if (_avoidCmd != null) { _avoidCmd.Execute(animator); }
            else { UnityEngine.Debug.Log("蝗樣∩繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ螟ｱ謨励"); }
        }
    }

    /// <summary> /// 回避終了 /// </summary>
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

            else { UnityEngine.Debug.Log("蝗樣∩繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ邨ゆｺ�､ｱ謨"); }
        }
    }

    /// <summary>///  アーチャーアニメーションー＞構え /// </summary>
    public void ArcherRecoilAnim() 
    {
        if(CommandMap.TryGetValue(jobType,out var commandSet)) 
        {
            AnimationBaseSO _currentCmd = commandSet.ArcherrecoilCd;
            if(_currentCmd != null) {_currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺"); }
        }
    }

    /// <summary> /// アーチャー -> 構え終了 /// </summary>
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
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺�"); }
        }
    }

    public void AttackAnimation_Archer()
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _currentCmd = commandSet.archerSkilsCd;
            if (_currentCmd != null) { _currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺�"); }
        }
        else { UnityEngine.Debug.LogError("繧ｸ繝ｧ繝冶ｨｭ螳壹Α繧ｹ"); }
    }

    public void AttackAnimation_Swordman(int comboCount) 
    {
        if (_swordSkillCmd != null)
        {
            _animFlgSO.swordAttackAnimFlg = true;
            _swordSkillCmd.ExecuteCombo(animator, comboCount);
            //OnComboSlash();
        }
        else { UnityEngine.Debug.LogWarning($"SwordSkillAnimSO 縺瑚ｨｭ螳壹＆繧後※縺�∪縺帙ｓ縲"); }
    }

    public void AttackAnimation_Wizard() 
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _currentCmd = commandSet.WizardSkilCd;
            if (_currentCmd != null) { _currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺�"); }
            _animFlgSO.MajicSkilFlg = false;
        }
        else { UnityEngine.Debug.LogError("繧ｸ繝ｧ繝冶ｨｭ螳壹Α繧ｹ"); }
    }


    public void SetWizardSkillIndex(int skillIndex)
    {
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            if (commandSet.WizardSkilCd is Majician_SkilAnimSO majicianSO)
            {
                majicianSO.ActiveSkillIndex = skillIndex;
            }
            else
            {
                UnityEngine.Debug.LogError("WizardSkilCd");
            }
        }
    }


    public void OnComboSlash()  { _plSwardLogic.TryAttackCombo(); }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_player.isAiming && jobType == JobType.ARCHER)
        {
            if (_cameraTransform == null) return;

            float lookAtWeight = 1.0f;
            Vector3 targetPosition = _cameraTransform.position + _cameraTransform.forward * 10f;

            animator.SetLookAtWeight(lookAtWeight);
            animator.SetLookAtPosition(targetPosition);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }

    public bool IsMovementRestricted 
    {
        get 
        {
            //アーチャーは常に自由に動けるように
            if(jobType == JobType.ARCHER) { return false; }
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsTag("Attack") && stateInfo.normalizedTime < 1.0f;
        }
    }
}
