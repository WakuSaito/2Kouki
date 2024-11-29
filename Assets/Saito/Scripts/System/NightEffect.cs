using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEffect : MonoBehaviour
{
    [SerializeField]
    TimeController timeController;

    [SerializeField]//描画距離の最大（昼間）
    float dayCameraFarMax = 25.0f;
    [SerializeField]//描画距離の最小（夜間）
    float nightCameraFarMin = 15.0f;

    [SerializeField, Header("日没から何分で描画距離を最小にするか")]
    int minOfMinutesAfterSunset = 60;

    [SerializeField, Header("日の出まで何分時点から描画距離を広げていくか")]
    int maxOfMinutesBeforeSunrise = 60;

    private void Awake()
    {
        //インスペクターでの指定が無ければ同じオブジェクトから取得
        if (timeController == null)
            timeController = gameObject.GetComponent<TimeController>();       
    }

    private void Update()
    {
        //昼間
        if(timeController.GetIsDaylight())
        {
            Camera.main.farClipPlane = dayCameraFarMax;
        }
        //夜間
        else
        {
            //日没から何分か
            int minutesAfterSunset = timeController.GetMinutesAfterSunset();
            //日の出まで何分か
            int minutesBeforeSunrise = timeController.GetMinutesBeforeSunrise();

            //暗さの割合
            float darknessPer;

            //暗くなり始める
            if (minutesAfterSunset < minOfMinutesAfterSunset)
            {
                darknessPer = (float)minutesAfterSunset / minOfMinutesAfterSunset;
            }
            //明るくなり始める
            else if(minutesBeforeSunrise < maxOfMinutesBeforeSunrise)
            {
                darknessPer = (float)minutesBeforeSunrise / minOfMinutesAfterSunset;
            }
            //真夜中
            else
            {
                darknessPer = 1.0f;
            }

            //描画距離を補完し代入
            Camera.main.farClipPlane = dayCameraFarMax * (1.0f - darknessPer) + nightCameraFarMin * darknessPer;
        }
    }
}
