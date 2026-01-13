using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip secondSound;

    private Vector3 mOffset;
    private float mZCoord;

    // --- ドラッグ機能の部分 ---
    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
    }

    // --- 当たった時の判定 ---
    private void OnTriggerEnter(Collider other)
    {
        // ターゲットに当たったら
        if (other.CompareTag("Target"))
        {
            if (audioSource.clip != secondSound)
            {
                audioSource.Stop();
                audioSource.clip = secondSound;
                audioSource.Play();
                Debug.Log("サウンドが切り替わりました！");
            }
        }
    }
}
