using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMove : DogBase
{
    [SerializeField]//���鑬�x
    float mRunSpeed = 6.0f;
    [SerializeField]//�������x
    float mWalkSpeed = 3.0f;

    [SerializeField]//�U��������x
    float mTurnSpeed = 1000;

    //�ڕW�Ƃ������
    Quaternion mTargetRotation;

    Rigidbody mRigidbody;

    //�����ݒ�
    public override void SetUpDog()
    {
        //rigidbody�̎擾
        mRigidbody = GetComponent<Rigidbody>();
        mTargetRotation = transform.rotation;
    }

    private void Update()
    {
        //���܂�Manager�ȊO��Update���g�������Ȃ����A��Ԃ��邽�ߎ���
        //��������
        var qua = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mTurnSpeed * Time.deltaTime);

        //y���ȊO�𖳎�
        qua.x = 0.0f; qua.z = 0.0f;
        //�����ύX
        transform.rotation = qua;
    }

    /// <summary>
    /// �w������ɕύX
    /// </summary>
    public void ChangeDirection(Quaternion _qua)
    {
        mTargetRotation = _qua;//�ڕW�̌�����ݒ�
    }

    /// <summary>
    /// �w�肵�����W�Ɍ�����ύX
    /// </summary>
    public void LookAtPosition(Vector3 _target_pos)
    {
        //���W�̎擾
        Vector3 pos = transform.position;
        Vector3 target_pos = _target_pos;
        //�x�N�g�����v�Z
        Vector3 direction = target_pos - pos;
        direction.y = 0;//y�����l�����Ȃ�

        //�x�N�g��������������߂�
        Quaternion target_rotation = Quaternion.LookRotation(direction, Vector3.up);

        //�����̕ύX
        //���炩�Ɍ�����ύX�ł���悤�ɂ�����
        ChangeDirection(target_rotation);
    }

    /// <summary>
    /// �O���ɑ���
    /// </summary>
    public void RunFront()
    {
        //�ړ����������߂�
        Vector3 vec = transform.forward;
        vec.y = 0.0f;//y���𖳎�����
        Vector3.Normalize(vec);

        //�ړ��x�N�g���X�V
        mRigidbody.velocity = new Vector3(vec.x * mRunSpeed, mRigidbody.velocity.y, vec.z * mRunSpeed);
    }

    /// <summary>
    /// �O���ɕ���
    /// </summary>
    public void WalkFront()
    {
        //�ړ����������߂�
        Vector3 vec = transform.forward;
        vec.y = 0.0f;//y���𖳎�����
        Vector3.Normalize(vec);

        //�ړ��x�N�g���X�V
        mRigidbody.velocity = new Vector3(vec.x * mWalkSpeed, mRigidbody.velocity.y, vec.z * mWalkSpeed);
    }

    /// <summary>
    /// �ړ���~
    /// </summary>
    public void StopMove()
    {
        //�ړ��x�N�g����0�ɂ���
        mRigidbody.velocity = new Vector3(0, mRigidbody.velocity.y, 0);
    }
}
