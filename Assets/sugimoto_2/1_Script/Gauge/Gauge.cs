using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    //ゲージ
    [SerializeField] GameObject gauge_obj;
    [SerializeField] GameObject back_gauge_obj;

    //数値
    float gauge_num_max;//最大数値
    float gauge_num_now;//今の数値

    //1メモリ
    float gauge_one_memory;

    //持続的に減らす用
    float food_reduce_timer = 0;
    float damage_reduce_timer = 0;

    


    //共通関数
    public float GaugeSetting(float _max)//ゲージの設定
    {
        //最大数値設定
        gauge_num_max = _max;
        //今の数値設定
        gauge_num_now = gauge_num_max;
        //１メモリあたりを設定
        gauge_one_memory = gauge_obj.GetComponent<RectTransform>().sizeDelta.x / gauge_num_max;

        return gauge_num_now;
    }

    public float Increase_Gauge(float _increase_value)    //ゲージを増やす処理
    {
        //ゲージの増やす量を設定
        float _increase_gauge = gauge_one_memory * _increase_value;

        //現在のゲージのサイズデータ取得
        Vector2 _now_gauge_size = gauge_obj.GetComponent<RectTransform>().sizeDelta;

        //現在のゲージの幅サイズからゲージの減らす量を引く
        _now_gauge_size.x += _increase_gauge;

        //計算したゲージのサイズに設定
        gauge_obj.GetComponent<RectTransform>().sizeDelta = _now_gauge_size;

        //gaugeが最大値を超えたら初期化
        if (gauge_obj.GetComponent<RectTransform>().sizeDelta.x > gauge_num_max * gauge_one_memory) 
        {
            _now_gauge_size.x = gauge_num_max * gauge_one_memory;
            gauge_num_now = gauge_num_max;
            gauge_obj.GetComponent<RectTransform>().sizeDelta = _now_gauge_size;
        }

        //今の数値を設定
        gauge_num_now += _increase_value;

        //今のゲージの数値を返す
        return gauge_num_now;
    }

    public float ReduceGauge(float _reduce_value)     //ゲージを減らす処理
    {
        //ゲージが０より大きければ実行
        if (gauge_obj.GetComponent<RectTransform>().sizeDelta.x > 0)
        {
            //ゲージの減らす量を設定
            float _reduce_gauge = gauge_one_memory * _reduce_value;

            //現在のゲージのサイズデータ取得
            Vector2 _now_gauge_size = gauge_obj.GetComponent<RectTransform>().sizeDelta;

            //現在のゲージの幅サイズからゲージの減らす量を引く
            _now_gauge_size.x -= _reduce_gauge;

            //計算したゲージのサイズに設定
            gauge_obj.GetComponent<RectTransform>().sizeDelta = _now_gauge_size;

            //今の数値を設定
            gauge_num_now -= _reduce_value;
        }
        if (gauge_obj.GetComponent<RectTransform>().sizeDelta.x <= 0)
        //０を超えた場合
        {
            //長さを調整
            gauge_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, gauge_obj.GetComponent<RectTransform>().sizeDelta.y);
            gauge_num_now = 0;
        }

        //今のゲージの数値を返す
        return gauge_num_now;
    }


    //食料ゲージ用
    public float DurationReduce(float _timer,float _reduce_value)    //持続的にゲージを減らす
    {
        //タイマー増加
        food_reduce_timer += Time.deltaTime;

        //時間が来たらゲージを減らす
        if (food_reduce_timer >= _timer) 
        {
            food_reduce_timer = 0.0f;
            return ReduceGauge(_reduce_value);
        }

        return gauge_num_now;
    }

    //持続ダメージ
    public float DurationDamage(float _timer, float _reduce_value, GameObject _chack_gage_obj,GameObject _reduce_gage_obj)   //持続ダメージ
    {
        if (_chack_gage_obj.GetComponent<Gauge>().gauge_num_now <= 0) 
        {
            //タイマー増加
            damage_reduce_timer += Time.deltaTime;

            //時間が来たらゲージを減らす
            if (damage_reduce_timer >= _timer)
            {
                damage_reduce_timer = 0.0f;
                return _reduce_gage_obj.GetComponent<Gauge>().ReduceGauge(_reduce_value);            
            }
        }
        else
        {
            damage_reduce_timer = 0.0f;
        }

        return gauge_num_now;
    }

    /// <summary>
    /// 現在の数値取得
    /// </summary>
    public float GetCurrentAmount()
    {
        return gauge_num_now;
    }
}
