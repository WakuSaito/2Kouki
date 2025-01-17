using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    public bool m_inSafeAreaFlag = false;

    /// <summary>
    /// プレイヤーが安全エリアに入ったらフラグTRUE
    /// </summary>
    /// <param name="other">コライダーに当たっている</param>
    private void OnTriggerEnter(Collider other)
    {
        if (m_inSafeAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<player>().m_inSafeAreaFlag = true;
            m_inSafeAreaFlag = true;
        }
    }

    /// <summary>
    /// プレイヤーが安全エリアから出たらフラグFALSE
    /// </summary>
    /// <param name="other">コライダーに当たっている</param>
    private void OnTriggerExit(Collider other)
    {
        if (!m_inSafeAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<player>().m_inSafeAreaFlag = false;
            m_inSafeAreaFlag = false;
        }
    }
}
