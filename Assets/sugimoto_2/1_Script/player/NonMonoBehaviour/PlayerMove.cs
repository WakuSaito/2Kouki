using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMove
{
    /*[SerializeField] インスペクターから設定*/
    /// <summary> リジットボディーオブジェクト </summary>
    [SerializeField] GameObject m_rbObj;
    /// <summary> 移動速度:歩き </summary>
    [SerializeField] float m_walkSpeed;
    /// <summary> 移動速度:走り </summary>
    [SerializeField] float m_runSpeed;


    /*プライベート　private*/
    /// <summary> リジットボディー </summary>
    Rigidbody m_rb;
    /// <summary> キー入力回数 </summary>
    int m_keyPushCnt = 0;
    /// <summary> 連続入力制限時間 </summary>
    float m_pushTimer = 0;
    /// <summary> 前に移動しているか </summary>
    bool m_forward_Flag = false;
    /// <summary> 走っているか </summary>
    bool m_runFlag = false;
    /// <summary> ベクトル </summary>
    Vector3 vec;

    public void SetMove()
    {
        m_rb = m_rbObj.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 前進ベクトル設定
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void MoveForwardVec(bool _phsh)
    {
        if (!_phsh) return;

        vec += m_rbObj.transform.forward;
        m_forward_Flag = true;
    }
    /// <summary>
    /// 後退ベクトル設定
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void MoveBackVec(bool _phsh)
    {
        if (!_phsh) return;
        vec -= m_rbObj.transform.forward;
    }
    /// <summary>
    /// 右ベクトル設定
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void MoveRightVec(bool _phsh)
    {
        if (!_phsh) return;
        vec += m_rbObj.transform.right;
    }
    /// <summary>
    /// 左ベクトル設定
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void MoveLeftVec(bool _phsh)
    {
        if (!_phsh) return;
        vec -= m_rbObj.transform.right;
    }

    /// <summary>
    /// ベクトルをVelocityに足し合わせる
    /// </summary>
    public void AddVelocityVec()
    {
        //斜め移動の速度を一定にするため正規化
        vec.Normalize();

        //移動
        if (m_runFlag&& m_forward_Flag)
        {
            m_rb.velocity = new Vector3(vec.x * m_runSpeed, m_rb.velocity.y, vec.z * m_runSpeed);
        }
        else
        {
            m_rb.velocity = new Vector3(vec.x * m_walkSpeed, m_rb.velocity.y, vec.z * m_walkSpeed);
        }

        //移動状態初期化
        m_forward_Flag = false;
        vec = Vector3.zero;
    }

    /// <summary>
    /// 走る準備：ダブル入力
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    /// <param name="_kye_code">入力するキー</param>
    public void SetUpRun(bool _phsh)
    {
        //動いていなければ初期化
        if (vec == Vector3.zero)
        {
            m_runFlag = false;
            m_keyPushCnt = 0;
        }


        //すでに走っていれば終了
        if (m_runFlag) return;

        //キー入力されたとき
        if (_phsh)
        {
            m_keyPushCnt++;

            //押してから時間を計測
            float check_time = Time.time - m_pushTimer;

            //前に入力されてから１秒経つまでに次の入力がなければキャンセル
            if (check_time <= 1)
            {
                m_runFlag = true;
            }
            else
            {
                Debug.Log("キャンセル");
                m_runFlag = false;
                m_keyPushCnt = 0;
                m_pushTimer = 0.0f;
            }

            m_pushTimer = Time.time;
        }
    }

    /// <summary>
    /// 走る準備：２つのキー入力
    /// </summary>
    /// <param name="_phsh1">１つ目のキー：入力されているか</param>
    /// <param name="_phsh2">２つ目のキー：入力されているか</param>
    /// <param name="_kye_code1">１つ目の入力するキー</param>
    /// <param name="_kye_code2">２つ目の入力するキー</param>
    public void SetUpRun(bool _phsh1,bool _phsh2)
    {
        //すでに走っていれば終了
        if (m_runFlag) return;

        //初期化
        m_runFlag = false;
        //入力されていなければ終了
        if (!_phsh1 || !_phsh2) return;

        m_runFlag = true;
    }

    /// <summary>
    /// 歩いているか
    /// 走っていないかつ移動している
    /// </summary>
    /// <returns>歩いていればtrue</returns>
    public bool WalkFlag()
    {
        return !RunFlag() && vec != Vector3.zero;
    }

    /// <summary>
    /// 走っているか
    /// </summary>
    /// <returns>走っていればtrue</returns>
    public bool RunFlag()
    {
        return m_runFlag && m_forward_Flag;
    }
}
