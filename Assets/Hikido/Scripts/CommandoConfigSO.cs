using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName ="CommandConfigSO",menuName = "CommandConfigSO")]
public class CommandoConfigSO : ScriptableObject
{
    [System.Serializable] public class CommandSet 
    {
        public JobType JobType;

        //各アニメーションごとのコマンドデータ
        //[Header("イベント別のコマンド")]
            
    }

    //コマンド設定リスト
    public CommandSet[] commandSets;
}
