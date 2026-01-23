using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPMonitor : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager; 
    private Animator _animator;
    private float _previousHP;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_gameManager == null)
        {
            // シーンを跨いで存在するマネージャーを自動で見つける
            _gameManager = FindFirstObjectByType<GameManager>();
        }

        if (_gameManager != null)
        {
            // ここが重要！シーン開始時の値を「直前の値」としてセットし、
            // 起動直後の誤作動を防ぐ
            _previousHP = _gameManager.Present_HP;
        }
    }

    void Update()
    {
        if (_gameManager == null) return;

        float currentHP = _gameManager.Present_HP;
        if (currentHP < _previousHP)
        {
            OnDamaged();
        }
        else if (currentHP > _previousHP)
        {
            OnHealed();
        }

        // 現在の値を保存して次のフレームへ
        _previousHP = currentHP;
    }

    private void OnDamaged()
    {
        _animator.SetTrigger("Damage"); 
    }

    private void OnHealed()
    {
        // 回復時のアニメーション
    }
}
