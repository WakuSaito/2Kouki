using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ゾンビの行動</para>
/// クラス名は変更したほうがいいかも
/// </summary>
public class ZombieAction : ZombieBase
{
    [SerializeField]//のけぞりの力
    private float m_knockBackPower = 10.0f;

    [SerializeField]//死亡時に生成するオブジェクト
    private GameObject m_dropItemPrefab;

    Rigidbody m_rigidbody;

    public override void SetUpZombie()
    {
        //rigidbodyの取得
        m_rigidbody = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// のけぞり
    /// </summary>
    public void KnockBack(Vector3 _vec)
    {
        Vector3.Normalize(_vec);//正規化
        //のけぞり
        m_rigidbody.AddForce(_vec * m_knockBackPower, ForceMode.Impulse);
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
        if (m_dropItemPrefab == null) return;

        //とりあえず同じ位置にドロップ
        Vector3 drop_pos = transform.position;
        //とりあえず埋まらないように

        //生成
        Instantiate(
            m_dropItemPrefab,
            drop_pos, 
            Quaternion.identity
            );
    }
}
