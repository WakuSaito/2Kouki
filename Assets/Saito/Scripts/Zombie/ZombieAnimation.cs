using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̃A�j���[�V�������s���N���X
/// </summary>
public class ZombieAnimation : ZombieBase
{
    //�A�j���[�^�[
    [SerializeField] private Animator m_animator;

    //��_���[�W�G�t�F�N�g
    [SerializeField] private GameObject m_damagedEffect;

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

    /// <summary>
    /// �U���A�j���[�V�����Đ�
    /// </summary>
    public void Attack()
    {
        Debug.Log("zombie:Attack");
        m_animator.SetTrigger("Attack");
    }
    /// <summary>
    /// ���s�A�j���[�V�����Đ�
    /// </summary>
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
    /// <summary>
    /// �A�C�h�����A�j���[�V�����Đ�
    /// </summary>
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
    /// <summary>
    /// ����A�j���[�V�����Đ�
    /// </summary>
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
    /// <summary>
    /// ��_���[�W�A�j���[�V����(��)�Đ�
    /// </summary>
    public void DamageHitLeft()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageL");
    }
    /// <summary>
    /// ��_���[�W�A�j���[�V����(�E)�Đ�
    /// </summary>
    public void DamageHitRight()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.IDLE;

        m_animator.SetTrigger("DamageR");
    }

    /// <summary>
    /// ���S�A�j���[�V�����Đ�
    /// </summary>
    public void Die()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (m_currentMoveType == MOVE_TYPE.DIE) return;
        m_currentMoveType = MOVE_TYPE.DIE;

        Debug.Log("zombie:Die");
        m_animator.SetTrigger("Die");
    }

    /// <summary>
    /// �_���[�W�G�t�F�N�g
    /// ��_���[�W�ʒu�ɃG�t�F�N�g�𐶐�
    /// </summary>
    /// <param name="_damaged_place">�_���[�W���󂯂��ʒu</param>
    public void DamagedEffect(Vector3 _damaged_place)
    {
        if (m_damagedEffect == null) return;

        //�O���Ɍ�������
        //Vector3 vec = _damaged_place - transform.position;

        GameObject effect = Instantiate(m_damagedEffect,
            _damaged_place, 
            Quaternion.identity
            );
    }
}
