using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>犬アニメーションクラス</para>
/// 犬のアニメーションを管理する
/// </summary>
public class DogAnimation : DogBase
{
    [SerializeField]//アニメーター
    private Animator m_animator;

    /// <summary>
    /// 移動アニメーションの種類
    /// Managerの方で管理してもいいかも
    /// </summary>
    enum MOVE_TYPE
    {
        IDLE,
        WALK,
        RUN
    }

    //現在の移動アニメーション
    private MOVE_TYPE mCurrentMoveType;


    /// <summary>
    /// <para>初期設定</para>
    /// アニメーションの初期状態設定
    /// </summary>
    public override void SetUpDog()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;
    }

    /// <summary>
    /// <para>アタック</para>
    /// 攻撃アニメーション再生
    /// </summary>
    public void Attack()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Attack");
        m_animator.SetTrigger("Attack");
    }

    /// <summary>
    /// <para>アイドル状態</para>
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
    /// <para>歩き</para>
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
    /// <para>走り</para>
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

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        m_animator.speed = 0;
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume()
    {
        m_animator.speed = 1;
    }
}
