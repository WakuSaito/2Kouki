using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��̃G�t�F�N�g�N���X
/// ����ɂ���ĕ`�拗����ύX
/// </summary>
public class NightEffect : MonoBehaviour
{
    [SerializeField]//���ԊǗ��N���X
    TimeController m_timeController;

    [SerializeField]//�`�拗���̍ő�i���ԁj
    float m_dayCameraFarMax = 25.0f;
    [SerializeField]//�`�拗���̍ŏ��i��ԁj
    float m_nightCameraFarMin = 15.0f;

    [SerializeField, Header("���v���牽���ŕ`�拗�����ŏ��ɂ��邩")]
    int m_minOfMinutesAfterSunset = 60;

    [SerializeField, Header("���̏o�܂ŉ������_����`�拗�����L���Ă�����")]
    int m_maxOfMinutesBeforeSunrise = 60;

    //�R���|�[�l���g�擾
    private void Awake()
    {
        //�C���X�y�N�^�[�ł̎w�肪������Γ����I�u�W�F�N�g����擾
        if (m_timeController == null)
            m_timeController = gameObject.GetComponent<TimeController>();       
    }

    //���ԑтɂ���ăJ�����̕`�拗����ύX����
    private void Update()
    {
        //����
        if(m_timeController.GetIsDaylight())
        {
            Camera.main.farClipPlane = m_dayCameraFarMax;
        }
        //���
        else
        {
            //���v���牽����
            int minutes_after_sunset = m_timeController.GetMinutesAfterSunset();
            //���̏o�܂ŉ�����
            int minutes_before_sunrise = m_timeController.GetMinutesBeforeSunrise();

            //�Â��̊���
            float darkness_per;

            //�Â��Ȃ�n�߂�
            if (minutes_after_sunset < m_minOfMinutesAfterSunset)
            {
                darkness_per = (float)minutes_after_sunset / m_minOfMinutesAfterSunset;
            }
            //���邭�Ȃ�n�߂�
            else if(minutes_before_sunrise < m_maxOfMinutesBeforeSunrise)
            {
                darkness_per = (float)minutes_before_sunrise / m_minOfMinutesAfterSunset;
            }
            //�^�钆
            else
            {
                darkness_per = 1.0f;
            }

            //�`�拗����⊮�����
            Camera.main.farClipPlane = m_dayCameraFarMax * (1.0f - darkness_per) + m_nightCameraFarMin * darkness_per;
        }
    }
}
