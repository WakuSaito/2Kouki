using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビのアニメーションを行うクラス
/// </summary>
public class ZombieAnimation : ZombieBase
{
    [SerializeField]//アニメーター
    private Animator animator;

    //移動アニメーションの種類
    enum MoveType
    { 
        IDLE,
        WALK,
        RUN
    }

    //現在の移動アニメーション
    MoveType correntMoveType;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpZombie()
    {
        correntMoveType = MoveType.WALK;
    }

    public void Attack()
    {
        Debug.Log("animation:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //同じアニメーションを複数呼び出ししないように
        if (correntMoveType == MoveType.WALK) return;
        correntMoveType = MoveType.WALK;

        Debug.Log("animation:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //同じアニメーションを複数呼び出ししないように
        if (correntMoveType == MoveType.IDLE) return;
        correntMoveType = MoveType.IDLE;

        Debug.Log("animation:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //同じアニメーションを複数呼び出ししないように
        if (correntMoveType == MoveType.RUN) return;
        correntMoveType = MoveType.RUN;

        Debug.Log("animation:Run");
        animator.SetTrigger("Run");
    }
}
