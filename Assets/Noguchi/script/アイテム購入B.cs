using UnityEngine;
using UnityEngine.UI;

public class CameraApproach : MonoBehaviour
{
     [Header("プレイヤーのコイン管理オブジェクト")]
    [SerializeField] private CoinManager coinManager;

    [Header("購入するアイテムの情報")]
    [SerializeField] private Item itemData;

    [Header("購入ボタン")]
    [SerializeField] private Button purchaseButton;

    [Header("UI表示")]
    [SerializeField] private Text resultText;

    void Start()
    {
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }
    }

    void OnPurchaseButtonClicked()
    {
        int playerCoins = coinManager.GetCoinCount();
        int itemPrice = itemData.price;

        if (playerCoins >= itemPrice)
        {
            // 購入成功
            coinManager.UseCoins(itemPrice);
            resultText.text = $"{itemData.itemName} を購入しました！";
            Debug.Log($"購入成功：{itemData.itemName}");
        }
        else
        {
            // コイン不足
            resultText.text = "コインが足りません！";
            Debug.Log("購入失敗：コイン不足");
        }
    }
}
