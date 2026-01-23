using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [System.Serializable]
    public struct EffectEntry
    {
        public string label;           // 管理用名前
        public GameObject prefab;      // VFXのプレハブ
        public Transform spawnPoint;   // 出現場所（剣の先など）
        public bool parentToPoint;     // 親子関係にするか（軌跡ならtrue）
    }

    [SerializeField] private EffectEntry[] attackEffects;

    //アニメーションイベントからこれを呼ぶ
    public void ExecuteAttackEffect(int index)
    {
        if (index < 0 || index >= attackEffects.Length) return;

        var effect = attackEffects[index];
        if (effect.prefab == null) return;

        //生成
        GameObject vfxInstance = Instantiate(effect.prefab, effect.spawnPoint.position, effect.spawnPoint.rotation);

        if (effect.parentToPoint)
        {
            vfxInstance.transform.SetParent(effect.spawnPoint);
        }
    }
}
