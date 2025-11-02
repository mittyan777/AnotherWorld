using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Changimage : MonoBehaviour
{
    [SerializeField] private Image targetImage; // •Ï‚¦‚éImage
    [SerializeField] private Sprite warriorSprite; // Eí‚²‚Æ‚Ì‰æ‘œ
    [SerializeField] private Sprite mageSprite;
    [SerializeField] private Sprite archerSprite;

    public void HideJobImage()  //job‚Ì”wŒi•”•ª‚ğ”ñ•\¦
    {
        if (targetImage != null)
        {
            {
                targetImage.gameObject.SetActive(false);
            }
        }
    }

    // ƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚½‚ÉŒÄ‚ÔŠÖ”

    public void jobName(string jobName)
    {

        if(jobName == "Œ•m")
        {
            targetImage.sprite = warriorSprite;
        }
        else if (jobName == "–‚–@")
        {
            targetImage.sprite = mageSprite;
        }
        else if (jobName == "‹|g‚¢")
        {
            targetImage.sprite = archerSprite;
        }
        Debug.Log("ó‚¯æ‚Á‚½E‹Æ" + jobName);
    }
}