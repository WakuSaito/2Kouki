using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>���ړ��N���X</para>
/// ���̈ړ����]���Ǘ������s����
/// </summary>
public class DogMove : DogBase
{   
    [SerializeField] float m_walkSpeed = 3.0f;//�������x    
    [SerializeField] float m_runSpeed  = 6.0f;//���鑬�x   
    [SerializeField] float m_turnSpeed = 1000f;//�U��������x

    //�ڕW�Ƃ������
    Quaternion m_targetRotation;

    Rigidbody m_rigidbody;

    //�����ݒ�
    //�R���|�[�l���g�A������Ԃ̎擾
    public override void SetUpDog()
    {
        //rigidbody�̎擾
        m_rigidbody = GetComponent<Rigidbody>();
        m_targetRotation = transform.rotation;
    }

    // Manager�ȊO��Update���g�������Ȃ����A��]���Ԃ��邽�ߎ���
    private void Update()
    {
        //��������
        var qua = Quaternion.RotateTowards(transform.rotation, m_targetRotation, m_turnSpeed * Time.deltaTime);

        //y���ȊO�𖳎�
        qua.x = 0.0f; qua.z = 0.0f;
        //�����ύX
        transform.rotation = qua;
    }

    /// <summary>
    /// <para>�����̌���</para>
    /// ���ۂ̉�]��Update�Ŏ��s����
    /// </summary>
    /// <param name="_qua">��������</param>
    public void ChangeDirection(Quaternion _qua)
    {
        m_targetRotation = _qua;//�ڕW�̌�����ݒ�
    }

    /// <summary>
    /// <para>�w�肵�����W������</para>
    /// �������������W����p�x���v�Z�����������߂�
    /// </summary>
    /// ///<param name="_target_pos">����������W</param>
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
    /// <para>�O���ɑ���</para>
    /// Rigidbody�̃x�N�g����ݒ肷��
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
    /// <para>�O���ɕ���</para>
    /// Rigidbody�̃x�N�g����ݒ肷��
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
    /// <para>�ړ���~</para>
    /// Rigidbody�̃x�N�g����0�ɂ���
    /// </summary>
    public void StopMove()
    {
        //�ړ��x�N�g����0�ɂ���
        m_rigidbody.velocity = new Vector3(0, m_rigidbody.velocity.y, 0);
    }

    /// <summary>
    /// <para>���[�v</para>
    /// ����̈ʒu�Ƀ��[�v������
    /// �ǂɑj�܂ꂽ�肵�Ĉړ��s�ɂȂ����ꍇ�Ɏg��
    /// </summary>
    /// <param name="_pos">�ړ�����W</param>
    public void Warp(Vector3 _pos)
    {
        StopMove();//�ړ����~�߂�
        transform.position = _pos;
    }
}
