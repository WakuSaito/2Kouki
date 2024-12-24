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
        // ダメージを受ける処理とか
        Debug.Log("Damage!");

        //ダメージ呼び出し
        if (playerScript != null)
            playerScript.DamagePlayer();
        else if (playerManager != null)
            playerManager.Damaged();
    }
}
