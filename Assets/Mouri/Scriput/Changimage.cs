using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Changimage : MonoBehaviour
{
    [SerializeField] private Image targetImage; // �ς���Image
    [SerializeField] private Sprite warriorSprite; // �E�킲�Ƃ̉摜
    [SerializeField] private Sprite mageSprite;
    [SerializeField] private Sprite archerSprite;

    // �{�^���������ꂽ���ɌĂԊ֐�
    public void ChangeToWarrior()
    {
        
    }

    public void ChangeToMage()
    {
        
    }

    public void ChangeToArcher()
    {
        
    }
    public void jobName(string jobName)
    {
       if(jobName == "���m")
       {
            targetImage.sprite = warriorSprite;
       }
       else if(jobName == "�|�g��")
       {
            targetImage.sprite = archerSprite;
       }
       else if( jobName == "���@")
       {
            targetImage.sprite = mageSprite;
       }
    }
}