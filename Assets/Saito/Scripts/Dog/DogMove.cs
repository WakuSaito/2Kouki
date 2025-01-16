using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 犬移動クラス
/// 犬の移動や回転を管理し実行する
/// </summary>
public class DogMove : DogBase
{
    [SerializeField]//走る速度
    float m_runSpeed = 6.0f;
    [SerializeField]//歩く速度
    float m_walkSpeed = 3.0f;

    [SerializeField]//振り向き速度
    float m_turnSpeed = 1000;

    //目標とする向き
    Quaternion m_targetRotation;

    Rigidbody m_rigidbody;

    //初期設定
    //コンポーネント、初期状態の取得
    public override void SetUpDog()
    {
        //rigidbodyの取得
        m_rigidbody = GetComponent<Rigidbody>();
        m_targetRotation = transform.rotation;
    }

    // Manager以外でUpdateを使いたくないが、回転を補間するため実装
    private void Update()
    {
        //向きを補間
        var qua = Quaternion.RotateTowards(transform.rotation, m_targetRotation, m_turnSpeed * Time.deltaTime);

        //y軸以外を無視
        qua.x = 0.0f; qua.z = 0.0f;
        //向き変更
        transform.rotation = qua;
    }

    /// <summary>
    /// 向きの決定
    /// 実際の回転はUpdateで実行する
    /// </summary>
    /// <param name="_qua">向く方向</param>
    public void ChangeDirection(Quaternion _qua)
    {
        m_targetRotation = _qua;//目標の向きを設定
    }

    /// <summary>
    /// 指定した座標を向く
    /// 注視したい座標から角度を計算し向きを決める
    /// </summary>
    /// ///<param name="_target_pos">注視する座標</param>
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
    /// Rigidbodyのベクトルを設定する
    /// </summary>
    public void RunFront()
    {
        //移動方向を求める
        Vector3 vec = transform.forward;
        vec.y = 0.0f;//y軸を無視する
        Vector3.Normalize(vec);

        //移動ベクトル更新
        m_rigidbody.velocity = new Vector3(vec.x * m_runSpeed, m_rigidbody.velocity.y, vec.z * m_runSpeed);
    }

    /// <summary>
    /// 前方に歩く
    /// Rigidbodyのベクトルを設定する
    /// </summary>
    public void WalkFront()
    {
        //移動方向を求める
        Vector3 vec = transform.forward;
        vec.y = 0.0f;//y軸を無視する
        Vector3.Normalize(vec);

        //移動ベクトル更新
        m_rigidbody.velocity = new Vector3(vec.x * m_walkSpeed, m_rigidbody.velocity.y, vec.z * m_walkSpeed);
    }

    /// <summary>
    /// 移動停止
    /// Rigidbodyのベクトルを0にする
    /// </summary>
    public void StopMove()
    {
        //移動ベクトルを0にする
        m_rigidbody.velocity = new Vector3(0, m_rigidbody.velocity.y, 0);
    }
}
