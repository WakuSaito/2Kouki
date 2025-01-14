using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMove : DogBase
{
    [SerializeField]//走る速度
    float mRunSpeed = 6.0f;
    [SerializeField]//歩く速度
    float mWalkSpeed = 3.0f;

    [SerializeField]//振り向き速度
    float mTurnSpeed = 1000;

    //目標とする向き
    Quaternion mTargetRotation;

    Rigidbody mRigidbody;

    //初期設定
    public override void SetUpDog()
    {
        //rigidbodyの取得
        mRigidbody = GetComponent<Rigidbody>();
        mTargetRotation = transform.rotation;
    }

    private void Update()
    {
        //あまりManager以外でUpdateを使いたくないが、補間するため実装
        //向きを補間
        var qua = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mTurnSpeed * Time.deltaTime);

        //y軸以外を無視
        qua.x = 0.0f; qua.z = 0.0f;
        //向き変更
        transform.rotation = qua;
    }

    /// <summary>
    /// 指定向きに変更
    /// </summary>
    public void ChangeDirection(Quaternion _qua)
    {
        mTargetRotation = _qua;//目標の向きを設定
    }

    /// <summary>
    /// 指定した座標に向きを変更
    /// </summary>
    public void LookAtPosition(Vector3 _target_pos)
    {
        //座標の取得
        Vector3 pos = transform.position;
        Vector3 target_pos = _target_pos;
        //ベクトルを計算
        Vector3 direction = target_pos - pos;
        direction.y = 0;//y軸を考慮しない

        //ベクトルから向きを求める
        Quaternion target_rotation = Quaternion.LookRotation(direction, Vector3.up);

        //向きの変更
        //滑らかに向きを変更できるようにしたい
        ChangeDirection(target_rotation);
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
        mRigidbody.velocity = new Vector3(vec.x * mRunSpeed, mRigidbody.velocity.y, vec.z * mRunSpeed);
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
        mRigidbody.velocity = new Vector3(vec.x * mWalkSpeed, mRigidbody.velocity.y, vec.z * mWalkSpeed);
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    public void StopMove()
    {
        //移動ベクトルを0にする
        mRigidbody.velocity = new Vector3(0, mRigidbody.velocity.y, 0);
    }
}
