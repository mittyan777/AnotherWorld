using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Chang : MonoBehaviour
{
    [SerializeField] private Image targetImage; // •Ï‚¦‚éImage
    [SerializeField] private Sprite warriorSprite; // Eí‚²‚Æ‚Ì‰æ‘œ
    [SerializeField] private Sprite mageSprite;
    [SerializeField] private Sprite archerSprite;

    // ƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚½‚ÉŒÄ‚ÔŠÖ”
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
            case "Œ•m":
                targetImage.sprite = warriorSprite;
                break;
            case "–‚–@":
                targetImage.sprite = mageSprite;
                break;
            case "‹|g‚¢":
                targetImage.sprite = archerSprite;
                break;
            default:
                Debug.LogWarning("" + jobName);
                break;
        }
    }
}