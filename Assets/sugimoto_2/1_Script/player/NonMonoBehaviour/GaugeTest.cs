using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaugeTest
{
    /*[SerializeField] インスペクターから設定*/
    /// <summary> ゲージオブジェクト </summary>
    [SerializeField] GameObject m_gaugeObj;
    /// <summary> 最大数値 </summary>
    [SerializeField] float m_maxNum;
    /// <summary> 減らす時間 </summary>
    [SerializeField] float m_subWaitTime;

    /*プライベート　private*/
    /// <summary> 今の数値 </summary>
    float m_nowNum;
    /// <summary> １メモリ </summary>
    float m_oneMemory;
    /// <summary> ゲージサイズ </summary>
    RectTransform m_gaugeSize;
    /// <summary> 時間計測用 </summary>
    float m_timer;


    //コンストラクタ
    public GaugeTest() { }
    
    /// <summary>
    /// ゲージの初期設定
    /// </summary>
    public void SetGauge()
    {
        m_nowNum = m_maxNum;
        m_gaugeSize = m_gaugeObj.GetComponent<RectTransform>();
        //１メモリを設定
        m_oneMemory = m_gaugeSize.sizeDelta.x / m_maxNum;
        Debug.Log(m_nowNum);
    }

    /// <summary>
    /// 数値を返す
    /// </summary>
    /// <returns>m_nowNum今の数値</returns>
    public float RetrunNowNum()
    {
        return m_nowNum;
    }

    /// <summary>
    /// ゲージが残っていないか調べる
    /// </summary>
    /// <returns>残っていないtrue、残っているfalse</returns>
    public bool NonGauge()
    {
        return m_nowNum == 0;
    }

    /// <summary>
    /// ゲージを増やす
    /// </summary>
    /// <param name="_add_num">増やす数値</param>
    public void AddGauge(float _add_num)
    {
        //ゲージの増やす量を設定
        float add_memory = m_oneMemory * _add_num;

        //現在のゲージのサイズデータ取得
        Vector2 now_gauge_size = m_gaugeSize.sizeDelta;

        //現在のゲージにメモリを足す
        now_gauge_size.x += add_memory;

        //計算したゲージのサイズに設定
        m_gaugeSize.sizeDelta = now_gauge_size;

        //gaugeが最大値を超えたら初期化
        if (m_gaugeSize.sizeDelta.x > (m_maxNum * m_oneMemory))
        {
            now_gauge_size.x = m_maxNum * m_oneMemory;
            m_nowNum = m_maxNum;
            m_gaugeSize.sizeDelta = now_gauge_size;
        }

        //今の数値を設定
        m_maxNum += _add_num;
    }

    /// <summary>
    /// ゲージを減らす
    /// </summary>
    /// <param name="_sub_num">減らす値</param>
    public void SubGauge(float _sub_num)
    {
        //ゲージが０より大きければ実行
        if (m_gaugeSize.sizeDelta.x > 0)
        {
            //ゲージの減らす量を設定
            float sub_memory = m_oneMemory * _sub_num;

            //現在のゲージのサイズデータ取得
            Vector2 now_gauge_size = m_gaugeSize.sizeDelta;

            //現在のゲージの幅サイズからゲージの減らす量を引く
            now_gauge_size.x -= sub_memory;

            //計算したゲージのサイズに設定
            m_gaugeSize.sizeDelta = now_gauge_size;

            //今の数値を設定
            m_nowNum -= _sub_num;
        }

        //０を超えた場合サイズを０に戻す
        if (m_gaugeSize.sizeDelta.x <= 0)
        {
            //長さを調整
            m_gaugeSize.sizeDelta = new Vector2(0, m_gaugeSize.sizeDelta.y);
            m_nowNum = 0;
        }
    }

    /// <summary>
    /// 定期的にゲージを減らす
    /// </summary>
    /// <param name="_sub_num">減らす値</param>
    /// <param name="_wait_time">減らすまでの待機時間</param>
    public void SubGaugeFixed(float _sub_num)
    {
        m_timer += Time.deltaTime;

        //時間が来たらゲージを減らす
        if (m_timer >= m_subWaitTime)
        {
            m_timer = 0.0f;
            SubGauge(_sub_num);
        }
    }
}

