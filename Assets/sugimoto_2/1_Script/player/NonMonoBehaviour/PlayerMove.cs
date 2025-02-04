using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMove
{
    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> ���W�b�g�{�f�B�[�I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_rbObj;
    /// <summary> �ړ����x:���� </summary>
    [SerializeField] float m_walkSpeed;
    /// <summary> �ړ����x:���� </summary>
    [SerializeField] float m_runSpeed;


    /*�v���C�x�[�g�@private*/
    /// <summary> ���W�b�g�{�f�B�[ </summary>
    Rigidbody m_rb;
    /// <summary> �L�[���͉� </summary>
    int m_keyPushCnt = 0;
    /// <summary> �A�����͐������� </summary>
    float m_pushTimer = 0;
    /// <summary> �O�Ɉړ����Ă��邩 </summary>
    bool m_forward_Flag = false;
    /// <summary> �����Ă��邩 </summary>
    bool m_runFlag = false;
    /// <summary> �x�N�g�� </summary>
    Vector3 vec;

    public void SetMove()
    {
        m_rb = m_rbObj.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �O�i�x�N�g���ݒ�
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void MoveForwardVec(bool _phsh)
    {
        if (!_phsh) return;

        vec += m_rbObj.transform.forward;
        m_forward_Flag = true;
    }
    /// <summary>
    /// ��ރx�N�g���ݒ�
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void MoveBackVec(bool _phsh)
    {
        if (!_phsh) return;
        vec -= m_rbObj.transform.forward;
    }
    /// <summary>
    /// �E�x�N�g���ݒ�
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void MoveRightVec(bool _phsh)
    {
        if (!_phsh) return;
        vec += m_rbObj.transform.right;
    }
    /// <summary>
    /// ���x�N�g���ݒ�
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void MoveLeftVec(bool _phsh)
    {
        if (!_phsh) return;
        vec -= m_rbObj.transform.right;
    }

    /// <summary>
    /// �x�N�g����Velocity�ɑ������킹��
    /// </summary>
    public void AddVelocityVec()
    {
        //�΂߈ړ��̑��x�����ɂ��邽�ߐ��K��
        vec.Normalize();

        //�ړ�
        if (m_runFlag&& m_forward_Flag)
        {
            m_rb.velocity = new Vector3(vec.x * m_runSpeed, m_rb.velocity.y, vec.z * m_runSpeed);
        }
        else
        {
            m_rb.velocity = new Vector3(vec.x * m_walkSpeed, m_rb.velocity.y, vec.z * m_walkSpeed);
        }

        //�ړ���ԏ�����
        m_forward_Flag = false;
        vec = Vector3.zero;
    }

    /// <summary>
    /// ���鏀���F�_�u������
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    /// <param name="_kye_code">���͂���L�[</param>
    public void SetUpRun(bool _phsh)
    {
        //�����Ă��Ȃ���Ώ�����
        if (vec == Vector3.zero)
        {
            m_runFlag = false;
            m_keyPushCnt = 0;
        }


        //���łɑ����Ă���ΏI��
        if (m_runFlag) return;

        //�L�[���͂��ꂽ�Ƃ�
        if (_phsh)
        {
            m_keyPushCnt++;

            //�����Ă��玞�Ԃ��v��
            float check_time = Time.time - m_pushTimer;

            //�O�ɓ��͂���Ă���P�b�o�܂łɎ��̓��͂��Ȃ���΃L�����Z��
            if (check_time <= 1)
            {
                m_runFlag = true;
            }
            else
            {
                Debug.Log("�L�����Z��");
                m_runFlag = false;
                m_keyPushCnt = 0;
                m_pushTimer = 0.0f;
            }

            m_pushTimer = Time.time;
        }
    }

    /// <summary>
    /// ���鏀���F�Q�̃L�[����
    /// </summary>
    /// <param name="_phsh1">�P�ڂ̃L�[�F���͂���Ă��邩</param>
    /// <param name="_phsh2">�Q�ڂ̃L�[�F���͂���Ă��邩</param>
    /// <param name="_kye_code1">�P�ڂ̓��͂���L�[</param>
    /// <param name="_kye_code2">�Q�ڂ̓��͂���L�[</param>
    public void SetUpRun(bool _phsh1,bool _phsh2)
    {
        //���łɑ����Ă���ΏI��
        if (m_runFlag) return;

        //������
        m_runFlag = false;
        //���͂���Ă��Ȃ���ΏI��
        if (!_phsh1 || !_phsh2) return;

        m_runFlag = true;
    }

    /// <summary>
    /// �����Ă��邩
    /// �����Ă��Ȃ����ړ����Ă���
    /// </summary>
    /// <returns>�����Ă����true</returns>
    public bool WalkFlag()
    {
        return !RunFlag() && vec != Vector3.zero;
    }

    /// <summary>
    /// �����Ă��邩
    /// </summary>
    /// <returns>�����Ă����true</returns>
    public bool RunFlag()
    {
        return m_runFlag && m_forward_Flag;
    }
}
