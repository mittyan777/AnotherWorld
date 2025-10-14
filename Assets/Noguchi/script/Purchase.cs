using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("選択ボタン4つ")]
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Button buttonC;
    [SerializeField] private Button buttonD;

    [Header("購入UIのボタン")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("UI参照")]
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject purchaseUI;

    // 現在選択されている商品名を保持
    private string currentItemName;

    void Start()
    {
        // 各ボタンのクリックイベント登録
        buttonA.onClick.AddListener(() => OnItemSelected("A"));
        buttonB.onClick.AddListener(() => OnItemSelected("B"));
        buttonC.onClick.AddListener(() => OnItemSelected("C"));
        buttonD.onClick.AddListener(() => OnItemSelected("D"));

        yesButton.onClick.AddListener(OnPurchaseConfirmed);
        noButton.onClick.AddListener(OnPurchaseCancelled);

        // 最初は購入UIを非表示
        purchaseUI.SetActive(false);
    }

    // 商品が選択されたとき
    void OnItemSelected(string itemName)
    {
        currentItemName = itemName;
        Debug.Log($"{itemName} が選択されました。購入UIに移行します。");

        // UI切り替え
        mainUI.SetActive(false);
        purchaseUI.SetActive(true);
    }

    // 「はい」ボタンが押されたとき
    void OnPurchaseConfirmed()
    {
        Debug.Log($"{currentItemName} の購入が完了しました！");
        ReturnToMainUI();
    }

    // 「いいえ」ボタンが押されたとき
    void OnPurchaseCancelled()
    {
        Debug.Log("購入をキャンセルしました。");
        ReturnToMainUI();
    }

    // メインUIに戻る
    void ReturnToMainUI()
    {
        purchaseUI.SetActive(false);
        mainUI.SetActive(true);
        currentItemName = null;
    }
}
