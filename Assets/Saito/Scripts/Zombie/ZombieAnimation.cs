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

    //�ړ��A�j���[�V�����̎��
    enum MoveType
    { 
        IDLE,
        WALK,
        RUN
    }

    //���݂̈ړ��A�j���[�V����
    MoveType currentMoveType;


    //�A�j���[�^�[�I�u�W�F�N�g�̃��[�J��Transform�����Z�b�g����R���[�`��
    //�U�����[�V���������X�ɉ�],�ړ����Ă��܂�����
    //�������炩�Ɉ�a���i���[�v�j������
    IEnumerator ResetLocalTransform(float _sec = 0)
    {
        yield return new WaitForSeconds(_sec);

        animatorObj.transform.localRotation = Quaternion.identity;
        animatorObj.transform.localPosition = Vector3.zero;
    }

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

        //�A�j���[�V�����I�����Ƀ��[�J���g�����X�t�H�[���̃��Z�b�g
        StartCoroutine(ResetLocalTransform(2.5f));
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

        //�A�j���[�V�����I�����Ƀ��[�J���g�����X�t�H�[���̃��Z�b�g
        StartCoroutine(ResetLocalTransform(0));

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
    public void Die()
    {
        Debug.Log("zombie:Die");
        animator.SetTrigger("Die");
    }
}
