using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビ当たり判定マスタークラス
/// 子のHitZoneから呼び出され、プレイヤーの被ダメージ処理を呼び出す
/// </summary>
public class ZombieHitMaster : MonoBehaviour
{
    ZombieManager m_zombieManager;

    private void Awake()
    {
        m_zombieManager = GetComponent<ZombieManager>();
    }

    /// <summary>
    /// <para>ダメージを受けた</para>
    /// 接触部位ごとの被ダメージ処理を呼ぶ
    /// </summary>
    public void TakeDamage(string _part_tag, int _damage, Vector3 _hit_pos)
    {
        if(_part_tag == "Body")
        {
            m_zombieManager.DamageBody(_hit_pos, _damage);
        }
        else if(_part_tag == "Head")
        {
            m_zombieManager.DamageHead(_hit_pos, _damage);
        }

    }
}
