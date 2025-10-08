using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_mitubosi : MonoBehaviour
{
    Animator animator;  
    public float coin = 0;
    public bool canControl = true;  // ← 追加！(プレイヤーが操作をするかしないかの判断場所）
    [SerializeField] GameObject rayobj;
    [SerializeField] GameObject slot;
    [SerializeField] float Direction;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
       // Cursor.lockState = CursorLockMode.Locked;   //追加（escを押すとマウスカーソルを呼び出す）
        //Cursor.visible = false;     //追加
    }

    // Update is called once per frame
    void Update()
    {
       rayobj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // ここで操作全部止まる


      
        
            // 前方にRayを飛ばして、ルーレット台に当たったら
            Ray ray = new Ray(new Vector3(rayobj.transform.position.x, rayobj.transform.position.y, rayobj.transform.position.z ), rayobj.transform.forward);
            RaycastHit hit;



            if (Physics.Raycast(ray, out hit, Direction)) // 2m以内を調べる
            {
                if (hit.collider.CompareTag("Roulette")) // ルーレット台のタグを"Roulette"にする
                {
                    // 「E」キーでルーレットを調べる
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("ルーレットを調べた！シーン切り替え");
                        slot.SetActive(true);

                        canControl = false; //追加（操作停止をさせるかさせないかを判断する）

                        //RouletteUIManager.Instance.OpenRouletteUI(this);
                        var cameraController = Camera.main.GetComponent<PlayerCamera>();
                        if (cameraController != null)
                        {
                            cameraController.enabled = false;
                        }
                    }

                }
            }
            Debug.DrawRay(ray.origin, ray.direction * Direction, Color.red);
        
    }
    private void FixedUpdate()
    {
        if (!canControl) return;        //追加（操作禁止時を判断している）

        if(Input.GetKey("w"))
        {
            transform.position += transform.forward * 5 * Time.deltaTime;
            animator.SetBool("walk", true);
        }
        else { animator.SetBool("walk", false); }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * 5 * Time.deltaTime;
            animator.SetBool("back", true);
        }
        else { animator.SetBool("back", false); }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * 5 * Time.deltaTime;
            animator.SetBool("left", true);
        }
        else { animator.SetBool("left", false); }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            animator.SetBool("right", true);
        }
        else { animator.SetBool("right", false); }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            coin += 1;
            Destroy(other.gameObject);
        }
    }

    public void Slotstop()      //追加部分スロット側でプログラムを呼び出す
    {
        canControl = true;
        slot.SetActive(false);

        var cameraController = Camera.main.GetComponent<PlayerCamera>();
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }
    }
}
