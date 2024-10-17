using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    /// <summary>
    /// 親スクリプト公開
    /// </summary>
    public HitMaster Master => master;
    HitMaster master;

    void Start()
    {
        master = GetComponentInParent<HitMaster>();
    }
}
