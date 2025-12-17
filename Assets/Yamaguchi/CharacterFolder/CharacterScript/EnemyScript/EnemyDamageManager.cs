using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    //敵のダメージ量
    [SerializeField] private int goblinAttackDamage01;
    [SerializeField] private int goblinAttackDamage03;
    [SerializeField] private int skeletonAttackDamage01;
    [SerializeField] private int skeletonAttackDamage03;
    [SerializeField] private int throwanleAttackDamage;
    [SerializeField] private int spiderAttackDamage01;
    [SerializeField] private int spiderAttackDamage02;
    [SerializeField] private int wolfAttackDamage;

    public int GoblinAttackDamage01 => goblinAttackDamage01;
    public int GoblinAttackDamage03 => goblinAttackDamage03;
    public int SkeletonAttackDamage01 => skeletonAttackDamage01;
    public int SkeletonAttackDamage03 => skeletonAttackDamage03;
    public int ThrowanleAttackDamage => throwanleAttackDamage; // 変数名とプロパティ名を揃えています
    public int SpiderAttackDamage01 => spiderAttackDamage01;
    public int SpiderAttackDamage02 => spiderAttackDamage02;
    public int WolfAttackDamage => wolfAttackDamage;

    private Dictionary<string, int> damageLookupTable;

    private void Awake()
    {
        // 辞書を初期化
        damageLookupTable = new Dictionary<string, int>
        {
            { "GoblinAttack01"  , goblinAttackDamage01 },
            { "GoblinAttack03"  , goblinAttackDamage03 },
            { "SkeletonAttack01", skeletonAttackDamage01 },
            { "SkeletonAttack03", skeletonAttackDamage03 },
            { "throwableAttack" , throwanleAttackDamage },
            { "spiderAttack01"  , spiderAttackDamage01 },
            { "spiderAttack02"  , spiderAttackDamage02 },
            { "WolfAttack01"    , wolfAttackDamage }
        };
    }
    public int GetDamageByTag(string tag)
    {
        if (damageLookupTable.TryGetValue(tag, out int damage))
        {
            return damage;
        }
        return 0; // タグが見つからなかった場合は 0 を返す
    }
}
