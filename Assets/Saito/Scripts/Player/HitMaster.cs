using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定マスタークラス
/// 子のHitZoneから呼び出され、プレイヤーの被ダメージ処理を呼び出す
/// </summary>
public class HitMaster : MonoBehaviour
{
    player m_playerScript;
    TestPlayerManager m_playerManager;

    //コンポーネント取得
    private void Awake()
    {
        m_playerScript = GetComponent<player>();
        m_playerManager = GetComponent<TestPlayerManager>();
    }

    /// <summary>
    /// <para>ダメージを受けた</para>
    /// プレイヤーの被ダメージ処理を呼ぶ
    /// </summary>
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
