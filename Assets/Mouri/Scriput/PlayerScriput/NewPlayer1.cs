using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer1 : MonoBehaviour  //プレイヤーの視界の限界値や、Rayの視覚化 //プレイヤーが操作できるかできないかを判断する部分
{

    //Animator animator;
    [SerializeField]
    public float moveSpeed = 5f;
    public float coin = 0;
    public bool canControl = true;  // ← 追加！(プレイヤーが操作をするかしないかの判断場所）
    //[SerializeField] GameObject RayPointer;
    [SerializeField] GameObject slot;
    [SerializeField] float Direction;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();  アニメーションを追加するときはコメントアウトを外す
        // Cursor.lockState = CursorLockMode.Locked;   //追加（escを押すとマウスカーソルを呼び出す）
        //Cursor.visible = false;     //追加
    }

    // Update is called once per frame
    void Update()
    {
        //RayPointer.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // ここで操作全部止まる




        // 前方にRayを飛ばして、ルーレット台に当たったら
        //Ray ray = new Ray(new Vector3(RayPointer.transform.position.x, RayPointer.transform.position.y, RayPointer.transform.position.z), RayPointer.transform.forward);
        //RaycastHit hit;



        //if (Physics.Raycast(ray, out hit, Direction)) //（Direction）でRayの距離を設定できる
        //{
        //    if (hit.collider.CompareTag("Roulette")) // ルーレット台のタグを"Roulette"にする
        //    {
        //        // 「E」キーでルーレットを調べる
        //        if (Input.GetKeyDown(KeyCode.E))
        //        {
        //            Debug.Log("ルーレットを調べた！シーン切り替え");
        //            slot.SetActive(true);
        //            //RouletteUIManager.Instance.OpenRouletteUI(this);

        //            canControl = false; //追加（操作停止をさせるかさせないかを判断する）

        //            var cameraController = Camera.main.GetComponent<MainCamera>();
        //            if (cameraController != null)
        //            {
        //                cameraController.enabled = false;
        //            }
        //        }

        //    }
        //}
        //Debug.DrawRay(ray.origin, ray.direction * Direction, Color.red);

    }
    private void FixedUpdate()
    {

        
        if (!canControl) return;        //追加（操作禁止時を判断している）
        Move();


        /*if (Input.GetKey("w"))
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
        else { animator.SetBool("right", false); }*/


    }
    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A,D
        float v = Input.GetAxis("Vertical");   // W,S

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            coin += 1;
            Destroy(other.gameObject);
        }
    }

    public void Slotstop()      //追加部分　スロット側でプログラムを呼び出す
    {
        canControl = true;
        slot.SetActive(false);

        var cameraController = Camera.main.GetComponent<MainCamera>();
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }
    }
}
