using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̃A�j���[�V�������s���N���X
/// </summary>
public class ZombieAnimation : ZombieBase
{
    //�A�j���[�^�[�����Ă���I�u�W�F�N�g
    private GameObject animatorObj;

    [SerializeField]//�A�j���[�^�[
    private Animator animator;

    [SerializeField]//�q�_���[�W�G�t�F�N�g
    private GameObject damagedEffect;

    //�ړ��A�j���[�V�����̎��
    enum MoveType
    { 
        IDLE,
        WALK,
        RUN,
        DIE
    }

    //���݂̈ړ��A�j���[�V����
    MoveType currentMoveType;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpZombie()
    {
        //�A�j���[�^�[���A�^�b�`���Ă���I�u�W�F�N�g�擾
        animatorObj = animator.gameObject;

        currentMoveType = MoveType.WALK;
    }

    public void Attack()
    {
        Debug.Log("zombie:Attack");
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (currentMoveType == MoveType.DIE) return;
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.WALK) return;
        currentMoveType = MoveType.WALK;

        Debug.Log("zombie:Walk");
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (currentMoveType == MoveType.DIE) return;
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.IDLE) return;
        currentMoveType = MoveType.IDLE;

        Debug.Log("zombie:Idle");
        animator.SetTrigger("Idle");
    }

    public void Run()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (currentMoveType == MoveType.DIE) return;
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.RUN) return;
        currentMoveType = MoveType.RUN;

        Debug.Log("zombie:Run");
        animator.SetTrigger("Run");
    }

    //��_���[�W���[�V����
    //���E�ɂ���ĕύX
    public void DamageHitLeft()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (currentMoveType == MoveType.DIE) return;
        currentMoveType = MoveType.IDLE;

        animator.SetTrigger("DamageL");
    }
    public void DamageHitRight()
    {
        //���S��ɋN���オ��Ȃ��悤��
        if (currentMoveType == MoveType.DIE) return;
        currentMoveType = MoveType.IDLE;

        animator.SetTrigger("DamageR");
    }


    public void Die()
    {
        //�����A�j���[�V�����𕡐��Ăяo�����Ȃ��悤��
        if (currentMoveType == MoveType.DIE) return;
        currentMoveType = MoveType.DIE;

        Debug.Log("zombie:Die");
        animator.SetTrigger("Die");
    }

    //�_���[�W�p�[�e�B�N���\��(�_���[�W���󂯂��ʒu)
    public void DamagedEffect(Vector3 _damagedPlace)
    {
        if (damagedEffect == null) return;

        //�O���Ɍ�������
        Vector3 vec = _damagedPlace - transform.position;

        GameObject effect = Instantiate(damagedEffect, 
            _damagedPlace, 
            Quaternion.identity
            );
    }
}
