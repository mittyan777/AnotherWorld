using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum RouletteSoundType_main
{
    CoinUse,  // コイン消費
    Spin,     // ルーレット回転
    Stop,     // 停止音
    Open      // ルーレット開く音
}
public class Roulette_Sound_main : MonoBehaviour
{
 
    
    [Header("ルーレット音")]
    [SerializeField]private  AudioSource audioSource;
    [SerializeField] private AudioClip []SoundClips;

    public RouletteSoundType rouletteSoundType;


    public void PlaySound()
    {
        Debug.Log(rouletteSoundType);

        if(audioSource == null|| SoundClips==null) return;

        int index = (int)rouletteSoundType;
        if (index >= SoundClips.Length && SoundClips[index] != null)return;
        {
            audioSource.PlayOneShot(SoundClips[index]);
        }

        if (rouletteSoundType == RouletteSoundType.Spin)
        {
            audioSource.loop = true;
            audioSource.clip = SoundClips[index];
            audioSource.Play();
            return;
        }

        if (rouletteSoundType == RouletteSoundType.Stop)
        {
            audioSource.loop = false;
            audioSource.Stop(); // Spinのループを停止

            audioSource.PlayOneShot(SoundClips[index]);
            return;
        }

        // -------------------------------
        // CoinUse / Open は通常の一回再生
        // -------------------------------
        audioSource.PlayOneShot(SoundClips[index]);


    }
}
