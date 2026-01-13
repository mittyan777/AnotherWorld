using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum RouletteSoundType
{
    CoinUse,  // コイン消費
    Spin,     // ルーレット回転
    Stop,     // 停止音
    Open      // ルーレット開く音
}
public class Roulette_Sound : MonoBehaviour
{


    [Header("ルーレット音")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] SoundClips;

    public RouletteSoundType rouletteSoundType;

    private bool isSpinPlaying = false;

    public bool IsSpinPlaying()
    {
        return isSpinPlaying;
    }


    public void PlaySound()
    {
        Debug.Log(rouletteSoundType);

        if (audioSource == null || SoundClips == null) return;

        int index = (int)rouletteSoundType;

        if (index >= SoundClips.Length || SoundClips[index] != null) return;

        if (rouletteSoundType == RouletteSoundType.Spin)    //音の種類_1
        {
            if (!isSpinPlaying)
            {
                isSpinPlaying = true;
                audioSource.loop = true;
                audioSource.clip = SoundClips[index];
                audioSource.Play();

            }
            return;

        }

        if (rouletteSoundType == RouletteSoundType.Stop)    //音の種類_2
        {
            if (isSpinPlaying)
            {
                audioSource.loop = false;
                audioSource.Stop(); // Spinのループを停止
                isSpinPlaying = false;
            }
            audioSource.PlayOneShot(SoundClips[index]);

            return;
        }

        // -------------------------------
        // CoinUse / Open は通常の一回再生
        // -------------------------------
        audioSource.PlayOneShot(SoundClips[index]);


    }
}
