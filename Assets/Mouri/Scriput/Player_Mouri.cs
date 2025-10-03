using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Mouri : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    public bool canControl = true;  // ← 追加！


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("aaaa");
    }

    void Update()
    {
        if (!canControl) return; // ここで操作全部止まる

        Move();
        Rotate();

        // 「E」キーでルーレットを調べる
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 前方にRayを飛ばして、ルーレット台に当たったら
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            

            if (Physics.Raycast(ray, out hit, 2f)) // 2m以内を調べる
            {
                if (hit.collider.CompareTag("Roulette")) // ルーレット台のタグを"Roulette"にする
                {
                    Debug.Log("ルーレットを調べた！シーン切り替え");
                    //RouletteUIManager.Instance.OpenRouletteUI(this);
                    
                }
            }
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A,D
        float v = Input.GetAxis("Vertical");   // W,S

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.Self);
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotateSpeed * Time.deltaTime);
    }
}