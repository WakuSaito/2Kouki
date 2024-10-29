using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMove : DogBase
{
    [SerializeField]//���鑬�x
    float run_speed = 6.0f;
    [SerializeField]//�������x
    float walk_speed = 3.0f;

    //�ڕW�Ƃ������
    Quaternion targetRotation;

    Rigidbody rb;

    //�����ݒ�
    public override void SetUpDog()
    {
        //rigidbody�̎擾
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        //���܂�Manager�ȊO��Update���g�������Ȃ����A��Ԃ��邽�ߎ���
        //��������
        var qua = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);

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
        targetRotation = _qua;//�ڕW�̌�����ݒ�
    }

    /// <summary>
    /// �w�肵�����W�Ɍ�����ύX
    /// </summary>
    public void LookAtPosition(Vector3 _targetPos)
    {
        //���W�̎擾
        Vector3 pos = transform.position;
        Vector3 target_pos = _targetPos;
        //�x�N�g�����v�Z
        Vector3 direction = target_pos - pos;
        direction.y = 0;//y�����l�����Ȃ�

        //�x�N�g��������������߂�
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        //�����̕ύX
        //���炩�Ɍ�����ύX�ł���悤�ɂ�����
        ChangeDirection(targetRotation);
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
        rb.velocity = vec * run_speed;
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
        rb.velocity = vec * walk_speed;
    }

    /// <summary>
    /// �ړ���~
    /// </summary>
    public void StopMove()
    {
        //�ړ��x�N�g����0�ɂ���
        rb.velocity = Vector3.zero;
    }
}
