using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMaster : MonoBehaviour
{
    player playerScript;

    private void Awake()
    {
        playerScript = GetComponent<player>();
    }
    public void TakeDamage()
    {
        // �_���[�W���󂯂鏈���Ƃ�
        Debug.Log("Damage!");

        //�_���[�W�Ăяo��
        playerScript.DamagePlayer();
    }
}
