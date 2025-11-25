using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationCmd 
{
    /// <summary>　/// アニメーション実行　/// </summary>
    /// <param name="animator"></param>
   void Execute(Animator animator);
}
