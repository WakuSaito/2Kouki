using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitZone : MonoBehaviour
{
    /// <summary>
    /// 親スクリプト公開
    /// </summary>
    public ZombieHitMaster Master => master;
    ZombieHitMaster master;

    void Start()
    {
        master = GetComponentInParent<ZombieHitMaster>();
    }
}
