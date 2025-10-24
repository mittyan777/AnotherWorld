using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Chang : MonoBehaviour
{
    [SerializeField] private Image targetImage; // �ς���Image
    [SerializeField] private Sprite warriorSprite; // �E�킲�Ƃ̉摜
    [SerializeField] private Sprite mageSprite;
    [SerializeField] private Sprite archerSprite;

    // �{�^���������ꂽ���ɌĂԊ֐�
    public void ChangeToWarrior()
    {
        targetImage.sprite = warriorSprite;
    }

    public void ChangeToMage()
    {
        targetImage.sprite = mageSprite;
    }

    public void ChangeToArcher()
    {
        targetImage.sprite = archerSprite;
    }
    public void jobName(string jobName)
    {
        switch (jobName)
        {
            case "���m":
                targetImage.sprite = warriorSprite;
                break;
            case "���@":
                targetImage.sprite = mageSprite;
                break;
            case "�|�g��":
                targetImage.sprite = archerSprite;
                break;
            default:
                Debug.LogWarning("" + jobName);
                break;
        }
    }
}