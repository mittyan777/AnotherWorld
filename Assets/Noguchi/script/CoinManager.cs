using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int coinCount = 1000; // 初期所持コイン

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        Debug.Log($"コインを {amount} 枚追加。現在：{coinCount}");
    }

    public void UseCoins(int amount)
    {
        coinCount -= amount;
        Debug.Log($"コインを {amount} 枚使用。残り：{coinCount}");
    }
}