using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���A�j���[�V�����N���X
/// ���̃A�j���[�V�������Ăяo��
/// </summary>
public class DogAnimation : DogBase
{
    [SerializeField]//�A�j���[�^�[
    private Animator m_animator;

    //�ړ��A�j���[�V�����̎��
    //Manager�̕��ŊǗ����Ă���������
    enum MOVE_TYPE
    {
        IDLE,
        WALK,
        RUN
    }

    //���݂̈ړ��A�j���[�V����
    private MOVE_TYPE mCurrentMoveType;


    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpDog()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;
    }

    /// <summary>
    /// �A�^�b�N
    /// �U���A�j���[�V�����Đ�
    /// </summary>
    public void Attack()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Attack");
        m_animator.SetTrigger("Attack");
    }

    /// <summary>
    /// �����~�܂�
    /// 
    /// </summary>
    public void Idle()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (mCurrentMoveType == MOVE_TYPE.IDLE) return;
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Idle");
        m_animator.SetTrigger("Idle");
    }

    /// <summary>
    /// ����
    /// 
    /// </summary>
    public void Walk()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (mCurrentMoveType == MOVE_TYPE.WALK) return;
        mCurrentMoveType = MOVE_TYPE.WALK;

        Debug.Log("dog:Walk");
        m_animator.SetTrigger("Walk");
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Run()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (mCurrentMoveType == MOVE_TYPE.RUN) return;
        mCurrentMoveType = MOVE_TYPE.RUN;

        Debug.Log("dog:Run");
        m_animator.SetTrigger("Run");
    }

}
