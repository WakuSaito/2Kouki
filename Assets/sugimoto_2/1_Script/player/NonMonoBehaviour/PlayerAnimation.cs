using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimation
{
    /*[SerializeField] インスペクターから設定*/
    /// <summary> アニメーターオブジェクト </summary>
    [SerializeField] GameObject m_animObj;

    /*プライベート　private*/
    /// <summary> アニメーター </summary>
    Animator m_anim;

    /// <summary>
    /// アニメーター取得
    /// </summary>
    public void SetAnim()
    {
        m_anim = m_animObj.GetComponent<Animator>();
    }

    /// <summary>
    /// 待機アニメーション
    /// </summary>
    public void Idle()
    {
        m_anim.SetBool("idle", true);

        m_anim.SetBool("Run", false);
        m_anim.SetBool("HandPislol", false);
    }

    /// <summary>
    /// 走るアニメーション
    /// </summary>
    public void Run()
    {
        m_anim.SetBool("Run", true);

        m_anim.SetBool("idle", false);
        m_anim.SetBool("HandPislol", false);
    }

    /// <summary>
    /// 銃を所持した状態の走るアニメーション
    /// </summary>
    public void GunHandRun()
    {
        m_anim.SetBool("HandPislol", true);

        m_anim.SetBool("Run", false);
        m_anim.SetBool("idle", false);
    }
}
