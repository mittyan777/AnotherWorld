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

/// 繧ｸ繝ｧ繝悶ち繧､繝励＃縺ｨ縺ｫ繧ｻ繝�ヨ縺吶ｋ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ繧ｳ繝槭Φ繝峨ｒ螟画峩
public enum JobType
{
    SWORDSMAN,    //蜑｣螢ｫ  = 0
    ARCHER,       //蠑謎ｽｿ縺(繧｢繝ｼ繝√Ε繝ｼ) = 1
    WIZARD,        //鬲疲ｳ穂ｽｿ縺 = 2

    NONE
}

public class PlayerAnimation_main : MonoBehaviour
{

    [SerializeField] private CommandoConfigSO _cmdCofigSO;
    [SerializeField] private AnimationFlagManagerSO _animFlgSO;

    //TODO�帙ユ繧ｹ繝域凾縺ｮ縺ｿhikido菴ｿ逕ｨ
    [SerializeField] GameManager _gameManager;
    //[SerializeField] GameManager _gameManager;

    [SerializeField] private Animator animator;
    [SerializeField] Player_main _player;  //繝励Ξ繧､繝､繝ｼhikido
    [SerializeField] private Transform _cameraTransform;
    private Transform _playerTransform;
    private bool _isCurrentlyAiming = false;
    private bool _shootInputConfirmed = false;

    [SerializeField] private SwordSkillAnime _swordSkillCmd;
    [SerializeField] Player_Swoad_main _plSwardLogic;

    //繧ｸ繝ｧ繝悶ち繧､繝
    public JobType jobType { get; private set; } = JobType.NONE;

    //閨ｷ遞ｮ縺ｫ蟇ｾ蠢懊＠縺溘さ繝槭Φ繝峨そ繝�ヨ
    private Dictionary<JobType, CommandoConfigSO.CommandSet> CommandMap;
    private int _currentjobNum = -1;
    private void Start()
    {
        if (_player != null) { _playerTransform = _player.transform; }

        if (_gameManager == null)
        {
            UnityEngine.Debug.LogError("GameManager縺瑚ｨｭ螳壹＆繧後※縺�∪縺帙ｓ縲ゅず繝ｧ繝冶ｨｭ螳壹′縺ｧ縺阪∪縺帙ｓ縲", this);
            return;
        }

        _currentjobNum = (int)_gameManager.job;
        SetJobType((JobType)_currentjobNum);
        _plSwardLogic.GetComponent<Player_Swoad_main>();
        

        UnityEngine.Debug.Log($"蛻晄悄繧ｸ繝ｧ繝悶ち繧､繝苓ｨｭ螳壼ｮ御ｺ: {jobType}");
    }

    private void Awake()
    {
        if (!animator)
        {
            UnityEngine.Debug.LogError("Animator繧ｳ繝ｳ繝昴�繝阪Φ繝医′隕九▽縺九ｊ縺ｾ縺帙ｓ縲", this);
            return;
        }

        if (!_cmdCofigSO)
        {
            UnityEngine.Debug.LogError("繧ｳ繝槭Φ繝峨さ繝ｳ繝輔ぅ繧ｰ縺瑚ｨｭ螳壹＆繧後※縺�↑縺", this);
            return;
        }

        if (_gameManager != null)
        {
            int jobNum = (int)_gameManager.job;
            SetJobType((JobType)jobNum);

            UnityEngine.Debug.Log($"蛻晄悄繧ｸ繝ｧ繝悶ち繧､繝: {jobType}");
        }

        CommandMap = _cmdCofigSO.commandSets.ToDictionary(set => set.JobType, set => set);

    }

