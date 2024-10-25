using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMove : DogBase
{
    [SerializeField]//���鑬�x
    float run_speed = 6.0f;
    [SerializeField]//�������x
    float walk_speed = 1.0f;

    Rigidbody rb;

    //�����ݒ�
    public override void SetUpDog()
    {
        //rigidbody�̎擾
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �w������ɕύX
    /// </summary>
    public void ChangeDirection(Quaternion _qua)
    {
        //y���ȊO�𖳎�
        _qua.x = 0.0f; _qua.z = 0.0f;
        //�����ύX
        transform.rotation = _qua;
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g�̕����Ɍ�����ύX
    /// </summary>
    public void LookAtObject(GameObject _targetObj)
    {
        if (_targetObj == null) return;//null�`�F�b�N

        //���W�̎擾
        Vector3 pos = transform.position;
        Vector3 target_pos = _targetObj.transform.position;
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
    /// �ړ���~
    /// </summary>
    public void StopMove()
    {
        //�ړ��x�N�g����0�ɂ���
        rb.velocity = Vector3.zero;
    }
}
