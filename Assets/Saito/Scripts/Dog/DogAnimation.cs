using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : DogBase
{
    [SerializeField]//アニメーター
    private Animator animator;
    //移動アニメーションの種類
    //Managerの方で管理してもいいかも
    enum MoveType
    {
        IDLE,
        WALK,
        RUN
    }

    //現在の移動アニメーション
    MoveType currentMoveType;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpDog()
    {
        currentMoveType = MoveType.IDLE;
    }

    public void Attack()
    {
        currentMoveType = MoveType.IDLE;

        Debug.Log("dog:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.WALK) return;
        currentMoveType = MoveType.WALK;

        Debug.Log("dog:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.IDLE) return;
        currentMoveType = MoveType.IDLE;

        Debug.Log("dog:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.RUN) return;
        currentMoveType = MoveType.RUN;

        Debug.Log("dog:Run");
        animator.SetTrigger("Run");
    }

}
