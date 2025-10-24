using UnityEngine;
using UnityEngine.UI;

public class ItemPurchaseManager : MonoBehaviour
{
    [Header("コイン管理")]
    [SerializeField] private CoinManager coinManager;

    [Header("結果表示UI")]
    [SerializeField] private Text resultText;

    public void TryPurchase(string itemName, int price)
    {
        int coins = coinManager.GetCoinCount();

        if (coins >= price)
        {
            coinManager.UseCoins(price);
            resultText.text = $"{itemName} を購入しました！";
            Debug.Log($"{itemName} を購入（-{price}）");
        }
        else
        {
            resultText.text = "コインが足りません！";
            Debug.Log($"購入失敗：{itemName}（必要: {price} / 所持: {coins}）");
        }
    }
}
