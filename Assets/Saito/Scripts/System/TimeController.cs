using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourMinute
{
    public const int MAX_HOUR = 24;
    public const int MAX_MINUTE = 60;

    private int hour = 0;
    private int minute = 0;

    public HourMinute(int _hour, int _minute)
    {
        AddHour(_hour);
        AddMinute(_minute);
    }

    public int GetHour() { return hour; }
    public int GetMinute() { return minute; }

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
public class TimeController : MonoBehaviour
{
    [SerializeField]//���z���I�u�W�F�N�g
    private GameObject directionalLightObj;

    [SerializeField]//���̏o����
    private int sunriseHour = 6;
    [SerializeField]//���v����
    private int sunsetHour = 20;

    [SerializeField]//�J�n���̎���
    private int startTimeHour = 8;

    //���Ԃ̒���
    private float daylightLengthSec;
    //��̒���
    private float nightLengthSec;

    //�Q�[����1���̕b��
    private float cycle1DaySec;
    [SerializeField]//�Q�[����1���Ԃ̕b�� �Q�[�����ɕύX����ƃo�O����������
    private float cycle1HourSec = 30;
    //�Q�[����1���̕b��
    private float cycle1minuteSec;

    //���݂̃Q�[��������
    private HourMinute currentTime;
    //���݂̃Q�[��������
    private int currentDayCount = 1;

    //�O�t���[���̃Q�[��������
    private int prevFrameHour;

    //�o�ߎ��ԃJ�E���g�p
    private float timeCount = 0;


    [SerializeField]//�����J�E���g����e�L�X�g�id���ځj
    private GameObject dayCountTextObj;
    [SerializeField]//���Ԃ��J�E���g����e�L�X�g�ihh:mm�j
    private GameObject timeCountTextObj;

    [SerializeField]//���Ԃ̌o�߂��~�߂�
    private bool isStopPassageTime = false;

    //���ԃt���O
    private bool isDaylight;

    //�f�o�b�O�p���Ԑ؂�ւ�
    [SerializeField]
    private bool onDebugSunset = false;
    [SerializeField]
    private bool onDebugSunrise = false;

    private void Awake()
    {
        //�傫�����Ȃ��悤�ɂ���
        sunriseHour %= HourMinute.MAX_HOUR;
        sunsetHour %= HourMinute.MAX_HOUR;
        startTimeHour %= HourMinute.MAX_HOUR;

        //���̒����i�b�j�v�Z
        int tmp = sunsetHour - sunriseHour;
        if (tmp < 0)
            tmp += HourMinute.MAX_HOUR;
        daylightLengthSec = tmp * cycle1HourSec;
        //��̒����i�b�j�v�Z
        nightLengthSec = (HourMinute.MAX_HOUR * cycle1HourSec) - daylightLengthSec;

        //�J�n���Ԍv�Z
        int startTmp = startTimeHour - sunriseHour;
        if (startTmp < 0)
            startTmp += HourMinute.MAX_HOUR;
        timeCount = startTmp * cycle1HourSec;

        //�Q�[����1���̕b���v�Z
        cycle1DaySec = cycle1HourSec * HourMinute.MAX_HOUR;
        //�Q�[����1���̕b���v�Z
        cycle1minuteSec = cycle1HourSec / HourMinute.MAX_MINUTE;

        //�����ݒ�
        currentTime = new HourMinute(startTimeHour, 0);
        prevFrameHour = currentTime.GetHour();
    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�O�p
        {
            if (onDebugSunrise)
            {
                ChangeSunrise();
                onDebugSunrise = false;
            }
            if (onDebugSunset)
            {
                ChangeSunset();
                onDebugSunset = false;
            }
        }

        if (isStopPassageTime) return;      

        //���Ԍv��
        timeCount += Time.deltaTime;

        //���l�𒴂��Ȃ��悤��
        if (timeCount >= cycle1DaySec)
        {
            timeCount -= cycle1DaySec;
        }

        //���z���̊p�x
        float sunRotate;
        //����
        if (timeCount < daylightLengthSec)
        {
            //���̌o�ߎ��Ԃ̊���
            float daylightTimePer = timeCount / daylightLengthSec;

            sunRotate = 180.0f * daylightTimePer;
            //�����ԕۑ�
            isDaylight = true;
        }
        //���
        else
        {
            //��̌o�ߎ��Ԃ̊���
            float nightTimePer = (timeCount - daylightLengthSec) / nightLengthSec;

            sunRotate = 180.0f * (1.0f - nightTimePer) + 360.0f * nightTimePer;
            //�����ԕۑ�
            isDaylight = false;
        }

        //���z���̊p�x�ύX
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(sunRotate, Vector3.right);

        //�Q�[�������ԍX�V
        currentTime = new HourMinute(sunriseHour, 0);
        currentTime.AddMinute((int)(timeCount / cycle1minuteSec));
        if(timeCountTextObj != null)
            timeCountTextObj.GetComponent<Text>().text = currentTime.GetTimeString();//�e�L�X�g�X�V

        //0���ɂȂ����^�C�~���O�œ���
        if (prevFrameHour > currentTime.GetHour())
        {
            //�����𑝂₷
            currentDayCount++;
            if (dayCountTextObj != null)
                dayCountTextObj.GetComponent<Text>().text = currentDayCount + "����";//�e�L�X�g�X�V
        }

        //���ԕۑ�
        prevFrameHour = currentTime.GetHour();  
    }

    //���Ԃ���v�ɕύX
    public void ChangeSunset()
    {
        isDaylight = false;
        timeCount = daylightLengthSec;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(180, Vector3.right);
        if (timeCountTextObj != null)
            timeCountTextObj.GetComponent<Text>().text = currentTime.GetTimeString();//�e�L�X�g�X�V
    }
    //���Ԃ���̏o�ɕύX
    public void ChangeSunrise()
    {
        isDaylight = true;
        timeCount = 0;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        if (timeCountTextObj != null)
            timeCountTextObj.GetComponent<Text>().text = currentTime.GetTimeString();//�e�L�X�g�X�V
    }

    /// <summary>
    /// ���Ԃ��ǂ������擾���� (��:true, ��:false)
    /// </summary>
    public bool GetIsDaylight()
    {
        return isDaylight;
    }

    public int GetCurrentHour()
    {
        return currentTime.GetHour();
    }

    //�����擾
    public int GetDayCount()
    {
        return currentDayCount;
    }

    /// <summary>
    /// ���v���牽���o�������擾����
    /// </summary>
    public int GetMinutesAfterSunset()
    {
        int hour = currentTime.GetHour();
        if (hour < sunsetHour)
            hour += HourMinute.MAX_HOUR;

        return (hour - sunsetHour) * HourMinute.MAX_MINUTE + currentTime.GetMinute();
    }

    /// <summary>
    /// ���̏o�܂ŉ������邩�擾����
    /// </summary>
    public int GetMinutesBeforeSunrise()
    {
        int hour = currentTime.GetHour();
        if (hour >= sunriseHour)
            hour -= HourMinute.MAX_HOUR;

        return (sunriseHour - hour) * HourMinute.MAX_MINUTE - currentTime.GetMinute();
    }
}
