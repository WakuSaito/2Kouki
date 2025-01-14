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
        // ダメージを受ける処理とか
        Debug.Log("Damage!");

        //ダメージ呼び出し
        if (mPlayerScript != null)
            mPlayerScript.DamagePlayer();
        else if (mPlayerManager != null)
            mPlayerManager.Damaged();
    }
}
