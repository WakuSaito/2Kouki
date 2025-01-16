using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����:���f�[�^�N���X
/// ����:���P�ʂ̐��l�ƌv�Z���������N���X�i�\���̂̂悤�ȗp�r�j
/// </summary>
public class HourMinute
{
    //�萔
    public const int MAX_HOUR = 24;
    public const int MAX_MINUTE = 60;

    //���l
    private int hour = 0;  //����
    private int minute = 0;//��

    /// <summary>
    /// �R���X�g���N�^
    /// ���ԁA�����w�肵�A�쐬
    /// </summary>
    /// <param name="_hour">����</param>
    /// <param name="_minute">��</param>
    public HourMinute(int _hour, int _minute)
    {
        AddHour(_hour);
        AddMinute(_minute);
    }
    /// <summary>
    /// ���Ԃ̎擾
    /// </summary>
    public int GetHour() { return hour; }
    /// <summary>
    /// ���̎擾
    /// </summary>
    public int GetMinute() { return minute; }

    /// <summary>
    /// ���̉��Z
    /// </summary>
    /// <param name="_minute">���Z���镪</param>
    /// <returns>���Z��̕�</returns>
    public int AddMinute(int _minute)
    {
        int sumMinute = minute + _minute;

        if (sumMinute >= MAX_MINUTE)
        {
            minute = sumMinute % MAX_MINUTE;
            AddHour(sumMinute / MAX_MINUTE);
        }
        else if (sumMinute < 0)
        {
            minute = sumMinute + MAX_MINUTE * ((sumMinute * -1) / MAX_MINUTE + 1);
            AddHour(sumMinute / MAX_MINUTE -1);
        }
        else
            minute = sumMinute;

        return minute;
    }
    /// <summary>
    /// ���Ԃ̉��Z
    /// </summary>
    /// <param name="_hour">���Z���鎞��</param>
    /// <returns>���Z��̎���</returns>
    public int AddHour(int _hour)
    {
        int sumHour = hour + _hour;

        if (sumHour >= MAX_HOUR)
        {
            hour = sumHour % MAX_HOUR;
        }
        else if (sumHour < 0)
        {
            hour = sumHour + MAX_HOUR * ((sumHour * -1) / MAX_HOUR + 1);
        }
        else
            hour = sumHour;

        return hour;
    }
    /// <summary>
    /// ����:���̕����^�擾
    /// ���l��string�^�ɕϊ����擾
    /// </summary>
    /// <returns>"hh:mm"</returns>
    public string GetTimeString()
    {
        string timeString = "";

        if (hour < 10)
            timeString += "0";
        timeString += hour.ToString() + ":";

        if (minute < 10)
            timeString += "0";
        timeString += minute.ToString();

        return timeString;
    }
}

//���Ԍo�߃X�N���v�g�@�i����̏����Ȃǁj
/// <summary>
/// ���ԊǗ��N���X
/// ���Ԍo�߁A�����̌����A�e�L�X�g������
/// �|�[�Y���Ɏ��Ԍo�߂��~����������IStopObject�p��
/// </summary>
public class TimeController : MonoBehaviour, IStopObject
{
    [SerializeField]//���z���I�u�W�F�N�g
    private GameObject m_directionalLightObj;

    [SerializeField]//���̏o����
    private int m_sunriseHour = 6;
    [SerializeField]//���v����
    private int m_sunsetHour = 20;

    [SerializeField]//�J�n���̎���
    private int m_startTimeHour = 8;

    //���Ԃ̒���
    private float m_daylightLengthSec;
    //��̒���
    private float m_nightLengthSec;

    //�Q�[����1���̕b��
    private float m_cycle1DaySec;
    [SerializeField]//�Q�[����1���Ԃ̕b�� �Q�[�����ɕύX����ƃo�O����������
    private float m_cycle1HourSec = 30;
    //�Q�[����1���̕b��
    private float m_cycle1minuteSec;

    //���݂̃Q�[��������
    private HourMinute m_currentTime;
    //���݂̃Q�[��������
    private int m_currentDayCount = 1;

    //�O�t���[���̃Q�[��������
    private int m_prevFrameHour;

    //�o�ߎ��ԃJ�E���g�p
    private float m_timeCount = 0;

    [SerializeField]//�����J�E���g����e�L�X�g�id���ځj
    private GameObject m_dayCountTextObj;
    [SerializeField]//���Ԃ��J�E���g����e�L�X�g�ihh:mm�j
    private GameObject m_timeCountTextObj;

    [SerializeField]//���Ԃ̌o�߂��~�߂�
    private bool m_isStopPassageTime = false;

    //���ԃt���O
    private bool m_isDaylight;

    //�f�o�b�O�p���Ԑ؂�ւ�
    [SerializeField]
    private bool m_onDebugSunset = false;
    [SerializeField]
    private bool m_onDebugSunrise = false;

    //���l�̌v�Z
    private void Awake()
    {
        //�傫�����Ȃ��悤�ɂ���
        m_sunriseHour %= HourMinute.MAX_HOUR;
        m_sunsetHour %= HourMinute.MAX_HOUR;
        m_startTimeHour %= HourMinute.MAX_HOUR;

        //���̒����i�b�j�v�Z
        int tmp = m_sunsetHour - m_sunriseHour;
        if (tmp < 0)
            tmp += HourMinute.MAX_HOUR;
        m_daylightLengthSec = tmp * m_cycle1HourSec;
        //��̒����i�b�j�v�Z
        m_nightLengthSec = (HourMinute.MAX_HOUR * m_cycle1HourSec) - m_daylightLengthSec;

        //�J�n���Ԍv�Z
        int startTmp = m_startTimeHour - m_sunriseHour;
        if (startTmp < 0)
            startTmp += HourMinute.MAX_HOUR;
        m_timeCount = startTmp * m_cycle1HourSec;

        //�Q�[����1���̕b���v�Z
        m_cycle1DaySec = m_cycle1HourSec * HourMinute.MAX_HOUR;
        //�Q�[����1���̕b���v�Z
        m_cycle1minuteSec = m_cycle1HourSec / HourMinute.MAX_MINUTE;

        //�����ݒ�
        m_currentTime = new HourMinute(m_startTimeHour, 0);
        m_prevFrameHour = m_currentTime.GetHour();
    }

    //���Ԍo�߂̏����A�����̌����v�Z�A�e�L�X�g�̍X�V
    void Update()
    {
        //�f�o�b�O�p
        {
            if (m_onDebugSunrise)
            {
                ChangeSunrise();
                m_onDebugSunrise = false;
            }
            if (m_onDebugSunset)
            {
                ChangeSunset();
                m_onDebugSunset = false;
            }
        }

        if (m_isStopPassageTime) return;

        //���Ԍv��
        m_timeCount += Time.deltaTime;

        //���l�𒴂��Ȃ��悤��
        if (m_timeCount >= m_cycle1DaySec)
        {
            m_timeCount -= m_cycle1DaySec;
        }

        //���z���̊p�x
        float sunRotate;
        //����
        if (m_timeCount < m_daylightLengthSec)
        {
            //���̌o�ߎ��Ԃ̊���
            float daylightTimePer = m_timeCount / m_daylightLengthSec;

            sunRotate = 180.0f * daylightTimePer;
            //�����ԕۑ�
            m_isDaylight = true;
        }
        //���
        else
        {
            //��̌o�ߎ��Ԃ̊���
            float nightTimePer = (m_timeCount - m_daylightLengthSec) / m_nightLengthSec;

            sunRotate = 180.0f * (1.0f - nightTimePer) + 360.0f * nightTimePer;
            //�����ԕۑ�
            m_isDaylight = false;
        }

        //���z���̊p�x�ύX
        m_directionalLightObj.transform.localRotation = Quaternion.AngleAxis(sunRotate, Vector3.right);

        //�Q�[�������ԍX�V
        m_currentTime = new HourMinute(m_sunriseHour, 0);
        m_currentTime.AddMinute((int)(m_timeCount / m_cycle1minuteSec));
        if(m_timeCountTextObj != null)
            m_timeCountTextObj.GetComponent<Text>().text = m_currentTime.GetTimeString();//�e�L�X�g�X�V

        //0���ɂȂ����^�C�~���O�œ���
        if (m_prevFrameHour > m_currentTime.GetHour())
        {
            //�����𑝂₷
            m_currentDayCount++;
            if (m_dayCountTextObj != null)
            {
                //dayCountTextObj.GetComponent<Text>().text = currentDayCount + "����";//�e�L�X�g�X�V
                m_dayCountTextObj.GetComponent<DayCountUI>().ChangeDay(m_currentDayCount);
            }
        }

        //���ԕۑ�
        m_prevFrameHour = m_currentTime.GetHour();  
    }

    /// <summary>
    /// ���Ԃ���v�ɕύX
    /// �f�o�b�O�p�@���ԋ����ύX
    /// </summary>
    public void ChangeSunset()
    {
        m_isDaylight = false;
        m_timeCount = m_daylightLengthSec;
        m_directionalLightObj.transform.localRotation = Quaternion.AngleAxis(180, Vector3.right);
        if (m_timeCountTextObj != null)
            m_timeCountTextObj.GetComponent<Text>().text = m_currentTime.GetTimeString();//�e�L�X�g�X�V
    }
    /// <summary>
    /// ���Ԃ���̏o�ɕύX
    /// �f�o�b�O�p�@���ԋ����ύX
    /// </summary>
    public void ChangeSunrise()
    {
        m_isDaylight = true;
        m_timeCount = 0;
        m_directionalLightObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        if (m_timeCountTextObj != null)
            m_timeCountTextObj.GetComponent<Text>().text = m_currentTime.GetTimeString();//�e�L�X�g�X�V
    }

    /// <summary>
    /// ���Ԃ��ǂ������擾����
    /// </summary>
    /// <returns>�� : true</returns>
    public bool GetIsDaylight()
    {
        return m_isDaylight;
    }

    /// <summary>
    /// ���݂̎��Ԏ擾
    /// </summary>
    public int GetCurrentHour()
    {
        return m_currentTime.GetHour();
    }

    /// <summary>
    /// ���݂̓����擾
    /// </summary>
    public int GetDayCount()
    {
        return m_currentDayCount;
    }

    /// <summary>
    /// ���v���牽���o�������擾����
    /// ���Ԃɂ���ĕ�Ԃ������̂Ŏ���
    /// </summary>
    public int GetMinutesAfterSunset()
    {
        int hour = m_currentTime.GetHour();
        if (hour < m_sunsetHour)
            hour += HourMinute.MAX_HOUR;

        return (hour - m_sunsetHour) * HourMinute.MAX_MINUTE + m_currentTime.GetMinute();
    }

    /// <summary>
    /// ���̏o�܂ŉ������邩�擾����
    /// ���Ԃɂ���ĕ�Ԃ������̂Ŏ���
    /// </summary>
    public int GetMinutesBeforeSunrise()
    {
        int hour = m_currentTime.GetHour();
        if (hour >= m_sunriseHour)
            hour -= HourMinute.MAX_HOUR;

        return (m_sunriseHour - hour) * HourMinute.MAX_MINUTE - m_currentTime.GetMinute();
    }
    /// <summary>
    /// �ꎞ��~
    /// �|�[�Y���ɒ�~����
    /// </summary>
    public void Pause()
    {
        m_isStopPassageTime = true;
    }
    /// <summary>
    /// �ĊJ
    /// �|�[�Y�������ɒ�~��������
    /// </summary>
    public void Resume()
    {
        m_isStopPassageTime = false;
    }
}
