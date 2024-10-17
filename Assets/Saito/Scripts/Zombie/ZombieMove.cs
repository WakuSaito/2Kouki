using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビの移動用クラス
/// </summary>
public class ZombieMove : ZombieBase
{
    [SerializeField]//走る速度
    float run_speed = 6.0f;
    [SerializeField]//歩く速度
    float walk_speed = 1.0f;

    Rigidbody rb;

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpZombie()
    {
        //rigidbodyの取得
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 前方に歩く
    /// </summary>
    public void WalkFront()
    {
        //移動方向を求める
        Vector3 vec = transform.forward;
        vec.y = 0.0f;//y軸を無視する
        Vector3.Normalize(vec);

        //移動ベクトル更新
        rb.velocity = vec * walk_speed;
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
    /// 移動停止
    /// </summary>
    public void StopMove()
    {
        //移動ベクトルを0にする
        rb.velocity = Vector3.zero;
    }
   
}
