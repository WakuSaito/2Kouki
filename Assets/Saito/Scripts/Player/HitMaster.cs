using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMaster : MonoBehaviour
{
    player playerScript;
    TestPlayerManager playerManager;

    private void Awake()
    {
        playerScript = GetComponent<player>();
        playerManager = GetComponent<TestPlayerManager>();
    }
    public void TakeDamage()
    {
        // �_���[�W���󂯂鏈���Ƃ�
        Debug.Log("Damage!");

        //�_���[�W�Ăяo��
        if (playerScript != null)
            playerScript.DamagePlayer();
        else if (playerManager != null)
            playerManager.Damaged();
    }
}
