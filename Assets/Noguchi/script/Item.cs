using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
   [Header("アイテム情報")]
    public string itemName;
    public int price;

    private Button button;
    private ItemPurchaseManager purchaseManager;

    void Start()
    {
        // Button コンポーネントを取得
        button = GetComponent<Button>();
        purchaseManager = FindObjectOfType<ItemPurchaseManager>();

        if (button != null && purchaseManager != null)
        {
            button.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.LogWarning($"ItemButton 初期化エラー: Button={button != null}, PurchaseManager={purchaseManager != null}");
        }
    }

    private void OnClick()
    {
        if (purchaseManager != null)
        {
            purchaseManager.TryPurchase(itemName, price);
        }
        else
        {
            Debug.LogError("ItemPurchaseManager が見つかりません。シーン内に配置してください。");
        }
    }
}