using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̈ړ��p�N���X
/// </summary>
public class ZombieMove : ZombieBase
{
    [SerializeField]//���鑬�x
    float run_speed = 6.0f;
    [SerializeField]//�������x
    float walk_speed = 1.0f;

    Rigidbody rb;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpZombie()
    {
        //rigidbody�̎擾
        rb = GetComponent<Rigidbody>();
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
    /// �ړ���~
    /// </summary>
    public void StopMove()
    {
        //�ړ��x�N�g����0�ɂ���
        rb.velocity = Vector3.zero;
    }
   
}
