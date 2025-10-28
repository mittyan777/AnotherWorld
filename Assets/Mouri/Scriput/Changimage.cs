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

    // ƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚½‚ÉŒÄ‚ÔŠÖ”
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
       if(jobName == "Œ•m")
       {
            targetImage.sprite = warriorSprite;
       }
       else if(jobName == "‹|g‚¢")
       {
            targetImage.sprite = archerSprite;
       }
       else if( jobName == "–‚–@")
       {
            targetImage.sprite = mageSprite;
       }
    }
}