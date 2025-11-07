using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class Purchase : MonoBehaviour
{

    private koin Koin;
    private int getcoin;

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
    [SerializeField] private GameObject shopUI; // ← ここをCanvasまたは親Panelに設定！

    //private int playerCoins = 1000;
    private List<string> inventory = new List<string>();
    private bool isShopOpen = false;

    void Start()
    {
          // 🔹 koin オブジェクトをシーンから自動取得
        Koin = FindObjectOfType<koin>();
        if (Koin == null)
        {
            Debug.LogError("koin オブジェクトがシーンに見つかりません。");
            return;
        }

        getcoin = Koin.playerCoin;//プレイヤーのコインを取得

        foreach (var group in scrollViewGroups)
        {
            for (int i = 0; i < group.itemButtons.Length; i++)
            {
                int index = i;
                string itemName = $"{group.groupName}_{index + 1}";
                int price = group.itemPrices[index];

                group.itemButtons[i].onClick.AddListener(() => TryPurchase(itemName, price));
            }
        }

        UpdateCoinUI();
        messageText.text = "";

        if (shopUI != null)
            shopUI.SetActive(false); // 起動時は非表示
    }

    void Update()
    {
        // Bキー入力でショップUIの表示切り替え
        if (Input.GetKeyDown(KeyCode.B))
        {
            isShopOpen = !isShopOpen;
            if (shopUI != null)
                shopUI.SetActive(isShopOpen);
        }
    }

    void TryPurchase(string itemName, int price)
    {
        if (getcoin >= price)
        {
            getcoin -= price;
            Koin.playerCoin = getcoin;
            inventory.Add(itemName);
            messageText.text = $"{itemName} を購入しました！（-{price}コイン）";
            Debug.Log($"{itemName} の購入完了。残りコイン: {getcoin}");
        }
        else
        {
            messageText.text = $"コインが足りません！（必要: {price}）";
            Debug.Log($"購入失敗：{itemName} の価格 {price} に対して残高不足");
        }

        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = $"所持コイン: {getcoin}";
    }

    public void ShowInventory()
    {
        Debug.Log("=== インベントリ ===");
        foreach (var item in inventory)
        {
            Debug.Log(item);
        }
    }
}
