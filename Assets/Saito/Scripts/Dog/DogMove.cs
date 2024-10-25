using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMove : DogBase
{
    [SerializeField]//走る速度
    float run_speed = 6.0f;
    [SerializeField]//歩く速度
    float walk_speed = 1.0f;

    Rigidbody rb;

    //初期設定
    public override void SetUpDog()
    {
        //rigidbodyの取得
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 指定向きに変更
    /// </summary>
    public void ChangeDirection(Quaternion _qua)
    {
        //y軸以外を無視
        _qua.x = 0.0f; _qua.z = 0.0f;
        //向き変更
        transform.rotation = _qua;
    }

    /// <summary>
    /// 指定したオブジェクトの方向に向きを変更
    /// </summary>
    public void LookAtObject(GameObject _targetObj)
    {
        if (_targetObj == null) return;//nullチェック

        //座標の取得
        Vector3 pos = transform.position;
        Vector3 target_pos = _targetObj.transform.position;
        //ベクトルを計算
        Vector3 direction = target_pos - pos;
        direction.y = 0;//y軸を考慮しない

        //ベクトルから向きを求める
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        //向きの変更
        //滑らかに向きを変更できるようにしたい
        ChangeDirection(targetRotation);
    }

    /// <summary>
    /// 前方に走る
    /// </summary>
    public void RunFront()
    {
        //移動方向を求める
        Vector3 vec = transform.forward;
        vec.y = 0.0f;//y軸を無視する
        Vector3.Normalize(vec);

        //移動ベクトル更新
        rb.velocity = vec * run_speed;
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    public void StopMove()
    {
        //移動ベクトルを0にする
        rb.velocity = Vector3.zero;
    }
}
