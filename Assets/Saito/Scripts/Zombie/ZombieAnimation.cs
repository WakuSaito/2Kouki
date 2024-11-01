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
        RUN
    }

    //現在の移動アニメーション
    MoveType currentMoveType;


    //アニメーターオブジェクトのローカルTransformをリセットするコルーチン
    //攻撃モーションが徐々に回転,移動してしまうため
    //ただ明らかに違和感（ワープ）がある
    IEnumerator ResetLocalTransform(float _sec = 0)
    {
        yield return new WaitForSeconds(_sec);

        animatorObj.transform.localRotation = Quaternion.identity;
        animatorObj.transform.localPosition = Vector3.zero;
    }

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

        //アニメーション終了時にローカルトランスフォームのリセット
        StartCoroutine(ResetLocalTransform(2.5f));
    }

    public void Walk()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.WALK) return;
        currentMoveType = MoveType.WALK;

        Debug.Log("zombie:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.IDLE) return;
        currentMoveType = MoveType.IDLE;

        //アニメーション終了時にローカルトランスフォームのリセット
        StartCoroutine(ResetLocalTransform(0));

        Debug.Log("zombie:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //同じアニメーションを複数呼び出ししないように
        if (currentMoveType == MoveType.RUN) return;
        currentMoveType = MoveType.RUN;

        Debug.Log("zombie:Run");
        animator.SetTrigger("Run");
    }
    public void Die()
    {
        Debug.Log("zombie:Die");
        animator.SetTrigger("Die");
    }
}
