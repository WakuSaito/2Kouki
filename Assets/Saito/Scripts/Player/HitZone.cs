using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    /// <summary>
    /// �e�X�N���v�g���J
    /// </summary>
    public HitMaster Master => master;
    HitMaster master;

    void Start()
    {
        master = GetComponentInParent<HitMaster>();
    }
}
