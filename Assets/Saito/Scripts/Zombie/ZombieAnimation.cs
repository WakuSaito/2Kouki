using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̃A�j���[�V�������s���N���X
/// </summary>
public class ZombieAnimation : ZombieBase
{
    [SerializeField]//�A�j���[�^�[
    private Animator m_animator;

    [SerializeField]//�q�_���[�W�G�t�F�N�g
    private GameObject m_damagedEffect;

    //�ړ��A�j���[�V�����̎��
    enum MOVE_TYPE
    { 
        IDLE,
        WALK,
        RUN,
        DIE
    }

    //���݂̈ړ��A�j���[�V����
    MOVE_TYPE m_currentMoveType;

    /// <summary>
    /// �����ݒ�
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
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.WALK) return;
        m_currentMoveType = MOVE_TYPE.WALK;

        Debug.Log("zombie:Walk");
        m_animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.IDLE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("zombie:Idle");
        m_animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.RUN) return;
        m_currentMoveType = MOVE_TYPE.RUN;

        Debug.Log("zombie:Run");
        m_animator.SetTrigger("Run");
    }

    //��_���[�W���[�V����
    //���E�ɂ���ĕύX
    public void DamageHitLeft()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageL");
    }
    public void DamageHitRight()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageR");
    }


    public void Die()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.DIE;

        Debug.Log("zombie:Die");
        m_animator.SetTrigger("Die");
    }

    //�_���[�W�p�[�e�B�N���\��(�_���[�W���󂯂��ʒu)
    public void DamagedEffect(Vector3 _damaged_place)
    {
        if (m_damagedEffect == null) return;

        //�O���Ɍ�������
        Vector3 vec = _damaged_place - transform.position;

        GameObject effect = Instantiate(m_damagedEffect,
            _damaged_place, 
            Quaternion.identity
            );
    }
}
