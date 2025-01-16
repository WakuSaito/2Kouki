using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビのアニメーションを行うクラス
/// </summary>
public class ZombieAnimation : ZombieBase
{
    [SerializeField]//アニメーター
    private Animator m_animator;

    [SerializeField]//ヒダメージエフェクト
    private GameObject m_damagedEffect;

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

    public void Attack()
    {
        Debug.Log("zombie:Attack");
        m_animator.SetTrigger("Attack");
    }

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
    public void DamageHitLeft()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageL");
    }
    public void DamageHitRight()
    {
        //死亡後に起き上がらないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageR");
    }


    public void Die()
    {
        //同じアニメーションを複数呼び出ししないように
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.DIE;

        Debug.Log("zombie:Die");
        m_animator.SetTrigger("Die");
    }

    //ダメージパーティクル表示(ダメージを受けた位置)
    public void DamagedEffect(Vector3 _damaged_place)
    {
        if (m_damagedEffect == null) return;

        //外側に向けたい
        Vector3 vec = _damaged_place - transform.position;

        GameObject effect = Instantiate(m_damagedEffect,
            _damaged_place, 
            Quaternion.identity
            );
    }
}
