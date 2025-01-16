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

//時間経過スクリプト　（昼夜の処理など）
public class TimeController : MonoBehaviour, IStopObject
{
    [SerializeField]//太陽光オブジェクト
    private GameObject m_directionalLightObj;

    [SerializeField]//日の出時間
    private int m_sunriseHour = 6;
    [SerializeField]//日没時間
    private int m_sunsetHour = 20;

    [SerializeField]//開始時の時間
    private int m_startTimeHour = 8;

    //昼間の長さ
    private float m_daylightLengthSec;
    //夜の長さ
    private float m_nightLengthSec;

    //ゲーム内1日の秒数
    private float m_cycle1DaySec;
    [SerializeField]//ゲーム内1時間の秒数 ゲーム中に変更するとバグが発生する
    private float m_cycle1HourSec = 30;
    //ゲーム内1分の秒数
    private float m_cycle1minuteSec;

    //現在のゲーム内時間
    private HourMinute m_currentTime;
    //現在のゲーム内日数
    private int m_currentDayCount = 1;

    //前フレームのゲーム内時間
    private int m_prevFrameHour;

    //経過時間カウント用
    private float m_timeCount = 0;


    [SerializeField]//日をカウントするテキスト（d日目）
    private GameObject m_dayCountTextObj;
    [SerializeField]//時間をカウントするテキスト（hh:mm）
    private GameObject m_timeCountTextObj;

    [SerializeField]//時間の経過を止める
    private bool m_isStopPassageTime = false;

    //昼間フラグ
    private bool m_isDaylight;

    //デバッグ用時間切り替え
    [SerializeField]
    private bool m_onDebugSunset = false;
    [SerializeField]
    private bool m_onDebugSunrise = false;

    private void Awake()
    {
        //大きすぎないようにする
        m_sunriseHour %= HourMinute.MAX_HOUR;
        m_sunsetHour %= HourMinute.MAX_HOUR;
        m_startTimeHour %= HourMinute.MAX_HOUR;

        //昼の長さ（秒）計算
        int tmp = m_sunsetHour - m_sunriseHour;
        if (tmp < 0)
            tmp += HourMinute.MAX_HOUR;
        m_daylightLengthSec = tmp * m_cycle1HourSec;
        //夜の長さ（秒）計算
        m_nightLengthSec = (HourMinute.MAX_HOUR * m_cycle1HourSec) - m_daylightLengthSec;

        //開始時間計算
        int startTmp = m_startTimeHour - m_sunriseHour;
        if (startTmp < 0)
            startTmp += HourMinute.MAX_HOUR;
        m_timeCount = startTmp * m_cycle1HourSec;

        //ゲーム内1日の秒数計算
        m_cycle1DaySec = m_cycle1HourSec * HourMinute.MAX_HOUR;
        //ゲーム内1日の秒数計算
        m_cycle1minuteSec = m_cycle1HourSec / HourMinute.MAX_MINUTE;

        //時刻設定
        m_currentTime = new HourMinute(m_startTimeHour, 0);
        m_prevFrameHour = m_currentTime.GetHour();
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
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

        //時間計測
        m_timeCount += Time.deltaTime;

        //一定値を超えないように
        if (m_timeCount >= m_cycle1DaySec)
        {
            m_timeCount -= m_cycle1DaySec;
        }

        //太陽光の角度
        float sunRotate;
        //昼間
        if (m_timeCount < m_daylightLengthSec)
        {
            //昼の経過時間の割合
            float daylightTimePer = m_timeCount / m_daylightLengthSec;

            sunRotate = 180.0f * daylightTimePer;
            //昼夜状態保存
            m_isDaylight = true;
        }
        //夜間
        else
        {
            //夜の経過時間の割合
            float nightTimePer = (m_timeCount - m_daylightLengthSec) / m_nightLengthSec;

            sunRotate = 180.0f * (1.0f - nightTimePer) + 360.0f * nightTimePer;
            //昼夜状態保存
            m_isDaylight = false;
        }

        //太陽光の角度変更
        m_directionalLightObj.transform.localRotation = Quaternion.AngleAxis(sunRotate, Vector3.right);

        //ゲーム内時間更新
        m_currentTime = new HourMinute(m_sunriseHour, 0);
        m_currentTime.AddMinute((int)(m_timeCount / m_cycle1minuteSec));
        if(m_timeCountTextObj != null)
            m_timeCountTextObj.GetComponent<Text>().text = m_currentTime.GetTimeString();//テキスト更新

        //0時になったタイミングで動作
        if (m_prevFrameHour > m_currentTime.GetHour())
        {
            //日数を増やす
            m_currentDayCount++;
            if (m_dayCountTextObj != null)
            {
                //dayCountTextObj.GetComponent<Text>().text = currentDayCount + "日目";//テキスト更新
                m_dayCountTextObj.GetComponent<DayCountUI>().ChangeDay(m_currentDayCount);
            }
        }

        //時間保存
        m_prevFrameHour = m_currentTime.GetHour();  
    }

    //時間を日没に変更
    public void ChangeSunset()
    {
        m_isDaylight = false;
        m_timeCount = m_daylightLengthSec;
        m_directionalLightObj.transform.localRotation = Quaternion.AngleAxis(180, Vector3.right);
        if (m_timeCountTextObj != null)
            m_timeCountTextObj.GetComponent<Text>().text = m_currentTime.GetTimeString();//テキスト更新
    }
    //時間を日の出に変更
    public void ChangeSunrise()
    {
        m_isDaylight = true;
        m_timeCount = 0;
        m_directionalLightObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        if (m_timeCountTextObj != null)
            m_timeCountTextObj.GetComponent<Text>().text = m_currentTime.GetTimeString();//テキスト更新
    }

    /// <summary>
    /// 昼間かどうかを取得する (昼:true, 夜:false)
    /// </summary>
    public bool GetIsDaylight()
    {
        return m_isDaylight;
    }

    public int GetCurrentHour()
    {
        return m_currentTime.GetHour();
    }

    //日数取得
    public int GetDayCount()
    {
        return m_currentDayCount;
    }

    /// <summary>
    /// 日没から何分経ったか取得する
    /// </summary>
    public int GetMinutesAfterSunset()
    {
        int hour = m_currentTime.GetHour();
        if (hour < m_sunsetHour)
            hour += HourMinute.MAX_HOUR;

        return (hour - m_sunsetHour) * HourMinute.MAX_MINUTE + m_currentTime.GetMinute();
    }

    /// <summary>
    /// 日の出まで何分あるか取得する
    /// </summary>
    public int GetMinutesBeforeSunrise()
    {
        int hour = m_currentTime.GetHour();
        if (hour >= m_sunriseHour)
            hour -= HourMinute.MAX_HOUR;

        return (m_sunriseHour - hour) * HourMinute.MAX_MINUTE - m_currentTime.GetMinute();
    }
    //一時停止
    public void Pause()
    {
        m_isStopPassageTime = true;
    }
    //再開
    public void Resume()
    {
        m_isStopPassageTime = false;
    }
}
