using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFence : MonoBehaviour
{
    bool m_inAreaFlag = false;

    //�t�F���X�I�u�W�F�N�g
    [SerializeField] Transform m_fenceTrans;
    [SerializeField] Transform m_fenceStopTrans;
    [SerializeField] Transform m_fenceCloseTrans;

    float m_timer = 0.0f;
    //animation���x
    [SerializeField] float m_speed = 3.0f;

    /// <summary>
    /// �v���C���[���t�F���X�̏o������ɓ�������t���OTRUE
    /// </summary>
    /// <param name="other">�R���C�_�[�ɓ������Ă���</param>
    private void OnTriggerEnter(Collider other)
    {
        if (m_inAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            m_timer = 0.0f;
            m_inAreaFlag = true;
        }
    }

    /// <summary>
    /// �v���C���[���t�F���X�̏o���������o����t���OFALSE
    /// </summary>
    /// <param name="other">�R���C�_�[�ɓ������Ă���</param>
    private void OnTriggerExit(Collider other)
    {
        if (!m_inAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            m_timer = 0.0f;
            m_inAreaFlag = false;
        }
    }

    /// <summary>
    /// �t�F���X�̊J����
    /// </summary>
    void Update()
    {
        
        if (m_inAreaFlag)
        {
            //�J����

            m_timer += Time.deltaTime;

            //�ʒu�X�V
            m_fenceTrans.position = Vector3.Lerp(m_fenceTrans.position, m_fenceStopTrans.position, m_timer * m_speed);
            m_fenceTrans.localRotation = Quaternion.Lerp(m_fenceTrans.localRotation, m_fenceStopTrans.localRotation, m_timer * m_speed);
        }
        else
        {
            //�߂�

            m_timer += Time.deltaTime;

            //�ʒu�X�V
            m_fenceTrans.position = Vector3.Lerp(m_fenceTrans.position, m_fenceCloseTrans.position, m_timer * m_speed);
            m_fenceTrans.localRotation = Quaternion.Lerp(m_fenceTrans.localRotation, m_fenceCloseTrans.localRotation, m_timer * m_speed);
        }
    }
}
