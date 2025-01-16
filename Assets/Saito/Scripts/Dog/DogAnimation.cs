using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 犬アニメーションクラス
/// 犬のアニメーションを管理する
/// </summary>
public class DogAnimation : DogBase
{
    [SerializeField]//アニメーター
    private Animator m_animator;

    //移動アニメーションの種類
    //Managerの方で管理してもいいかも
    enum MOVE_TYPE
    {
        IDLE,
        WALK,
        RUN
    }

    //現在の移動アニメーション
    private MOVE_TYPE mCurrentMoveType;


    /// <summary>
    /// 初期設定
    /// アニメーションの初期状態設定
    /// </summary>
    public override void SetUpDog()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;
    }

    /// <summary>
    /// アタック
    /// 攻撃アニメーション再生
    /// </summary>
    public void Attack()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Attack");
        m_animator.SetTrigger("Attack");
    }

    /// <summary>
    /// アイドル状態
    /// アイドル状態アニメーション再生
    /// </summary>
    public void Idle()
    {
        //同じアニメーションを複数呼び出ししないように
        if (mCurrentMoveType == MOVE_TYPE.IDLE) return;
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Idle");
        m_animator.SetTrigger("Idle");
    }

    /// <summary>
    /// 歩き
    /// 歩きアニメーション再生
    /// </summary>
    public void Walk()
    {
        //同じアニメーションを複数呼び出ししないように
        if (mCurrentMoveType == MOVE_TYPE.WALK) return;
        mCurrentMoveType = MOVE_TYPE.WALK;

        Debug.Log("dog:Walk");
        m_animator.SetTrigger("Walk");
    }

    /// <summary>
    /// 走り
    /// 走りアニメーション再生
    /// </summary>
    public void Run()
    {
        //同じアニメーションを複数呼び出ししないように
        if (mCurrentMoveType == MOVE_TYPE.RUN) return;
        mCurrentMoveType = MOVE_TYPE.RUN;

        Debug.Log("dog:Run");
        m_animator.SetTrigger("Run");
    }

}
