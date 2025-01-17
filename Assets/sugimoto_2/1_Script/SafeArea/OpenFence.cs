using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFence : MonoBehaviour
{
    bool m_inAreaFlag = false;

    //フェンスオブジェクト
    [SerializeField] Transform m_fenceTrans;
    [SerializeField] Transform m_fenceStopTrans;
    [SerializeField] Transform m_fenceCloseTrans;

    float m_timer = 0.0f;
    //animation速度
    [SerializeField] float m_speed = 3.0f;

    /// <summary>
    /// プレイヤーがフェンスの出入り口に入ったらフラグTRUE
    /// </summary>
    /// <param name="other">コライダーに当たっている</param>
    private void OnTriggerEnter(Collider other)
    {
        if (m_inAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            m_timer = 0.0f;
            m_inAreaFlag = true;
        }
    }

    /// <summary>
    /// プレイヤーがフェンスの出入り口から出たらフラグFALSE
    /// </summary>
    /// <param name="other">コライダーに当たっている</param>
    private void OnTriggerExit(Collider other)
    {
        if (!m_inAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            m_timer = 0.0f;
            m_inAreaFlag = false;
        }
    }

    /// <summary>
    /// フェンスの開閉処理
    /// </summary>
    void Update()
    {
        
        if (m_inAreaFlag)
        {
            //開ける

            m_timer += Time.deltaTime;

            //位置更新
            m_fenceTrans.position = Vector3.Lerp(m_fenceTrans.position, m_fenceStopTrans.position, m_timer * m_speed);
            m_fenceTrans.localRotation = Quaternion.Lerp(m_fenceTrans.localRotation, m_fenceStopTrans.localRotation, m_timer * m_speed);
        }
        else
        {
            //閉める

            m_timer += Time.deltaTime;

            //位置更新
            m_fenceTrans.position = Vector3.Lerp(m_fenceTrans.position, m_fenceCloseTrans.position, m_timer * m_speed);
            m_fenceTrans.localRotation = Quaternion.Lerp(m_fenceTrans.localRotation, m_fenceCloseTrans.localRotation, m_timer * m_speed);
        }
    }
}
