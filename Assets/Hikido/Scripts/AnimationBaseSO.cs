using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationBaseSO", menuName = "AnimationBaseSO")]

public abstract class AnimationBaseSO : ScriptableObject, IAnimationCmd
{
    //パラメータネーム列挙体
    [SerializeField] protected ParaMatorName targetParm;

    [SerializeField] protected float floatValue;

    public  abstract void Execute(Animator animator);

}
