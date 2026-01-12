using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Open_Chest : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Animator animator;        // 宝箱のAnimator
    [SerializeField] private Collider chestTrigger;    // 宝箱の接触判定
    [SerializeField] private KeyCode openKey = KeyCode.E; // 開けるキー設定

    [Header("UI設定")]
    [SerializeField] private GameObject openUI;       // 「Eで開ける」などのUI
    [SerializeField] private float destroyTime = 1.5f; // 宝箱破壊までの遅延時間

    [Header("サウンド設定")]
    [SerializeField] private AudioSource Open_SE;

    [Header("コイン設定")]
    [SerializeField] private GameObject Coin;         // コインPrefab
    [SerializeField] private Transform CoinSpawn;     // コイン出現位置
    [SerializeField] private int CoinCount;           // 生成するコインの個数

    [Header("コインの跳び方")]
    [SerializeField] private float CoinUp;            // 上方向の力
    [SerializeField] private float CoinSide;          // 横方向のばらつき

    [Header("コイン演出遅延")]
    [SerializeField] private float OpenChestTime;     // コイン生成間隔

    [Header("アニメーション待機時間")]
    [SerializeField] private float AnimWaitTime = 1.0f; // アニメーション完了までの待機時間

    // プレイヤー接近判定
    private bool isPlayerNear = false;

    // 宝箱開封済み判定
    private bool isOpened = false;

    // 自動取得用（ResetはInspectorの「Reset」で呼ばれる）
    private void Reset()
    {
        animator = GetComponent<Animator>();
        chestTrigger = GetComponent<Collider>();
    }

    private void Start()
    {
        // UI初期状態は非表示
        if (openUI != null)
        {
            openUI.SetActive(false);
        }
    }

    private void Update()
    {
        // プレイヤーが近く & 未開封 & Eキー押下で開封開始
        if (isPlayerNear && !isOpened && Input.GetKeyDown(openKey))
        {
            StartCoroutine(OpenChestCoroutine());


            if (Open_SE != null)
            {
                Open_SE.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpened) return; // 開封済みなら何もしない

        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (openUI != null) openUI.SetActive(true); // UI表示
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (openUI != null) openUI.SetActive(false); // UI非表示
        }
    }

    // 宝箱開封処理のコルーチン
    private IEnumerator OpenChestCoroutine()
    {
        isOpened = true;      // 宝箱を開封済みにする
        isPlayerNear = false; // プレイヤー判定をオフ

        // アニメーション再生
        if (animator != null)
        {
            animator.SetBool("open", true);
        }


        // 再接触防止
        if (chestTrigger != null)
        {
            chestTrigger.enabled = false;
        }

        // UI非表示
        if (openUI != null)
        {
            openUI.SetActive(false);
        }

        // アニメーションが完了するまで待機
        yield return new WaitForSeconds(AnimWaitTime);

        // コイン生成開始
        yield return StartCoroutine(SpawnCoins());

        // 宝箱破壊（コイン生成後）
        Destroy(gameObject, destroyTime);
    }

    // コイン生成処理
    private IEnumerator SpawnCoins()
    {
        for (int i = 0; i < CoinCount; i++)
        {
            Vector3 spawnPosition = CoinSpawn.position; // コインの生成位置
            Quaternion spawnRotation = Quaternion.identity;

            GameObject coin = Instantiate(Coin, spawnPosition, spawnRotation);

            Rigidbody coin_rb = coin.GetComponent<Rigidbody>();
            if (coin_rb != null)
            {
                coin_rb.freezeRotation = true; // 回転を固定

                // 上方向と横方向のばらつきを加えた力
                Vector3 force = Vector3.up * CoinUp +
                                new Vector3(
                                    Random.Range(-CoinSide, CoinSide),
                                    0f,
                                    Random.Range(-CoinSide, CoinSide)
                                );
                coin_rb.AddForce(force, ForceMode.Impulse);
            }

            // 指定間隔で次のコインを生成
            yield return new WaitForSeconds(OpenChestTime);
        }
    }
}