using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : DogBase
{
    [SerializeField]//�A�j���[�^�[
    private Animator animator;
    //�ړ��A�j���[�V�����̎��
    //Manager�̕��ŊǗ����Ă���������
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
    public override void SetUpDog()
    {
        currentMoveType = MoveType.IDLE;
    }

    public void Attack()
    {
        currentMoveType = MoveType.IDLE;

        Debug.Log("dog:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.WALK) return;
        currentMoveType = MoveType.WALK;

        Debug.Log("dog:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.IDLE) return;
        currentMoveType = MoveType.IDLE;

        Debug.Log("dog:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.RUN) return;
        currentMoveType = MoveType.RUN;

        Debug.Log("dog:Run");
        animator.SetTrigger("Run");
    }

}
