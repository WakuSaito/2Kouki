using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビのアニメーションを行うクラス
/// </summary>
public class ZombieAnimation : ZombieBase
{
    //アニメーターをつけているオブジェクト
    private GameObject animatorObj;

    [SerializeField]//アニメーター
    private Animator animator;

    //移動アニメーションの種類
    enum MoveType
    { 
        IDLE,
        WALK,
        RUN,
        DIE
    }

    //現在の移動アニメーション
    MoveType currentMoveType;

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpZombie()
    {
        //アニメーターをアタッチしているオブジェクト取得
        animatorObj = animator.gameObject;

        currentMoveType = MoveType.WALK;
    }

    public void Attack()
    {
        Debug.Log("zombie:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //死亡後に起き上がらないように
        if (currentMoveType == MoveType.DIE) return;
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.WALK) return;
        currentMoveType = MoveType.WALK;

        Debug.Log("zombie:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //死亡後に起き上がらないように
        if (currentMoveType == MoveType.DIE) return;
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.IDLE) return;
        currentMoveType = MoveType.IDLE;

        Debug.Log("zombie:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //死亡後に起き上がらないように
        if (currentMoveType == MoveType.DIE) return;
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.RUN) return;
        currentMoveType = MoveType.RUN;

        Debug.Log("zombie:Run");
        animator.SetTrigger("Run");
    }

    //被ダメージモーション
    //左右によって変更
    public void DamageHitLeft()
    {
        //死亡後に起き上がらないように
        if (currentMoveType == MoveType.DIE) return;
        currentMoveType = MoveType.IDLE;

        animator.SetTrigger("DamageL");
    }
    public void DamageHitRight()
    {
        //死亡後に起き上がらないように
        if (currentMoveType == MoveType.DIE) return;
        currentMoveType = MoveType.IDLE;

        animator.SetTrigger("DamageR");
    }


    public void Die()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.DIE) return;
        currentMoveType = MoveType.DIE;

        Debug.Log("zombie:Die");
        animator.SetTrigger("Die");
    }
}
