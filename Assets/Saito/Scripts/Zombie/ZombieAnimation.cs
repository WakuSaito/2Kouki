using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̃A�j���[�V�������s���N���X
/// </summary>
public class ZombieAnimation : ZombieBase
{
    [SerializeField]//�A�j���[�^�[
    private Animator animator;

    //�ړ��A�j���[�V�����̎��
    enum MoveType
    { 
        IDLE,
        WALK,
        RUN
    }

    //���݂̈ړ��A�j���[�V����
    MoveType currentMoveType;


    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpZombie()
    {
        currentMoveType = MoveType.WALK;
    }

    public void Attack()
    {
        Debug.Log("zombie:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.WALK) return;
        currentMoveType = MoveType.WALK;

        Debug.Log("zombie:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.IDLE) return;
        currentMoveType = MoveType.IDLE;

        Debug.Log("zombie:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.RUN) return;
        currentMoveType = MoveType.RUN;

        Debug.Log("zombie:Run");
        animator.SetTrigger("Run");
    }
}
