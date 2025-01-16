using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMaster : MonoBehaviour
{
    player m_playerScript;
    TestPlayerManager m_playerManager;

    private void Awake()
    {
        m_playerScript = GetComponent<player>();
        m_playerManager = GetComponent<TestPlayerManager>();
    }
    public void TakeDamage()
    {
        // ダメージを受ける処理とか
        Debug.Log("Damage!");

        //ダメージ呼び出し
        if (m_playerScript != null)
            m_playerScript.DamagePlayer();
        else if (m_playerManager != null)
            m_playerManager.Damaged();
    }
}
