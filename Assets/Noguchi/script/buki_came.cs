using UnityEngine;

public class CameraApproach : MonoBehaviour
{
    [Header("対象設定")]
    public string targetTag = "syounin";
    private Transform target;

    [Header("移動パラメータ")]
    public float moveSpeed = 3f;           // 移動スピード
    public float stopThreshold = 0.05f;    // 停止距離

    [Header("カメラ位置設定")]
    public float cameraHeight = 2f;        // カメラの高さ(Y)
    public float finalDistance = 3f;       // Z方向の距離
    public float sideOffset = -2f;         // X方向のズレ（左がマイナス、右がプラス）

    private bool isApproaching = false;
    private bool isReturning = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 targetPosition;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
        if (obj != null)
        {
            target = obj.transform;
        }
        else
        {
            Debug.LogWarning($"Tag '{targetTag}' が見つかりません。");
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (target == null) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartApproach();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartReturn();
        }

        if (isApproaching)
        {
            Vector3 basePos = target.position;
            // X方向に少しズラす（左寄りから撮影）
            targetPosition = new Vector3(
                basePos.x + sideOffset,   // ← このオフセットでキャラが右側に映る
                cameraHeight,
                basePos.z - finalDistance
            );

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < stopThreshold)
            {
                isApproaching = false;
            }
        }

        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, originalPosition) < stopThreshold)
            {
                isReturning = false;
            }
        }
    }

    void StartApproach()
    {
        if (isReturning) return;
        isApproaching = true;
        Debug.Log("カメラがsyouninに右寄せ構図で近づきます...");
    }

    void StartReturn()
    {
        if (isApproaching) return;
        isReturning = true;
        Debug.Log("カメラが元の位置に戻ります...");
    }
}
