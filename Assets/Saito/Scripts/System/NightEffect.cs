using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEffect : MonoBehaviour
{
    [SerializeField]
    TimeController m_timeController;

    [SerializeField]//描画距離の最大（昼間）
    float m_dayCameraFarMax = 25.0f;
    [SerializeField]//描画距離の最小（夜間）
    float m_nightCameraFarMin = 15.0f;

    [SerializeField, Header("日没から何分で描画距離を最小にするか")]
    int m_minOfMinutesAfterSunset = 60;

    [SerializeField, Header("日の出まで何分時点から描画距離を広げていくか")]
    int m_maxOfMinutesBeforeSunrise = 60;

    private void Awake()
    {
        //インスペクターでの指定が無ければ同じオブジェクトから取得
        if (m_timeController == null)
            m_timeController = gameObject.GetComponent<TimeController>();       
    }

    private void Update()
    {
        //昼間
        if(m_timeController.GetIsDaylight())
        {
            Camera.main.farClipPlane = m_dayCameraFarMax;
        }
        //夜間
        else
        {
            //日没から何分か
            int minutes_after_sunset = m_timeController.GetMinutesAfterSunset();
            //日の出まで何分か
            int minutes_before_sunrise = m_timeController.GetMinutesBeforeSunrise();

            //暗さの割合
            float darkness_per;

            //暗くなり始める
            if (minutes_after_sunset < m_minOfMinutesAfterSunset)
            {
                darkness_per = (float)minutes_after_sunset / m_minOfMinutesAfterSunset;
            }
            //明るくなり始める
            else if(minutes_before_sunrise < m_maxOfMinutesBeforeSunrise)
            {
                darkness_per = (float)minutes_before_sunrise / m_minOfMinutesAfterSunset;
            }
            //真夜中
            else
            {
                darkness_per = 1.0f;
            }

            //描画距離を補完し代入
            Camera.main.farClipPlane = m_dayCameraFarMax * (1.0f - darkness_per) + m_nightCameraFarMin * darkness_per;
        }
    }
}
