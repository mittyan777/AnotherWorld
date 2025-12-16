using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Sound : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;

    [SerializeField] private float fadeTime = 2.0f;

    private int last = -1;

    private void Start()
    {
        source.volume= 0f;

        StartCoroutine(PlayRandom());
    }

    IEnumerator PlayRandom()
    {
        while (true)    //街にいる限り永遠に繰り返す。
        {
            int index=Random.Range(0,clips.Length); //鳴らす音源を選ぶ（clips.Lemgh)これで配列の中に音源を指定

            if (index == last)  //last＝-1だから0の状態にリセットする
            {
               index= Random.Range(0, clips.Length) ;

            }

            last= index;
            
            source.clip = clips[index];
            
            float targetVolume= Random.Range(0.2f, 0.35f);
            source.pitch = Random.Range(0.95f, 1.05f);

            source.Play();

            //フェードイン
            yield return StartCoroutine(FadeVolume(0.05f,targetVolume));

            float waitTime = source.clip.length - fadeTime;
            if(waitTime < 0f)waitTime = 0f;

            yield return new WaitForSeconds(waitTime);

            //フェードアウト
            yield return StartCoroutine(FadeVolume(targetVolume,0.05f));

            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
        }
    }

    IEnumerator FadeVolume(float from,float to)
    {
        float time = 0f;

        while (time < fadeTime)
        {
            source.volume = Mathf.Lerp(from, to, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        source.volume = to;
    }
}
