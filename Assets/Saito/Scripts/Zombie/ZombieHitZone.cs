using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitZone : MonoBehaviour
{
    /// <summary>
    /// �e�X�N���v�g���J
    /// </summary>
    public ZombieHitMaster Master => master;
    ZombieHitMaster master;

    void Start()
    {
        master = GetComponentInParent<ZombieHitMaster>();
    }
}
