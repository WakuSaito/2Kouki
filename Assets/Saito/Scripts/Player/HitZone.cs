using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>パーツ当たり判定クラス</para>
/// コライダーごとにアタッチし、同じオブジェクトに複数回攻撃判定が当たらないようにする
/// </summary>
public class HitZone : MonoBehaviour
{
    /// <summary>
    /// 親スクリプト公開
    /// </summary>
    public HitMaster Master => m_master;
    HitMaster m_master;

    void Start()
    {
        m_master = GetComponentInParent<HitMaster>();
    }
}
