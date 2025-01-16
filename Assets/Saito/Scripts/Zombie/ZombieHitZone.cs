using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
