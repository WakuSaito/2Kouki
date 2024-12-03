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

    [SerializeField]//死亡時に生成するオブジェクト
    private GameObject dropItemPrefab;

    Rigidbody rb;

    public override void SetUpZombie()
    {
        //rigidbodyの取得
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// のけぞり
    /// </summary>
    public void KnockBack(Vector3 _vec)
    {
        Vector3.Normalize(_vec);//正規化
        //のけぞり
        rb.AddForce(_vec * knockBackPower, ForceMode.Impulse);
    }
    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Dead()
    {
        Debug.Log("ゾンビが死亡");

        ItemDrop();//アイテムドロップ

        Destroy(gameObject);//とりあえず即destroy
    }

    /// <summary>
    /// オブジェクトをドロップする
    /// </summary>
    private void ItemDrop()
    {
        //ドロップするアイテムの指定が無ければreturn
        if (dropItemPrefab == null) return;

        //とりあえず同じ位置にドロップ
        Vector3 dropPos = transform.position;
        //とりあえず埋まらないように

        //生成
        Instantiate(
            dropItemPrefab, 
            dropPos, 
            Quaternion.identity
            );
    }
}
