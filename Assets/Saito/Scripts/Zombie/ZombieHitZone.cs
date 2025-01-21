using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ゾンビパーツ当たり判定クラス</para>
/// コライダーごとにアタッチし、同じオブジェクトに複数回攻撃判定が当たらないようにする
/// </summary>
public class ZombieHitZone : MonoBehaviour
{
    /// <summary>
    /// 親スクリプト公開
    /// </summary>
    public ZombieHitMaster Master => m_master;
    ZombieHitMaster m_master;

    void Start()
    {
        m_master = GetComponentInParent<ZombieHitMaster>();
    }
}
