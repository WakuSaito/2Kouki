using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerViewpointMove
{
    /// <summary> Y���̐��� </summary>
    const int ROT_Y_MAX = 60;

    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> X�����ɉ�]���������I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_rotXObj;
    /// <summary> Y�����ɉ�]���������I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_rotYObj;
    /// <summary> �J�������x </summary>
    [SerializeField] float m_cameraSensitivity;


    /*�v���C�x�[�g�@private*/
    /// <summary> ��]�ʁFX </summary>
    float m_rotX = 0.0f;
    /// <summary> ��]�ʁFY </summary>
    float m_rotY = 0.0f;


    /// <summary>
    /// ���_�ړ�
    /// </summary>
    public void ViewpointMove()
    {
        //�}�E�X�̓�������ړ��ʁi��]�ʁj���v�Z
        m_rotX += Input.GetAxis("Mouse X") * m_cameraSensitivity * Time.deltaTime;
        m_rotY += Input.GetAxis("Mouse Y") * m_cameraSensitivity * Time.deltaTime;

        //Y���͈ړ�����������
        m_rotY = Mathf.Clamp(m_rotY, -ROT_Y_MAX, ROT_Y_MAX);

        m_rotXObj.transform.localRotation = Quaternion.AngleAxis(m_rotX, Vector3.up);
        m_rotYObj.transform.localRotation = Quaternion.AngleAxis(m_rotY, Vector3.left);//*���O����
    }
}
