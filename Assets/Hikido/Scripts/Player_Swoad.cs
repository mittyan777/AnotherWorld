using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Swoad : PlayerAtackBase
{
    //攻撃カウントの定数
    private const float ATTACKCOUNT = 10.0f;

    [SerializeField] float inputwaitTime = 0.0f;
    bool isAttack = false;
    bool isAttackEnd = false;
 
     

    // Start is called before the first frame update
    void Start()
    {
        inputwaitTime = ATTACKCOUNT;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:攻撃開始時->Attackがtrueになった時にカウントを減らしていく
        if (isAttack) 
        { 
            inputwaitTime -= 1.0f;
            if(inputwaitTime < 0.0f) 
            {
                isAttackEnd = true; 
            }
        }

        
    }

    private void SwordSkill()
    {
        //左クリック入力状況
        bool _bisAttack = Input.GetMouseButton(0);
        float _inputTime = 0.0f;

        if (_bisAttack) { _inputTime += Time.deltaTime; }

        if(isAttack && _inputTime >= inputwaitTime)
        {
            _inputTime = 0;

            //TODO:多段攻撃の処理
        }


    }

}
