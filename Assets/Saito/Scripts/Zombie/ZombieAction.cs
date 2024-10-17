using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビの行動
/// クラス名は変更したほうがいいかも
/// </summary>
public class ZombieAction : ZombieBase
{
    [SerializeField]//のけぞりの力
    private float knockBackPower = 10.0f;

    Rigidbody rb;

    public override void SetUpZombie()
    {
        //rigidbodyの取得
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// のけぞり
    /// 後で引数にのけぞり方向を指定したい
    /// </summary>
    public void KnockBack()
    {
        //のけぞり
        rb.AddForce(transform.forward * -1.0f * knockBackPower, ForceMode.Impulse);
    }
    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Dead()
    {
        Debug.Log("ゾンビが死亡");
        Destroy(gameObject);
    }
}
