using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Purchase : MonoBehaviour
{
    [Header("コイン管理")]
    
    private float getCoin;

    [System.Serializable]
    public class ScrollViewGroup
    {
        public string groupName;
        public Button[] itemButtons;
        public int[] itemPrices;
    }

    [Header("スクロールビューごとの商品情報")]
    [SerializeField] private ScrollViewGroup[] scrollViewGroups;

    [Header("UI参照")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text messageText;

    [Header("ショップ全体のUIオブジェクト")]
    [SerializeField] private GameObject shopUI;

    private List<string> inventory = new List<string>();

    GameObject gm;

    void Start()
    {
     
       

        // プレイヤーの所持コイン取得
         gm = GameObject.FindWithTag("GameManager");
        if (gm == null)
        {
            Debug.LogError("GameManager が見つかりません。");
            return;
        }

     

        // 商品ボタンにイベント登録
        InitializeButtons();

        // UI 初期化
        UpdateCoinUI();
        if (messageText != null) messageText.text = "";
        if (shopUI != null) shopUI.SetActive(false);
    }

    /// <summary>
    /// スクロールビュー内ボタンに購入処理を設定
    /// </summary>
    private void InitializeButtons()
    {
        foreach (var group in scrollViewGroups)
        {
            for (int i = 0; i < group.itemButtons.Length; i++)
            {
                int index = i;

                string itemName = $"{group.groupName}_{index + 1}";
                int price = group.itemPrices[index];

                group.itemButtons[i].onClick.AddListener(() =>
                {
                    TryPurchase(itemName, price);
                });
            }
        }
    }

    /// <summary>
    /// 商品購入処理
    /// </summary>
    private void TryPurchase(string itemName, float price)
    {

        
        if (getCoin >= price)
        {
            gm.GetComponent<GameManager>().Coin -= price;
           
            inventory.Add(itemName);

            messageText.text = $"{itemName} を購入しました！\n（-{price}コイン）";
            Invoke("message_reset", 1);
            Debug.Log($"{itemName} の購入完了。残りコイン: {gm.GetComponent<GameManager>().Coin}");
        }
        else
        {
            messageText.text = $"コインが足りません！（必要: {price}）";
            Debug.Log($"購入失敗：価格 {price} に対して残高不足");
        }
        

        UpdateCoinUI();
    }
    void message_reset()
    {
        messageText.text = "";
    }
    /// <summary>
    /// コイン表示を更新
    /// </summary>
    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = $"所持コイン: {getCoin}";
    }

    /// <summary>
    /// 現在のインベントリをログに表示
    /// </summary>
    public void ShowInventory()
    {
        Debug.Log("=== インベントリ ===");
        foreach (var item in inventory)
        {
            Debug.Log(item);
        }
    }
    private void Update()
    {
        UpdateCoinUI();
        getCoin = gm.GetComponent<GameManager>().Coin;
    }
}
