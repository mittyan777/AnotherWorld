using UnityEngine;

public class JobSetter : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;

    // テストしたいジョブをInspectorで設定できるようにする
    [SerializeField] private JobType initialJob = JobType.SWORDSMAN;

    // デバッグ用の一時的なジョブ切り替えキー
    [Header("デバッグ用キー")]
    [SerializeField] private KeyCode switchWizardKey = KeyCode.T;
    [SerializeField] private KeyCode switchArcherKey = KeyCode.U;

    void Start()
    {
        // ゲーム開始時に初期ジョブを設定
        playerAnimation.SetJobType(initialJob);
        Debug.Log($"初期ジョブが設定されました: {initialJob}");
    }

    void Update()
    {
        // 1キーでWIZARDに切り替え
        if (Input.GetKeyDown(switchWizardKey))
        {
            playerAnimation.SetJobType(JobType.WIZARD);
            Debug.Log("ジョブをWIZARDに切り替えました。");
        }

        // 2キーでARCHERに切り替え
        if (Input.GetKeyDown(switchArcherKey))
        {
            playerAnimation.SetJobType(JobType.ARCHER);
            Debug.Log("ジョブをARCHERに切り替えました。");
        }

    }
}