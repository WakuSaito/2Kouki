using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitZone : MonoBehaviour
{
    /// <summary>
    /// �e�X�N���v�g���J
    /// </summary>
    public ZombieHitMaster Master => m_master;
    ZombieHitMaster m_master;

    void Start()
    {
        m_master = GetComponentInParent<ZombieHitMaster>();
    }
}
