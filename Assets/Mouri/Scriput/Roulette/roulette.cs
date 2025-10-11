using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roulette : MonoBehaviour
{

    //���[���b�g�̃V�X�e����v���O����     //�X���b�g�̐����������_���ŏo���A�ϐ��ɉ��������Ȃǂ𐧌䂵�Ă���   //�{�^�����������ۂɉ�ʂ�����
    [SerializeField]int power = 0;
    bool powerslot = true;

    [SerializeField] Text powerslot_text;

    [SerializeField] NewPlayer player;

    [SerializeField] GameObject slotUI;

    [SerializeField] int PMIN_Text;

    [SerializeField] int PMAX_Text;

    [SerializeField] float tienn;   //���[���b�g��ʂ�������܂ł̎���
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (powerslot)
        {
            power = Random.Range(PMIN_Text, PMAX_Text);

        }

        powerslot_text.text=power.ToString();
    }
    private void FixedUpdate()
    {
       
        if (powerslot == true) { power = Random.Range(PMIN_Text, PMAX_Text); }
       
        powerslot_text.text = ($"{power}");
    }
    public void stopslot()  //�X���b�g���X�g�b�v����v���O����
    {
        powerslot = false;

        StartCoroutine(CloseSlotAfterDelay(tienn));     //�uStartCoroutine�v���̊֐��Ŏ��Ԃ𐔂��n�߂�֐��i�^�C�}�[�̃X�^�[�g�{�^���H�j�݂����Ȗ���
    }

    IEnumerator CloseSlotAfterDelay(float delay)        //�X�^�[�g���Ă���ԂɁA�R���[�`���i���Ԃ��g������b���𐔂����肷��v���O�����j�ǂ�Ȏ��Ԃ̃v���O�����̒��̏��������邩�������Ă���A�ꎞ��~�⎞�Ԃ̃v���O�������ĊJ�ł���IEn...���Ԃ��܂����֐�
    {
        yield return new WaitForSeconds(delay);     //�ꎞ��~�����Ă��炤�v���O�����w��       
        slotUI.SetActive(false);
        player.Slotstop();
    }

    public void StartSlot()
    {
        power = 0;
        powerslot = true;
        powerslot_text.text = "0";
        slotUI.SetActive(true);
    }

}
