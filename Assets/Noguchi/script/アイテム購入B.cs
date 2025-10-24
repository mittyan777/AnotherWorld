using UnityEngine;

public class CameraApproach : MonoBehaviour
{
    [Header("アイテム欄UI")]
    [SerializeField] private GameObject inventoryUI;

    private bool isOpen = false; // 開いているかどうかのフラグ

    void Start()
    {
        // 開始時は非表示
        if (inventoryUI != null)
            inventoryUI.SetActive(false);
    }

    void Update()
    {
        // Bキーで開閉
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen; // 状態を反転
        inventoryUI.SetActive(isOpen);

        if (isOpen)
            Debug.Log("アイテム欄を開きました。");
        else
            Debug.Log("アイテム欄を閉じました。");
    }
}
