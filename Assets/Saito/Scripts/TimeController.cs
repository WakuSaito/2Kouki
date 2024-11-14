using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//時間経過スクリプト　（昼夜の処理など）
public class TimeController : MonoBehaviour
{
    [SerializeField]//太陽光オブジェクト
    private GameObject directionalLightObj;

    [SerializeField]//昼間の長さ
    private float daylightLengthSec = 120;
    [SerializeField]//夜の長さ
    private float nightLengthSec = 90;

    //1日の合計時間
    private float cicleLengthSec;

    [SerializeField]//開始時の時間（朝から数えた時間）
    private float startTime = 30;
    //経過時間カウント用
    private float timeCount = 0;

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
        cicleLengthSec = daylightLengthSec + nightLengthSec;
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
        if (timeCount >= cicleLengthSec)
        {
            timeCount -= cicleLengthSec;
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

        Debug.Log(timeCount);

        //太陽光の角度変更
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(sunRotate, Vector3.right);
    }

    //時間を日没に変更
    public void ChangeSunset()
    {
        isDaylight = false;
        timeCount = daylightLengthSec;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(180, Vector3.right);
    }
    //時間を日の出に変更
    public void ChangeSunrise()
    {
        isDaylight = true;
        timeCount = 0;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

    }

    /// <summary>
    /// 昼間かどうかを取得する (昼:true, 夜:false)
    /// </summary>
    public bool GetIsDaylight()
    {
        return isDaylight;
    }
}
