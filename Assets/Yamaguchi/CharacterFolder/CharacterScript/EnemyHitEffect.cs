using System.Collections;
using UnityEngine;

public class EnemyHitEffect : MonoBehaviour
{
    private Renderer[] renderers;
    //色のメモ帳(色を上書きするための変数:これは)
    private MaterialPropertyBlock propBlock;

    private Color[] defaultColors;//初期の色保存

    [SerializeField] private Color hitColor = Color.red; // ヒット時の色
    [SerializeField] private float duration = 0.2f;      // 色が変わる時間

    // シェーダーのプロパティ名
    [SerializeField] private string colorPropertyName;

    void Awake()
    {
        //子階層も含めてすべてのRendererを取得
        //自分の子オブジェクト(体のパーツ)すべてをリストアップ
        renderers = GetComponentsInChildren<Renderer>();
        //マテリアル設定の上書き用のメモ帳
        propBlock = new MaterialPropertyBlock();
    }

    // 攻撃が当たった時にこれを呼ぶ
    public void PlayerAttackHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }

    private IEnumerator HitCoroutine()
    {
        // 色をヒット色に変更
        EnemySetColor(hitColor, false);

        yield return new WaitForSeconds(duration);

        // 色を元に戻す（プロパティブロックをクリア）
        EnemySetColor(Color.white, true); // 白を掛けると元のテクスチャの色になります
    }

    private void EnemySetColor(Color color, bool isReset)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            // 配列のi番目のレンダラーを取り出す
            Renderer r = renderers[i];

            //現在のマテリアルの設定を上書き
            r.GetPropertyBlock(propBlock);

            //propBlockのcolorPropertyNameって言う場所の色を受け取った色にする
            if (isReset)
            {
                //メモを削除すれば自動的に初期の色に戻る
                propBlock.Clear();
            }
            else
            {
                //メモ帳に「この色で描画して」と書き込む
                propBlock.SetColor(colorPropertyName, color);
            }

            //書き換えたプロパティブロックをレンダラーに反映させる
            r.SetPropertyBlock(propBlock);
        }
    }
}