    private void Update()
    {
        if (_gameManager == null) { _gameManager = GetComponent<GameManager>(); }

        int jobFromManager = (int)_gameManager.job;
        if (jobFromManager != _currentjobNum)
        {
            _currentjobNum = jobFromManager;
            SetJobType((JobType)_currentjobNum);

            UnityEngine.Debug.Log($"繧ｸ繝ｧ繝悶′譖ｴ譁ｰ縺輔ｌ縺ｾ縺励◆� JobType: {jobType}");
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

        //ADS縺悟ｧ九∪縺｣縺滓怙蛻昴�繝輔Ξ繝ｼ繝蛟､繧呈､懷�
        bool isAimingStartFrame = (isAiming && isArcher && !_isCurrentlyAiming);

        //繝槭え繧ｹ繝懊ち繝ｳ髮｢縺励〒逋ｺ蟆
        bool isShootRequested = _shootInputConfirmed;

        int adsLayerIndex = animator.GetLayerIndex("ADS Layer");

        //ADS譎ゅ�蜃ｦ逅
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
        //繝√ぉ繝�け
        if (_animFlgSO)
        {
            //蜷�い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ縺ｮ蜃ｦ逅�ｒAction縺ｫ逋ｻ骭ｲ
            _animFlgSO.NormalMajicAttack += AttackAnimation_Normal;
            _animFlgSO.AttackArcherSkills += AttackAnimation_Archer;
            _animFlgSO.ArcherRecoil += ArcherRecoilAnim;
            _animFlgSO.MajicSkil += AttackAnimation_Wizard;

            //蝗樣∩逕ｨ繧､繝吶Φ繝育匳骭ｲ
            _animFlgSO.AvoidanceEvents += AvoidAnim;
        }
        else
        {
            UnityEngine.Debug.LogError("_animFlgSO縺悟ｭ伜惠縺励↑縺", this);
            return;
        }
    }

    private void OnDisable()
    {
        if (_animFlgSO)
        {
            //蜷�い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ蜃ｦ逅�ｒAction縺九ｉ蜑企勁
            _animFlgSO.NormalMajicAttack -= AttackAnimation_Normal;
            _animFlgSO.AttackArcherSkills -= AttackAnimation_Archer;
            _animFlgSO.ArcherRecoil -= ArcherRecoilAnim;
            _animFlgSO.MajicSkil -= AttackAnimation_Wizard;

            //蝗樣∩逕ｨ繧､繝吶Φ繝郁ｧ｣髯､
            _animFlgSO.AvoidanceEvents -= AvoidAnim;
        }
    }

    /// <summary> /// 繧ｸ繝ｧ繝冶ｨｭ螳(繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ逕ｨ) /// </summary>
    /// <param name="_newJob"></param>
    public void SetJobType(JobType _newJob)
    {
        jobType = _newJob;
    }

    /// <summary> /// 騾壼ｸｸ謾ｻ謦�い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ /// </summary>
    private void AttackAnimation_Normal()
    {
        //謾ｻ謦��繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ逋ｺ轣ｫ
        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _currentCmd = commandSet.normalAttackCd;
            if (_currentCmd != null) { _currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺�"); }
        }
        else { UnityEngine.Debug.LogError($"繧ｸ繝ｧ繝冶ｨｭ螳壹Α繧ｹ{jobType}"); }
    }

    /// <summary> /// 謾ｻ謦�い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ邨ゆｺ�畑髢｢謨ｰ /// </summary>
    public void AttackAnimation_NormalEnd()
    {

        if (CommandMap.TryGetValue(jobType, out var commandSet))
        {
            AnimationBaseSO _endCmd = commandSet.normalAttackEndCd;
            if (_endCmd != null) { _endCmd.Execute(animator); }
            else { UnityEngine.Debug.LogError("螟ｱ謨"); }
        }
    }

    /// <summary> /// 蝗樣∩繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ /// </summary>
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

    /// <summary> /// 蝗樣∩繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ邨ゆｺ�さ繝槭Φ繝 /// </summary>
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

    /// <summary>/// 繧｢繝ｼ繝√Ε繝ｼ逋ｺ蟆�い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ /// </summary>
    public void ArcherRecoilAnim() 
    {
        if(CommandMap.TryGetValue(jobType,out var commandSet)) 
        {
            AnimationBaseSO _currentCmd = commandSet.ArcherrecoilCd;
            if(_currentCmd != null) {_currentCmd.Execute(animator); }
            else { UnityEngine.Debug.LogWarning($"繧ｸ繝ｧ繝:{jobType} 縺ｮ繧ｳ繝槭Φ繝峨′險ｭ螳壹＆繧後※縺�↑縺"); }
        }
    }

    /// <summary> /// 邨ゆｺ�畑繧ｳ繝槭Φ繝 /// </summary>
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

    /// <summary> /// 繧｢繝ｼ繝√Ε繝ｼ繧ｹ繧ｭ繝ｫ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ /// </summary>
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

    //蜑｣螢ｫ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ
    public void AttackAnimation_Swordman(int comboCount) 
    {
        if (_swordSkillCmd != null)
        { 
            _swordSkillCmd.ExecuteCombo(animator, comboCount);
            OnComboSlash();
        }
        else { UnityEngine.Debug.LogWarning($"SwordSkillAnimSO 縺瑚ｨｭ螳壹＆繧後※縺�∪縺帙ｓ縲"); }
    }

    //繝槭ず繧ｷ繝｣繝ｳ縺ｮ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ
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
                UnityEngine.Debug.LogError("WizardSkilCd縺勲ajician_SkilAnimSO縺ｧ縺ｯ縺ゅｊ縺ｾ縺帙ｓ縲");
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
            Vector3 targetPosition = _cameraTransform.position + _cameraTransform.forward * 10f; // 繧ｫ繝｡繝ｩ縺ｮ隕九※縺�ｋ蜈

            animator.SetLookAtWeight(lookAtWeight);
            animator.SetLookAtPosition(targetPosition);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }
}
