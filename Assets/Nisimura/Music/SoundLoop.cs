using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoop : MonoBehaviour
{
    public AudioSource myAudio;

    void Start()
    {
        // ƒ‹[ƒv‚ğ—LŒø‚É‚·‚é
        myAudio.loop = true;
        // Ä¶
        myAudio.Play();
    }
}
