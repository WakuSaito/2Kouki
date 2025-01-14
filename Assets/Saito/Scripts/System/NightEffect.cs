using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEffect : MonoBehaviour
{
    [SerializeField]
    TimeController mTimeController;

    [SerializeField]//描画距離の最大（昼間）
    float mDayCameraFarMax = 25.0f;
    [SerializeField]//描画距離の最小（夜間）
    float mNightCameraFarMin = 15.0f;

    [SerializeField, Header("日没から何分で描画距離を最小にするか")]
    int mMinOfMinutesAfterSunset = 60;

    [SerializeField, Header("日の出まで何分時点から描画距離を広げていくか")]
    int mMaxOfMinutesBeforeSunrise = 60;

    private void Awake()
    {
        //インスペクターでの指定が無ければ同じオブジェクトから取得
        if (mTimeController == null)
            mTimeController = gameObject.GetComponent<TimeController>();       
    }

    private void Update()
    {
        //昼間
        if(mTimeController.GetIsDaylight())
        {
            Camera.main.farClipPlane = mDayCameraFarMax;
        }
        //夜間
        else
        {
            //日没から何分か
            int minutes_after_sunset = mTimeController.GetMinutesAfterSunset();
            //日の出まで何分か
            int minutes_before_sunrise = mTimeController.GetMinutesBeforeSunrise();

            //暗さの割合
            float darkness_per;

            //暗くなり始める
            if (minutes_after_sunset < mMinOfMinutesAfterSunset)
            {
                darkness_per = (float)minutes_after_sunset / mMinOfMinutesAfterSunset;
            }
            //明るくなり始める
            else if(minutes_before_sunrise < mMaxOfMinutesBeforeSunrise)
            {
                darkness_per = (float)minutes_before_sunrise / mMinOfMinutesAfterSunset;
            }
            //真夜中
            else
            {
                darkness_per = 1.0f;
            }

            //描画距離を補完し代入
            Camera.main.farClipPlane = mDayCameraFarMax * (1.0f - darkness_per) + mNightCameraFarMin * darkness_per;
        }
    }
}
