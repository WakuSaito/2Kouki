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
        // ダメージを受ける処理とか
        Debug.Log("Damage!");

        //ダメージ呼び出し
        playerScript.DamagePlayer();
    }
}
