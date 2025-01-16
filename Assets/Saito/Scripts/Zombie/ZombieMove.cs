using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̈ړ��p�N���X
/// </summary>
public class ZombieMove : ZombieBase
{
    [SerializeField]//���鑬�x
    float m_runSpeed = 6.0f;
    [SerializeField]//�������x
    float m_walkSpeed = 1.0f;

    [SerializeField]//�U��������x
    float m_turnSpeed = 1000;

    //�ڕW�Ƃ������
    Quaternion m_targetRotation;

    Rigidbody m_rigidbody;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpZombie()
    {
        //rigidbody�̎擾
        m_rigidbody = GetComponent<Rigidbody>();
        m_targetRotation = transform.rotation;
    }

    private void Update()
    {
        //���܂�Manager�ȊO��Update���g�������Ȃ����A��Ԃ��邽�ߎ���
        //��������
        var qua = Quaternion.RotateTowards(transform.rotation, m_targetRotation, m_turnSpeed * Time.deltaTime);

        //y���ȊO�𖳎�
        qua.x = 0.0f; qua.z = 0.0f;
        //�����ύX
        transform.rotation = qua;
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
        m_rigidbody.velocity = new Vector3(vec.x * m_walkSpeed, m_rigidbody.velocity.y, vec.z * m_walkSpeed);
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
        m_rigidbody.velocity = new Vector3(vec.x * m_runSpeed, m_rigidbody.velocity.y, vec.z * m_runSpeed);
    }

    /// <summary>
    /// �w������ɕύX
    /// </summary>
    public void ChangeDirection(Quaternion _qua)
    {
        m_targetRotation = _qua;//�ڕW�̌�����ݒ�
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
    /// �ړ���~
    /// </summary>
    public void StopMove()
    {
        //�ړ��x�N�g����0�ɂ���
        m_rigidbody.velocity = new Vector3(0, m_rigidbody.velocity.y,0);
    }
   
}
