using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CommandConfigSO",menuName = "CommandConfigSO")]
public class CommandoConfigSO : ScriptableObject
{
    [System.Serializable] public class CommandSet 
    {
        public JobType JobType;

        //各アニメーションごとのコマンドデータ
        [Header("イベント別のコマンド")]
        //通常攻撃コマンド
        public AnimationBaseSO normalAttackCd;
        //回避コマンド
        public AnimationBaseSO avoidCd;
        //アーチャースキルコマンド
        public AnimationBaseSO archerSkilsCd;

        //移動コマンド
        //上下左右移動用のアニメーションコマンド管理

        [Header("終了用コマンド")]
        //通常攻撃アニメーション終了用コマンド
        public AnimationBaseSO normalAttackEndCd;

        //回避終了用コマンド
        public AnimationBaseSO avoidAnimEndCd;

        //移動アニメーション終了用コマンド  -> 不要の場合は削除するあとで
        public AnimationBaseSO walkanimEndCd;

        //アーチャースキル終了用コマンド
        public AnimationBaseSO ArcherSkilsEndCd;

    }

    //コマンド設定リスト
    public CommandSet[] commandSets;
}
