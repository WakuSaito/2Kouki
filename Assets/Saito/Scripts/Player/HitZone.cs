using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    /// <summary>
    /// �e�X�N���v�g���J
    /// </summary>
    public HitMaster Master => m_master;
    HitMaster m_master;

    void Start()
    {
        m_master = GetComponentInParent<HitMaster>();
    }
}
