using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class ScrollViewGroup
    {
        public string groupName; 
        public Button[] itemButtons; // 4つのボタン
        public int[] itemPrices;     // 各ボタンに対応した価格
    }

    [Header("スクロールビューごとの商品情報")]
    [SerializeField] private ScrollViewGroup[] scrollViewGroups; // 全4スクロールビューを登録

    [Header("UI参照")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text messageText;

    private int playerCoins = 1000;
    private List<string> inventory = new List<string>();

    void Start()
    {
        // 各スクロールビューのボタンにイベント登録
        foreach (var group in scrollViewGroups)
        {
            for (int i = 0; i < group.itemButtons.Length; i++)
            {
                int index = i; // ローカル変数に退避（ラムダ式用）
                string itemName = $"{group.groupName}_{index + 1}";
                int price = group.itemPrices[index];

                group.itemButtons[i].onClick.AddListener(() => TryPurchase(itemName, price));
            }
        }

        UpdateCoinUI();
        messageText.text = "";
    }

    // 購入処理
    void TryPurchase(string itemName, int price)
    {
        if (playerCoins >= price)
        {
            playerCoins -= price;
            inventory.Add(itemName);
            messageText.text = $"{itemName} を購入しました！（-{price}コイン）";
            Debug.Log($"{itemName} の購入完了。残りコイン: {playerCoins}");
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
            coinText.text = $"所持コイン: {playerCoins}";
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
