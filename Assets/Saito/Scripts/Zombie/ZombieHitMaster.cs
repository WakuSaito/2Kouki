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
    /// 接触した部位のタグを渡す
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
