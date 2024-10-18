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
    MoveType correntMoveType;


    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpZombie()
    {
        correntMoveType = MoveType.WALK;
    }

    public void Attack()
    {
        Debug.Log("animation:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (correntMoveType == MoveType.WALK) return;
        correntMoveType = MoveType.WALK;

        Debug.Log("animation:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (correntMoveType == MoveType.IDLE) return;
        correntMoveType = MoveType.IDLE;

        Debug.Log("animation:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (correntMoveType == MoveType.RUN) return;
        correntMoveType = MoveType.RUN;

        Debug.Log("animation:Run");
        animator.SetTrigger("Run");
    }
}
