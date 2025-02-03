using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビのアニメーションを行うクラス
/// </summary>
public class ZombieAnimation : ZombieBase
{
    //アニメーター
    [SerializeField] private Animator m_animator;

    //被ダメージエフェクト
    [SerializeField] private GameObject m_damagedEffect;

    //移動アニメーションの種類
    enum MOVE_TYPE
    { 
        IDLE,
        WALK,
        RUN,
        DIE
    }

    //現在の移動アニメーション
    MOVE_TYPE m_currentMoveType;

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpZombie()
    {
        m_currentMoveType = MOVE_TYPE.WALK;
    }

    /// <summary>
    /// 攻撃アニメーション再生
    /// </summary>
    public void Attack()
    {
        Debug.Log("zombie:Attack");
        m_animator.SetTrigger("Attack");
    }
    /// <summary>
    /// 歩行アニメーション再生
    /// </summary>
    public void Walk()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        //同じアニメーションを複数呼び出ししないように
        if (m_currentMoveType == MOVE_TYPE.WALK) return;
        m_currentMoveType = MOVE_TYPE.WALK;

        Debug.Log("zombie:Walk");
        m_animator.SetTrigger("Walk");
    }
    /// <summary>
    /// アイドル時アニメーション再生
    /// </summary>
    public void Idle()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        //同じアニメーションを複数呼び出ししないように
        if (m_currentMoveType == MOVE_TYPE.IDLE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("zombie:Idle");
        m_animator.SetTrigger("Idle");
    }
    /// <summary>
    /// 走りアニメーション再生
    /// </summary>
    public void Run()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        //同じアニメーションを複数呼び出ししないように
        if (m_currentMoveType == MOVE_TYPE.RUN) return;
        m_currentMoveType = MOVE_TYPE.RUN;

        Debug.Log("zombie:Run");
        m_animator.SetTrigger("Run");
    }

    //被ダメージモーション
    //左右によって変更
    /// <summary>
    /// 被ダメージアニメーション(左)再生
    /// </summary>
    public void DamageHitLeft()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageL");
    }
    /// <summary>
    /// 被ダメージアニメーション(右)再生
    /// </summary>
    public void DamageHitRight()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageR");
    }

    /// <summary>
    /// 死亡アニメーション再生
    /// </summary>
    public void Die()
    {
        //同じアニメーションを複数呼び出ししないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.DIE;

        Debug.Log("zombie:Die");
        m_animator.SetTrigger("Die");
    }

    /// <summary>
    /// ダメージエフェクト
    /// 被ダメージ位置にエフェクトを生成
    /// </summary>
    /// <param name="_damaged_place">ダメージを受けた位置</param>
    public void DamagedEffect(Vector3 _damaged_place)
    {
        if (m_damagedEffect == null) return;

        //外側に向けたい
        //Vector3 vec = _damaged_place - transform.position;

        GameObject effect = Instantiate(m_damagedEffect,
            _damaged_place, 
            Quaternion.identity
            );
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
