using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DorScripu : MonoBehaviour
{
    [SerializeField] private Transform player;      // �v���C���[��Transform
    [SerializeField] private Animator doorAnimator; // �h�A��Animator
    [SerializeField] private float openDistance ; // �J������
    [SerializeField] private Transform DistancePointer;//�h�A�̃|�W�V�����Ōv�Z���Ă��܂��Ƃ��ꂪ�����邽�ߋ�̃I�u�W�F�N�g�𗘗p���ăh�A�̌v�Z�𐳂����������邽�߂̕���

    [SerializeField] float distance;
    private bool isLocked = false;
   // private bool isPlayerRange = false;


    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
        // "Player" �^�O�̃I�u�W�F�N�g�������ŒT���Ď擾
        GameObject obj = GameObject.FindGameObjectWithTag("Player");//FindGameObjetWithTag�Ƃ��������̓^�O�����Ă��邱�̃Q�[���I�u�W�F�N�g��T���ĂƂ����Ӗ�
        if (obj != null)
        {
            player = obj.transform;
        }
        else
        {
            Debug.LogError("Player�^�O�̃I�u�W�F�N�g��������܂���I");
        }
    }

    private void Update()
    {
        if (isLocked) return;

         distance = Vector3.Distance(player.position,DistancePointer.position); //Vector3���ɂ���uDistance]���ĂыN�����Ă���

        Debug.Log("�����F" + distance);

        if (distance <= openDistance)
        {
           
            doorAnimator.SetBool("open", true);
            Debug.Log("�h�A���J��");
        }
        else 
        {
            doorAnimator.SetBool("open", false);
            
        }
    }
    public void LockDoor()
    {
        doorAnimator.SetBool("open", false);
        isLocked = true;
        Debug.Log("�h�A�����b�N");
    }

}
