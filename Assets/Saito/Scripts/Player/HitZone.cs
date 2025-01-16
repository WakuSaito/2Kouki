using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
