using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_hikido : MonoBehaviour
{
    Animator animator;
    public float coin = 0;
    public float HP;
    public float MP;
    public float AttackStatus;
    public float DefenseStatus;
    public bool canControl = true;  // ← 追加！
    [SerializeField] GameObject rayobj;
    [SerializeField] GameObject slot;
    [SerializeField] float Direction;
    [SerializeField] GameObject shootposition;
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject Electric_ball;
    [SerializeField] GameObject[] manager;

    //[SerializeField] PlayerAtackBase attackBase;
    bool _avoidance = false;

    
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0f;
        animator = GetComponent<Animator>();
        //attackBase = GetComponent<PlayerAtackBase>();
        // Cursor.lockState = CursorLockMode.Locked;   //追加
        //Cursor.visible = false;     //追加
    }

    // Update is called once per frame
    void Update()
    {
        if (manager != null) { manager = GameObject.FindGameObjectsWithTag("GameManager"); }
        rayobj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // ここで操作全部止まる

        // 前方にRayを飛ばして、ルーレット台に当たったら
        Ray ray = new Ray(new Vector3(rayobj.transform.position.x, rayobj.transform.position.y, rayobj.transform.position.z), rayobj.transform.forward);
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
                    //RouletteUIManager.Instance.OpenRouletteUI(this);
                }

            }
        }
        Debug.DrawRay(ray.origin, ray.direction * Direction, Color.red);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (manager[0].GetComponent<GameManager>().Status[4] == 4 && manager[0].GetComponent<GameManager>().slot == false) { Instantiate(fireball, shootposition.transform.position, Quaternion.identity); }


        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (manager[0].GetComponent<GameManager>().Status[4] == 4 && manager[0].GetComponent<GameManager>().slot == false) { Instantiate(Electric_ball, shootposition.transform.position, Quaternion.identity); }
        }

        //attackBase.GetbAvoindance();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * 5 * Time.deltaTime;
            //_avoidance = attackBase.GetbAvoindance();
            if (_avoidance)
            {
                animator.SetBool("walk", true);
            }
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
        if (other.gameObject.tag == "coin")
        {
            coin += 1;
            Destroy(other.gameObject);
        }
    }
}
