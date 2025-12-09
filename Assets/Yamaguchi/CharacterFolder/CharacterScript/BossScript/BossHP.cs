using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    [SerializeField] private int maxBossHP;
    public int currentBossHP { get; private set; }

    // BossManagerをAwake/Startで取得するため private に戻し、シングルトンから取得
    private BossMoveManager manager;

    [SerializeField] private float damageTime;
    [SerializeField] private float breakTime;

    //HPバー用変数
    //赤色バー
    [SerializeField] private Slider currentHPSlider; // UnityEngine.UI.Slider を使う
    //黄色バー(遅延追尾)
    [SerializeField] private Slider fadingHPSlider;
    //黄色いバーの減るスピード
    [SerializeField] private float fadeSpeed = 2.0f;
    //ダメージ処理区間
    [SerializeField] private float damageInterval;
    //ダメージを受けれるかのフラグ
    private bool isInvulnerabal = false;

    private void Start()
    {
        // シングルトンを参照
        manager = BossMoveManager.instance;
        currentBossHP = maxBossHP;

        // --- Sliderの初期設定 ---
        if (currentHPSlider != null)
        {
            currentHPSlider.maxValue = maxBossHP; // 最大値をボスHPに設定
            currentHPSlider.value = maxBossHP;    // 現在値を最大HPに設定
        }
        if (fadingHPSlider != null)
        {
            fadingHPSlider.maxValue = maxBossHP;  // 最大値をボスHPに設定
            fadingHPSlider.value = maxBossHP;     // 現在値を最大HPに設定
        }
    }

    private void Update()
    {
        // HPバーのフェード処理
        if (fadingHPSlider != null && currentHPSlider != null)
        {
            // 黄色いSliderのvalueを、赤色Sliderのvalue（現在のHP値）まで徐々に減らす
            fadingHPSlider.value = Mathf.Lerp(
                fadingHPSlider.value,
                currentHPSlider.value, // 目標値は赤色Sliderの値 (currentHPBar.fillAmount の代わりに currentHPSlider.value を使用)
                Time.deltaTime * fadeSpeed
            );
        }
    }

    private void UpdateHPBar(int newHP)
    {
        //赤色のSlider (currentHPSlider) を目標値に即座に設定
        if (currentHPSlider != null)
        {
            // valueプロパティに現在のHPを直接設定
            currentHPSlider.value = newHP;
        }
    }

    //メインのダメージ処理
    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerabal) return;
        if (currentBossHP <= 0) return;

        currentBossHP -= damageAmount;
        UpdateHPBar(currentBossHP);
        // 死亡チェックを修正
        if (currentBossHP <= 0)
        {
            currentBossHP = 0;
            if (manager != null)
            {
                // Managerに共通処理の実行を依頼する
                manager.HandleBossDeath();
            }
        }
    }

    public IEnumerator TestDamege02(int testDamage) 
    {
        if (!isInvulnerabal)
        {
            isInvulnerabal = true;
            if (currentBossHP >= 0)
            { 
                currentBossHP -= testDamage;
                UpdateHPBar(currentBossHP);
                // 死亡チェックを修正
                if (currentBossHP <= 0)
                {
                    currentBossHP = 0;
                    if (manager != null)
                    {
                        // Managerに共通処理の実行を依頼する
                        manager.HandleBossDeath();
                    }
                }
                yield return new WaitForSeconds(damageInterval);
                isInvulnerabal = false;
            }
        }
    }

    //Die()はオブジェクトの遅延破壊のみを行う
    public void Die()
    {
        Destroy(gameObject, breakTime);
    }
}