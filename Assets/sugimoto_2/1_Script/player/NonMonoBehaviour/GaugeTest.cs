using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaugeTest
{
    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> �Q�[�W�I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_gaugeObj;
    /// <summary> �ő吔�l </summary>
    [SerializeField] float m_maxNum;
    /// <summary> ���炷���� </summary>
    [SerializeField] float m_subWaitTime;

    /*�v���C�x�[�g�@private*/
    /// <summary> ���̐��l </summary>
    float m_nowNum;
    /// <summary> �P������ </summary>
    float m_oneMemory;
    /// <summary> �Q�[�W�T�C�Y </summary>
    RectTransform m_gaugeSize;
    /// <summary> ���Ԍv���p </summary>
    float m_timer;


    //�R���X�g���N�^
    public GaugeTest() { }
    
    /// <summary>
    /// �Q�[�W�̏����ݒ�
    /// </summary>
    public void SetGauge()
    {
        m_nowNum = m_maxNum;
        m_gaugeSize = m_gaugeObj.GetComponent<RectTransform>();
        //�P��������ݒ�
        m_oneMemory = m_gaugeSize.sizeDelta.x / m_maxNum;
        Debug.Log(m_nowNum);
    }

    /// <summary>
    /// ���l��Ԃ�
    /// </summary>
    /// <returns>m_nowNum���̐��l</returns>
    public float RetrunNowNum()
    {
        return m_nowNum;
    }

    /// <summary>
    /// �Q�[�W���c���Ă��Ȃ������ׂ�
    /// </summary>
    /// <returns>�c���Ă��Ȃ�true�A�c���Ă���false</returns>
    public bool NonGauge()
    {
        return m_nowNum == 0;
    }

    /// <summary>
    /// �Q�[�W�𑝂₷
    /// </summary>
    /// <param name="_add_num">���₷���l</param>
    public void AddGauge(float _add_num)
    {
        //�Q�[�W�̑��₷�ʂ�ݒ�
        float add_memory = m_oneMemory * _add_num;

        //���݂̃Q�[�W�̃T�C�Y�f�[�^�擾
        Vector2 now_gauge_size = m_gaugeSize.sizeDelta;

        //���݂̃Q�[�W�Ƀ������𑫂�
        now_gauge_size.x += add_memory;

        //�v�Z�����Q�[�W�̃T�C�Y�ɐݒ�
        m_gaugeSize.sizeDelta = now_gauge_size;

        //gauge���ő�l�𒴂����珉����
        if (m_gaugeSize.sizeDelta.x > (m_maxNum * m_oneMemory))
        {
            now_gauge_size.x = m_maxNum * m_oneMemory;
            m_nowNum = m_maxNum;
            m_gaugeSize.sizeDelta = now_gauge_size;
        }

        //���̐��l��ݒ�
        m_maxNum += _add_num;
    }

    /// <summary>
    /// �Q�[�W�����炷
    /// </summary>
    /// <param name="_sub_num">���炷�l</param>
    public void SubGauge(float _sub_num)
    {
        //�Q�[�W���O���傫����Ύ��s
        if (m_gaugeSize.sizeDelta.x > 0)
        {
            //�Q�[�W�̌��炷�ʂ�ݒ�
            float sub_memory = m_oneMemory * _sub_num;

            //���݂̃Q�[�W�̃T�C�Y�f�[�^�擾
            Vector2 now_gauge_size = m_gaugeSize.sizeDelta;

            //���݂̃Q�[�W�̕��T�C�Y����Q�[�W�̌��炷�ʂ�����
            now_gauge_size.x -= sub_memory;

            //�v�Z�����Q�[�W�̃T�C�Y�ɐݒ�
            m_gaugeSize.sizeDelta = now_gauge_size;

            //���̐��l��ݒ�
            m_nowNum -= _sub_num;
        }

        //�O�𒴂����ꍇ�T�C�Y���O�ɖ߂�
        if (m_gaugeSize.sizeDelta.x <= 0)
        {
            //�����𒲐�
            m_gaugeSize.sizeDelta = new Vector2(0, m_gaugeSize.sizeDelta.y);
            m_nowNum = 0;
        }
    }

    /// <summary>
    /// ����I�ɃQ�[�W�����炷
    /// </summary>
    /// <param name="_sub_num">���炷�l</param>
    /// <param name="_wait_time">���炷�܂ł̑ҋ@����</param>
    public void SubGaugeFixed(float _sub_num)
    {
        m_timer += Time.deltaTime;

        //���Ԃ�������Q�[�W�����炷
        if (m_timer >= m_subWaitTime)
        {
            m_timer = 0.0f;
            SubGauge(_sub_num);
        }
    }
}

