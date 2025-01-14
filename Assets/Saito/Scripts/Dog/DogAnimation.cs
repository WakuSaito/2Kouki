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
    private Animator mAnimator;

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
        mAnimator.SetTrigger("Attack");
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
        mAnimator.SetTrigger("Idle");
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
        mAnimator.SetTrigger("Walk");
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
        mAnimator.SetTrigger("Run");
    }

}
