using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitMaster : MonoBehaviour
{
    ZombieManager zombieManager;

    private void Awake()
    {
        zombieManager = GetComponent<ZombieManager>();
    }
    /// <summary>
    /// ÚG‚µ‚½•”ˆÊ‚Ìƒ^ƒO‚ğ“n‚·
    /// </summary>
    public void TakeDamage(string _partTag, int _damage, Vector3 _hitpos)
    {
        if(_partTag=="Body")
        {
            zombieManager.DamageBody(_hitpos, _damage);
        }
        else if(_partTag == "Head")
        {
            zombieManager.DamageHead(_hitpos, _damage);
        }

    }
}
