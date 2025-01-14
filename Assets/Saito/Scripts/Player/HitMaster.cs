using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMaster : MonoBehaviour
{
    player mPlayerScript;
    TestPlayerManager mPlayerManager;

    private void Awake()
    {
        mPlayerScript = GetComponent<player>();
        mPlayerManager = GetComponent<TestPlayerManager>();
    }
    public void TakeDamage()
    {
        // �_���[�W���󂯂鏈���Ƃ�
        Debug.Log("Damage!");

        //�_���[�W�Ăяo��
        if (mPlayerScript != null)
            mPlayerScript.DamagePlayer();
        else if (mPlayerManager != null)
            mPlayerManager.Damaged();
    }
}
