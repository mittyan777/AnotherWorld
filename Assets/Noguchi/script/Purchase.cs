using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
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

    private int playerCoins = 1000;
    private List<string> inventory = new List<string>();
    private bool isShopOpen = false;

    void Start()
    {
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
