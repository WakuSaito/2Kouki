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
    private GameObject directionalLightObj;

    [SerializeField]//日の出時間
    private int sunriseHour = 6;
    [SerializeField]//日没時間
    private int sunsetHour = 20;

    [SerializeField]//開始時の時間
    private int startTimeHour = 8;

    //昼間の長さ
    private float daylightLengthSec;
    //夜の長さ
    private float nightLengthSec;

    //ゲーム内1日の秒数
    private float cycle1DaySec;
    [SerializeField]//ゲーム内1時間の秒数 ゲーム中に変更するとバグが発生する
    private float cycle1HourSec = 30;
    //ゲーム内1分の秒数
    private float cycle1minuteSec;

    //現在のゲーム内時間
    private HourMinute currentTime;
    //現在のゲーム内日数
    private int currentDayCount = 1;

    //前フレームのゲーム内時間
    private int prevFrameHour;

    //経過時間カウント用
    private float timeCount = 0;


    [SerializeField]//日をカウントするテキスト（d日目）
    private GameObject dayCountTextObj;
    [SerializeField]//時間をカウントするテキスト（hh:mm）
    private GameObject timeCountTextObj;

    [SerializeField]//時間の経過を止める
    private bool isStopPassageTime = false;

    //昼間フラグ
    private bool isDaylight;

    //デバッグ用時間切り替え
    [SerializeField]
    private bool onDebugSunset = false;
    [SerializeField]
    private bool onDebugSunrise = false;

    private void Awake()
    {
        //大きすぎないようにする
        sunriseHour %= HourMinute.MAX_HOUR;
        sunsetHour %= HourMinute.MAX_HOUR;
        startTimeHour %= HourMinute.MAX_HOUR;

        //昼の長さ（秒）計算
        int tmp = sunsetHour - sunriseHour;
        if (tmp < 0)
            tmp += HourMinute.MAX_HOUR;
        daylightLengthSec = tmp * cycle1HourSec;
        //夜の長さ（秒）計算
        nightLengthSec = (HourMinute.MAX_HOUR * cycle1HourSec) - daylightLengthSec;

        //開始時間計算
        int startTmp = startTimeHour - sunriseHour;
        if (startTmp < 0)
            startTmp += HourMinute.MAX_HOUR;
        timeCount = startTmp * cycle1HourSec;

        //ゲーム内1日の秒数計算
        cycle1DaySec = cycle1HourSec * HourMinute.MAX_HOUR;
        //ゲーム内1日の秒数計算
        cycle1minuteSec = cycle1HourSec / HourMinute.MAX_MINUTE;

        //時刻設定
        currentTime = new HourMinute(startTimeHour, 0);
        prevFrameHour = currentTime.GetHour();
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
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

        //時間計測
        timeCount += Time.deltaTime;

        //一定値を超えないように
        if (timeCount >= cycle1DaySec)
        {
            timeCount -= cycle1DaySec;
        }

        //太陽光の角度
        float sunRotate;
        //昼間
        if (timeCount < daylightLengthSec)
        {
            //昼の経過時間の割合
            float daylightTimePer = timeCount / daylightLengthSec;

            sunRotate = 180.0f * daylightTimePer;
            //昼夜状態保存
            isDaylight = true;
        }
        //夜間
        else
        {
            //夜の経過時間の割合
            float nightTimePer = (timeCount - daylightLengthSec) / nightLengthSec;

            sunRotate = 180.0f * (1.0f - nightTimePer) + 360.0f * nightTimePer;
            //昼夜状態保存
            isDaylight = false;
        }

        //太陽光の角度変更
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(sunRotate, Vector3.right);

        //ゲーム内時間更新
        currentTime = new HourMinute(sunriseHour, 0);
        currentTime.AddMinute((int)(timeCount / cycle1minuteSec));
        if(timeCountTextObj != null)
            timeCountTextObj.GetComponent<Text>().text = currentTime.GetTimeString();//テキスト更新

        //0時になったタイミングで動作
        if (prevFrameHour > currentTime.GetHour())
        {
            //日数を増やす
            currentDayCount++;
            if (dayCountTextObj != null)
            {
                //dayCountTextObj.GetComponent<Text>().text = currentDayCount + "日目";//テキスト更新
                dayCountTextObj.GetComponent<DayCountUI>().ChangeDay(currentDayCount);
            }
        }

        //時間保存
        prevFrameHour = currentTime.GetHour();  
    }

    //時間を日没に変更
    public void ChangeSunset()
    {
        isDaylight = false;
        timeCount = daylightLengthSec;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(180, Vector3.right);
        if (timeCountTextObj != null)
            timeCountTextObj.GetComponent<Text>().text = currentTime.GetTimeString();//テキスト更新
    }
    //時間を日の出に変更
    public void ChangeSunrise()
    {
        isDaylight = true;
        timeCount = 0;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        if (timeCountTextObj != null)
            timeCountTextObj.GetComponent<Text>().text = currentTime.GetTimeString();//テキスト更新
    }

    /// <summary>
    /// 昼間かどうかを取得する (昼:true, 夜:false)
    /// </summary>
    public bool GetIsDaylight()
    {
        return isDaylight;
    }

    public int GetCurrentHour()
    {
        return currentTime.GetHour();
    }

    //日数取得
    public int GetDayCount()
    {
        return currentDayCount;
    }

    /// <summary>
    /// 日没から何分経ったか取得する
    /// </summary>
    public int GetMinutesAfterSunset()
    {
        int hour = currentTime.GetHour();
        if (hour < sunsetHour)
            hour += HourMinute.MAX_HOUR;

        return (hour - sunsetHour) * HourMinute.MAX_MINUTE + currentTime.GetMinute();
    }

    /// <summary>
    /// 日の出まで何分あるか取得する
    /// </summary>
    public int GetMinutesBeforeSunrise()
    {
        int hour = currentTime.GetHour();
        if (hour >= sunriseHour)
            hour -= HourMinute.MAX_HOUR;

        return (sunriseHour - hour) * HourMinute.MAX_MINUTE - currentTime.GetMinute();
    }
    //一時停止
    public void Pause()
    {
        isStopPassageTime = true;
    }
    //再開
    public void Resume()
    {
        isStopPassageTime = false;
    }
}
