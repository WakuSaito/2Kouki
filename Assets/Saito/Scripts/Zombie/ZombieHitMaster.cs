using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitMaster : MonoBehaviour
{
    ZombieManager m_zombieManager;

    private void Awake()
    {
        m_zombieManager = GetComponent<ZombieManager>();
    }
    /// <summary>
    /// ÚG‚µ‚½•”ˆÊ‚Ìƒ^ƒO‚ğ“n‚·
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
